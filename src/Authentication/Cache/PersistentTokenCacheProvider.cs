namespace AutoBrew.PowerShell.Cache
{
    using Azure.Identity;
    using Microsoft.Identity.Client;
    using Microsoft.Identity.Client.Extensions.Msal;

    /// <summary>
    /// Provides the ability to cache tokens that are persisted by local storage.
    /// </summary>
    public class PersistentTokenCacheProvider : TokenCacheProvider
    {
        /// <summary>
        /// The key chain service for the token cache.
        /// </summary>
        private const string MsalTokenCacheKeychainService = "Microsoft.Developer.IdentityService";

        /// <summary>
        /// The keyring collection for the token cache.
        /// </summary>
        private const string MsalTokenCacheKeyringCollection = "default";

        /// <summary>
        /// The name for the file that will be used to persist the token cache.
        /// </summary>
        private const string MsalTokenCacheName = "msal.cache";

        /// <summary>
        /// The first keyring attribute for the token cache.
        /// </summary>
        private static readonly KeyValuePair<string, string> MsaltokenCacheKeyringAttribute1 =
            new("MsalClientID", "Microsoft.Developer.IdentityService");

        /// <summary>
        /// The second keyring attribute for the token cache.
        /// </summary>
        private static readonly KeyValuePair<string, string> MsaltokenCacheKeyringAttribute2 =
            new("Microsoft.Developer.IdentityService", "1.0.0.0");

        /// <summary>
        /// The directory for the msal.cache file.
        /// </summary>
        private static readonly string MsalTokenCacheDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".IdentityService");

        /// <summary>
        /// The options that control the storage of the token cache.
        /// </summary>
        private readonly TokenCachePersistenceOptions persistenceOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentTokenCacheProvider" /> class.
        /// </summary>
        /// <param name="options">The options that control the storage of the token cache.</param>
        public PersistentTokenCacheProvider(TokenCachePersistenceOptions options = null)
        {
            persistenceOptions = options ?? new TokenCachePersistenceOptions()
            {
                UnsafeAllowUnencryptedStorage = true
            };
        }

        /// <summary>
        /// Gets the data from the token cache.
        /// </summary>
        /// <returns>An array of bytes that represent the data from the cache.</returns>
        internal override async Task<byte[]> GetCacheDataAsync()
        {
            MsalCacheHelper msalCacheHelper = await GetCacheHelperAsync().ConfigureAwait(false);

            return msalCacheHelper.LoadUnencryptedTokenCache();
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
        /// Registers a token cache to synchronize with the appropriate storage.
        /// </summary>
        /// <param name="tokenCache">The token cache to be registered.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        public override async Task RegisterCacheAsync(ITokenCache tokenCache)
        {
            MsalCacheHelper msalCacheHelper;

            tokenCache.AssertNotNull(nameof(tokenCache));

            msalCacheHelper = await GetCacheHelperAsync().ConfigureAwait(false);
            msalCacheHelper.RegisterCache(tokenCache);
        }

        /// <summary>
        /// Verifies the underlying persistence mechanism for the token cache.
        /// </summary>
        /// <returns><c>true</c> if the underlying persistence mechanism was successfully verified; otherwise, <c>false</c>.</returns>
        public override async Task<bool> VerifyPersistenceAsync()
        {
            MsalCacheHelper helper;

            try
            {
                helper = await GetCacheHelperAsync().ConfigureAwait(false);
                helper.VerifyPersistence();

                return true;
            }
            catch (MsalCachePersistenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get an aptly configured instance of the <see cref="MsalCacheHelper" /> class.
        /// </summary>
        /// <returns>An aptly configured instance of the <see cref="MsalCacheHelper" /> class.</returns>
        private async Task<MsalCacheHelper> GetCacheHelperAsync()
        {
            MsalCacheHelper msalCacheHelper;

            try
            {
                msalCacheHelper = await GetProtectedCacheHelperAsync().ConfigureAwait(false);
                msalCacheHelper.VerifyPersistence();
            }
            catch (MsalCachePersistenceException)
            {
                if (persistenceOptions.UnsafeAllowUnencryptedStorage)
                {
                    msalCacheHelper = await GetFallbackCacheHelperAsync().ConfigureAwait(false);
                    msalCacheHelper.VerifyPersistence();
                }
                else
                {
                    throw;
                }
            }

            return msalCacheHelper;
        }

        /// <summary>
        /// Gets the fallback cache helper if the protected cache failed to be verified.
        /// </summary>
        /// <returns>The fallback cache helper if the protected cache failed to be verified.</returns>
        private async Task<MsalCacheHelper> GetFallbackCacheHelperAsync()
        {
            StorageCreationProperties storageProperties =
                new StorageCreationPropertiesBuilder(MsalTokenCacheName, MsalTokenCacheDirectory)
                    .WithMacKeyChain(MsalTokenCacheKeychainService, MsalTokenCacheName)
                    .WithLinuxUnprotectedFile()
                    .Build();

            return await MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an instance of the <see cref="MsalCacheHelper" /> class.
        /// </summary>
        /// <returns>An instance of the <see cref="MsalCacheHelper" /> class.</returns>
        private async Task<MsalCacheHelper> GetProtectedCacheHelperAsync()
        {
            StorageCreationProperties storageProperties =
                new StorageCreationPropertiesBuilder(MsalTokenCacheName, MsalTokenCacheDirectory)
                    .WithMacKeyChain(MsalTokenCacheKeychainService, MsalTokenCacheName)
                    .WithLinuxKeyring(
                        MsalTokenCacheName,
                        MsalTokenCacheKeyringCollection,
                        MsalTokenCacheName,
                        MsaltokenCacheKeyringAttribute1,
                        MsaltokenCacheKeyringAttribute2)
                    .Build();

            return await MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false);
        }
    }
}