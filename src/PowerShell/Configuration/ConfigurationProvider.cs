namespace AutoBrew.PowerShell.Configuration
{
    using System.Collections.Concurrent;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Microsoft.Extensions.Configuration;
    using Models.Configuration;

    /// <summary>
    /// Provides configurations used throughout the module.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        /// The name for the directory that will be used to persist the configuration.
        /// </summary>
        private const string directoryname = ".AutomationBrew";

        /// <summary>
        /// The name for the file that will be used to persist the configuration.
        /// </summary>
        private const string filename = "PSConfig.json";

        /// <summary>
        /// The collection of configuration definitions used by the module.
        /// </summary>
        private readonly IDictionary<string, ConfigurationDefinition> definitionMap = new ConcurrentDictionary<string, ConfigurationDefinition>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The collection of data used to initialize the in-memory configuration provider.
        /// </summary>
        private readonly IDictionary<string, string> initialData = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// The lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.
        /// </summary>
        private readonly ReaderWriterLockSlim resourceLock = new(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// The path for the configuration file.
        /// </summary>
        private readonly string configFilePath;

        /// <summary>
        /// The root resource that makes it possible to interact with the configuration sources.
        /// </summary>
        private IConfigurationRoot configurationRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationProvider" /> class.
        /// </summary>
        /// <param name="filepath">The path for the configuration file.</param>
        /// <exception cref="ArgumentException">
        /// The filepath parameter is empty or null.
        /// </exception>
        private ConfigurationProvider(string filepath)
        {
            filepath.AssertNotEmpty(nameof(filepath));

            ValidateConfigurationContent(filepath);

            configFilePath = filepath;
        }

        /// <summary>
        /// Gets the value for the configuration with the specified key.
        /// </summary>
        /// <typeparam name="TValue">Tye type for the value of the configuration.</typeparam>
        /// <param name="key">The key for the configuration.</param>
        /// <returns>
        /// The value for the configuration with the specified key if defined; otherwise, the default value for the configuration.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The key parameter is empty or null.
        /// or 
        /// The configuration definition with [{key}] as the key is not registered.
        /// </exception>
        public (Type ProviderType, TValue Value) GetConfigurationValue<TValue>(string key)
        {
            key.AssertNotEmpty(nameof(key));

            if (definitionMap.TryGetValue(key, out ConfigurationDefinition definition) == false)
            {
                throw new ArgumentException($"The configuration definition with [{key}] as the key is not registered.", nameof(key));
            }

            IConfigurationSection section = configurationRoot.GetSection(definition.Category);

            foreach (Microsoft.Extensions.Configuration.IConfigurationProvider provider in configurationRoot.Providers)
            {
                if (provider.TryGet($"{definition.Category}:{key}", out _))
                {
                    return (provider.GetType(), section.GetValue(key, (TValue)definition.DefaultValue));
                }
            }

            return (null, (TValue)definition.DefaultValue);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ConfigurationProvider" /> class.
        /// </summary>
        /// <returns>An instance of the <see cref="ConfigurationProvider" /> class that is ready for use.</returns>
        public static IConfigurationProvider Initialize()
        {
            IConfigurationProvider provider = new ConfigurationProvider(GetConfigurationFilePath());

            provider.Build();

            return provider;
        }

        /// <summary>
        /// Gets the list of configurations that have been defined.
        /// </summary>
        /// <returns>The list of configuations that have been defined.</returns>
        public IList<ConfigurationData> ListConfiguration()
        {
            List<ConfigurationData> results = new();

            foreach (IConfigurationSection categorySection in configurationRoot.GetChildren())
            {
                foreach (IConfigurationSection configurationSection in categorySection.GetChildren())
                {
                    if (definitionMap.TryGetValue(configurationSection.Key, out ConfigurationDefinition definition))
                    {
                        (Type ProviderType, object Value) = GetConfigurationValue<object>(configurationSection.Key);

                        if (Value != null)
                        {
                            results.Add(new ConfigurationData(definition, GetConfigurationScope(ProviderType), Value));
                        }
                    }
                }
            }

            results.AddRange(definitionMap
                .Where(d => results.SingleOrDefault(c => c.Definition.Key.Equals(d.Key, StringComparison.OrdinalIgnoreCase)) == null)
                .Select(d => new ConfigurationData(d.Value, ConfigurationScope.Default, d.Value.DefaultValue)));

            return results;
        }

        /// <summary>
        /// Builds the configuration from the set of registered sources.
        /// </summary>
        public void Build()
        {
            resourceLock.EnterReadLock();

            try
            {
                configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(initialData).AddJsonFile(configFilePath).Build();
            }
            finally
            {
                resourceLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Registers the configuration definition for use throughout the module.
        /// </summary>
        /// <param name="definition">The defintion of the configuration to be registered.</param>
        /// <exception cref="ModuleException">There is a definition with the key {key} already registered.</exception>
        public void RegisterDefinition(ConfigurationDefinition definition)
        {
            definition.AssertNotNull(nameof(definition));

            if (definitionMap.ContainsKey(definition.Key))
            {
                throw new ModuleException($"There is a definition with the key {definition.Key} already registered.", ModuleExceptionCategory.Configuration);
            }

            definitionMap[definition.Key] = definition;
        }

        /// <summary>
        /// Updates the specified configuration.
        /// </summary>
        /// <typeparam name="TValue">The type for the value of the configuration.</typeparam>
        /// <param name="key">The key for the configuration.</param>
        /// <param name="scope">The scope for the configuration.</param>
        /// <param name="value">The value for the configuration.</param>
        /// <exception cref="ArgumentException">
        /// The key parameter is empty or null.
        /// or 
        /// Unexpected value type [{Type}]. The value of the configuration [{Key}] should be of type [{ValueType}].
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The value parameter is null.
        /// </exception>
        public void UpdateConfiguration<TValue>(string key, ConfigurationScope scope, TValue value)
        {
            key.AssertNotEmpty(nameof(key));
            value.AssertNotNull(nameof(value));

            if (definitionMap.TryGetValue(key, out ConfigurationDefinition definition) == false)
            {
                throw new ArgumentException($"The configuration definition with [{key}] as the key is not registered.", nameof(key));
            }

            try
            {
                resourceLock.EnterWriteLock();

                definition.Validate(value);

                if (scope == ConfigurationScope.CurrentUser)
                {
                    JsonNode node = JsonNode.Parse(File.ReadAllText(configFilePath),
                        new JsonNodeOptions { PropertyNameCaseInsensitive = true },
                        new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });

                    JsonNode category = node[definition.Category];

                    if (category == null)
                    {
                        node[definition.Category] = new JsonObject { [key] = JsonSerializer.SerializeToNode(value) };
                    }
                    else
                    {
                        node[definition.Category][key] = JsonSerializer.SerializeToNode(value);
                    }

                    File.WriteAllText(configFilePath, node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                }
                else if (scope == ConfigurationScope.Process)
                {
                    initialData[$"{definition.Category}:{key}"] = value.ToString();
                }

                Build();
            }
            finally
            {
                resourceLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the path for the configuration file.
        /// </summary>
        /// <returns>The path for the configuration file.</returns>
        /// <exception cref="ModuleException">TODO</exception>
        private static string GetConfigurationFilePath()
        {
            List<string> paths = new()
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), directoryname, filename),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), directoryname, filename)
            };

            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            foreach (string path in paths)
            {
                try
                {
                    Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
                    ResetConfigurationContent(path);

                    return path;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            throw new ModuleException("Unable to store the configuration file.", ModuleExceptionCategory.Configuration);
        }

        /// <summary>
        /// Gets the scope for the configuration based on the specified provider type.
        /// </summary>
        /// <param name="providerType">The type of provider for the configuration.</param>
        /// <returns>The scope for the configuration.</returns>
        private static ConfigurationScope GetConfigurationScope(Type providerType)
        {
            if (providerType == typeof(Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider))
            {
                return ConfigurationScope.CurrentUser;
            }
            else if (providerType == typeof(Microsoft.Extensions.Configuration.Memory.MemoryConfigurationProvider))
            {
                return ConfigurationScope.Process;
            }

            return ConfigurationScope.Default;
        }

        /// <summary>
        /// Resets the content of the configuration file.
        /// </summary>
        /// <param name="filepath">The path for the configuration file to be reset.</param>
        private static void ResetConfigurationContent(string filepath)
        {
            File.WriteAllText(filepath, "{}");
        }

        /// <summary>
        /// Validates the content of the configuration file.
        /// </summary>
        /// <param name="filepath">The path for the configuration file to be validated.</param>
        private static void ValidateConfigurationContent(string filepath)
        {
            bool isValidJson = true;
            string value = File.Exists(filepath) ? File.ReadAllText(filepath) : string.Empty;

            try
            {
                JsonDocument.Parse(value);
            }
            catch
            {
                isValidJson = false;
            }

            if (string.IsNullOrEmpty(value) || isValidJson == false)
            {
                ResetConfigurationContent(filepath);
            }
        }
    }
}