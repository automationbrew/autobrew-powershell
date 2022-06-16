namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Cache;
    using Configuration;
    using Factories;
    using Models;
    using Models.Configuration;
    using Properties;

    /// <summary>
    /// Initializes components for use throughout the module.
    /// </summary>
    public class ModuleInitializer : IModuleAssemblyInitializer
    {
        /// <summary>
        /// Performs the actions to initialize the module when it has been imported.
        /// </summary>
        public void OnImport()
        {
            if (ModuleSession.Instance.AuthenticationFactory == null)
            {
                ModuleSession.Instance.AuthenticationFactory = new AuthenticationFactory();
            }

            MsalHttpClientFactory.Initialize();

            InitalizeConfiguration();
            InitializeTokenCacheAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Initializes the token cache provider for use by the module.
        /// </summary>
        /// <param name="localStorage">A flag that indicates if local storage should be used to persist tokens.</param>
        /// <returns>An instance of the <see cref="Task" /> that represents the asynchronous operation.</returns>
        public static async Task InitializeTokenCacheAsync(bool localStorage = true)
        {
            TokenCacheProvider provider = localStorage ?
                new PersistentTokenCacheProvider() : new InMemoryTokenCacheProvider();

            if (localStorage)
            {
                if (await provider.VerifyPersistenceAsync().ConfigureAwait(false) == false)
                {
                    provider = new InMemoryTokenCacheProvider();
                }
            }

            ModuleSession.Instance.RegisterComponent(ComponentType.TokenCache, () => provider, true);
        }

        /// <summary>
        /// Initializes the configuration provider for use by the module.
        /// </summary>
        public void InitalizeConfiguration()
        {
            IConfigurationProvider provider = ConfigurationProvider.Initialize();

            provider.RegisterDefinition(new TypedConfigurationDefinition<bool>(
                ConfigurationCategory.Telemetry,
                true,
                Resources.DataCollectionConfigurationDefinition,
                ConfigurationKey.DataCollection));

            ModuleSession.Instance.RegisterComponent(ComponentType.Configuration, () => provider, true);
        }
    }
}