namespace AutoBrew.PowerShell.Cache
{
    using Azure.Identity;
    using Microsoft.Identity.Client;

    /// <summary>
    /// Provides the ability to cache tokens that are persisted in memory.
    /// </summary>
    public class InMemoryTokenCacheProvider : TokenCacheProvider
    {
        /// <summary>
        /// The options that are used to control the in-memory token cache.
        /// </summary>
        private readonly InMemoryTokenCacheOptions persistenceOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryTokenCacheProvider" /> class.
        /// </summary>
        /// <param name="options">The options that are used to control the in-memory token cache.</param>
        public InMemoryTokenCacheProvider(InMemoryTokenCacheOptions options = null)
        {
            persistenceOptions = options ?? new InMemoryTokenCacheOptions();
        }

        /// <summary>
        /// Gets the data from the token cache.
        /// </summary>
        /// <returns>An array of bytes that represent the data from the cache.</returns>
        internal override async Task<byte[]> GetCacheDataAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            return persistenceOptions.TokenCache.ToArray();
        }

        /// <summary>
        /// Gets the persistence options for the token cache provider.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="TokenCachePersistenceOptions" /> that represents the persistence options for the provider.
        /// </returns>
        public override TokenCachePersistenceOptions GetPersistenceOptions()
        {
            return persistenceOptions;
        }

        /// <summary>
        /// Registers a token cache to synchronize with the approriate storage.
        /// </summary>
        /// <param name="tokenCache">The token cache to be registered.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        public override async Task RegisterCacheAsync(ITokenCache tokenCache)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}