namespace AutoBrew.PowerShell.Models
{
    using System.Collections.Concurrent;
    using Factories;
    using Models.Authentication;

    /// <summary>
    /// Represents the current session for the module.
    /// </summary>
    public sealed class ModuleSession
    {
        /// <summary>
        /// Provides a singleton instance of the <see cref="ModuleSession" /> class.
        /// </summary>
        private static readonly Lazy<ModuleSession> session = new();

        /// <summary>
        /// Provides a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.
        /// </summary>
        private static readonly ReaderWriterLockSlim resourceLock = new(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Provides the components that have been registered.
        /// </summary>
        private readonly IDictionary<ComponentType, object> componentRegistry = new ConcurrentDictionary<ComponentType, object>();

        /// <summary>
        /// Provides the environments that have been registered.
        /// </summary>
        private readonly IDictionary<string, ModuleEnvironment> environmentRegistry = new ConcurrentDictionary<string, ModuleEnvironment>();

        /// <summary>
        /// Gets or sets the factory used to perform authentication requests.
        /// </summary>
        public IAuthenticationFactory AuthenticationFactory { get; set; }

        /// <summary>
        /// Gets or sets the factory to used to create clients that can interact with online services.
        /// </summary>
        public IClientFactory ClientFactory { get; set; }

        /// <summary>
        /// Gets or sets the module context.
        /// </summary>
        public ModuleContext Context { get; set; }

        /// <summary>
        /// Gets the instance of the <see cref="ModuleSession" /> class.
        /// </summary>
        public static ModuleSession Instance
        {
            get
            {
                resourceLock.EnterReadLock();

                try
                {
                    return session.Value;
                }
                finally
                {
                    resourceLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets a collection of environments that have been registered.
        /// </summary>
        /// <returns>A collection of environments that have been registered.</returns>
        public IEnumerable<ModuleEnvironment> ListEnvironments()
        {
            return environmentRegistry.Values;
        }

        /// <summary>
        /// Gets the component associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of component in the registry.</typeparam>
        /// <param name="key">The key for the component.</param>
        /// <param name="component">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the component registry contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public bool TryGetComponent<T>(ComponentType key, out T component) where T : class
        {
            key.AssertNotNull(nameof(key));

            bool flag = componentRegistry.TryGetValue(key, out object value);

            component = value as T ?? default;

            return flag;
        }

        /// <summary>
        /// Gets the environment associated with the specified name.
        /// </summary>
        /// <param name="name">The name for the environment in the registry.</param>
        /// <param name="environment">When this function returns, the instance of the <see cref="ModuleEnvironment" /> class associated with the specified name if found; otherwise, the default value.</param>
        /// <returns><c>true</c> if the registry contains an environment with the specified name; otherwise, <c>false</c></returns>
        public bool TryGetEnvironment(string name, out ModuleEnvironment environment)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = ModuleEnvironmentConstants.AzureCloud;
            }

            return environmentRegistry.TryGetValue(name, out environment);
        }

        /// <summary>
        /// Initializes the session for use by the module.
        /// </summary>
        public void Initialize()
        {
            if (environmentRegistry.ContainsKey(ModuleEnvironmentConstants.AzureCloud) == false)
            {
                environmentRegistry.Add(
                    ModuleEnvironmentConstants.AzureCloud,
                    new ModuleEnvironment
                    {
                        ActiveDirectoryAuthority = ModuleEnvironmentConstants.ActiveDirectoryAuthority,
                        ApplicationId = ModuleEnvironmentConstants.ApplicationId,
                        MicrosoftGraphEndpoint = ModuleEnvironmentConstants.MicrosoftGraphEndpoint,
                        MicrosoftPartnerCenterEndpoint = ModuleEnvironmentConstants.MicrosoftPartnerCenterEndpoint,
                        Name = ModuleEnvironmentConstants.AzureCloud,
                        Tenant = ModuleEnvironmentConstants.Tenant,
                        Type = ModuleEnvironmentType.BuiltIn
                    });
            }
        }

        /// <summary>
        /// Registers a component with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of component in the registry.</typeparam>
        /// <param name="key">The key for the component.</param>
        /// <param name="initializer">The method used to initialize the component.</param>
        /// <param name="overwrite">A flag that indicates if the component should be overwrite an existing registration.</param>
        public void RegisterComponent<T>(ComponentType key, Func<T> initializer, bool overwrite = false) where T : class
        {
            initializer.AssertNotNull(nameof(initializer));

            if (!componentRegistry.ContainsKey(key) || overwrite)
            {
                componentRegistry[key] = initializer();
            }
        }

        /// <summary>
        /// Registers an environment with the specified name.
        /// </summary>
        /// <param name="name">The name for the environment to register.</param>
        /// <param name="environment">The instance of the <see cref="ModuleEnvironment" /> class to register.</param>
        /// <param name="overwrite">The flag that indicates whether to overwrite an existing environment with the same name.</param>
        /// <exception cref="ArgumentException">
        /// The name parameter is empty or null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The environment parameter is null.
        /// </exception>
        /// <exception cref="ModuleException">
        /// The environment {name} with cannot be registered. Only user defined environments can be registered.
        /// </exception>
        public void RegisterEnvironment(string name, ModuleEnvironment environment, bool overwrite = false)
        {
            name.AssertNotEmpty(nameof(name));
            environment.AssertNotNull(nameof(environment));

            if (environment.Type == ModuleEnvironmentType.BuiltIn)
            {
                throw new ModuleException($"The environment {name} with cannot be registered. Only user defined environments can be registered.");
            }

            if (environmentRegistry.ContainsKey(name) == false || overwrite)
            {
                environmentRegistry[name] = environment;
            }
        }

        /// <summary>
        /// Unregisters the component with the specified key.
        /// </summary>
        /// <param name="key">The key for the component.</param>
        public void UnregisterComponent(ComponentType key)
        {
            if (componentRegistry.ContainsKey(key))
            {
                componentRegistry.Remove(key);
            }
        }

        /// <summary>
        /// Unregisters the environment with the specified name.
        /// </summary>
        /// <param name="name">The name of the environment to be unregistered.</param>
        /// <exception cref="ArgumentException">
        /// The name parameter is empty or null.
        /// </exception>
        public void UnregisterEnvironment(string name)
        {
            name.AssertNotEmpty(nameof(name));

            if (environmentRegistry.ContainsKey(name) == false)
            {
                return;
            }

            if (environmentRegistry[name].Type == ModuleEnvironmentType.BuiltIn)
            {
                throw new ModuleException($"The environment {name} cannot be unregistered because it is a builtin environment.");
            }

            environmentRegistry.Remove(name);
        }
    }
}