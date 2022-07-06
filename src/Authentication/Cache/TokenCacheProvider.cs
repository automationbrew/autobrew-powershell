namespace AutoBrew.PowerShell.Cache
{
    using System.Security;
    using System.Text.Json.Nodes;
    using Azure.Identity;
    using Microsoft.Identity.Client;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Provides the ability to interact with the token cache.
    /// </summary>
    public abstract class TokenCacheProvider
    {
        /// <summary>
        /// Gets the data from the token cache.
        /// </summary>
        /// <returns>An array of bytes that represent the data from the cache.</returns>
        internal abstract Task<byte[]> GetCacheDataAsync();

        /// <summary>
        /// Gets the persistence options for the token cache provider.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="TokenCachePersistenceOptions" /> that represents the persistence options for the provider.
        /// </returns>
        public abstract TokenCachePersistenceOptions GetPersistenceOptions();

        /// <summary>
        /// Gets the refresh token associated with the specified client and home account.
        /// </summary>
        /// <param name="clientId">The identifier for the client used to request the access token.</param>
        /// <param name="homeAccountId">The identifier of the home account for the user.</param>
        /// <returns>The refresh token associated with the specified client and home account if discovered; otherwise, null.</returns>
        /// <exception cref="ArgumentException">
        /// The clientId parameter is empty or null.
        /// or
        /// The homeAccountId parameter is empty or null.
        /// </exception>
        public async Task<SecureString> GetRefreshTokenAsync(string clientId, string homeAccountId)
        {
            clientId.AssertNotEmpty(nameof(clientId));
            homeAccountId.AssertNotEmpty(nameof(homeAccountId));

            byte[] buffer = await GetCacheDataAsync().ConfigureAwait(false);

            if (buffer.Length <= 0)
            {
                return null;
            }

            using Stream stream = new MemoryStream(buffer);
            JsonNode node = JsonNode.Parse(stream, new JsonNodeOptions { PropertyNameCaseInsensitive = true });

            if (clientId.Equals(ModuleEnvironmentConstants.ApplicationId))
            {
                return node["RefreshToken"]?[$"{homeAccountId}-login.windows.net-refreshtoken-1--"]?["secret"]?.ToString().AsSecureString();
            }

            return node["RefreshToken"]?[$"{homeAccountId}-login.windows.net-refreshtoken-{clientId}--"]?["secret"]?.ToString().AsSecureString();
        }

        /// <summary>
        /// Removes the specified account from the token cache.
        /// </summary>
        /// <param name="moduleAccount">The module account to be removed from the token cache.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// The moduleAccount parameter is null.
        /// </exception>
        public async Task RemoveAccountAsync(ModuleAccount moduleAccount)
        {
            moduleAccount.AssertNotNull(nameof(moduleAccount));

            if (moduleAccount.AccountType is not ModuleAccountType.User)
            {
                return;
            }

            IPublicClientApplication client = PublicClientApplicationBuilder.Create(moduleAccount.GetProperty(ExtendedPropertyType.ApplicationId)).Build();
            await RegisterCacheAsync(client.UserTokenCache).ConfigureAwait(false);

            IEnumerable<IAccount> accounts = await client.GetAccountsAsync().ConfigureAwait(false);

            foreach (IAccount account in accounts.Where(a => IsAccountMatch(a, moduleAccount)))
            {
                await client.RemoveAsync(account).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Registers a token cache to synchronize with the approriate storage.
        /// </summary>
        /// <param name="tokenCache">The token cache to be registered.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        public abstract Task RegisterCacheAsync(ITokenCache tokenCache);

        /// <summary>
        /// Verifies the underlying persistence mechanism for the token cache.
        /// </summary>
        /// <returns><c>true</c> if the underlying persistence mechanism was succesfully verified; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> VerifyPersistenceAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Determines if the identity and module accounts are a match.
        /// </summary>
        /// <param name="identityAccount">The identity account to be compared.</param>
        /// <param name="moduleAccount">The module account to be compared.</param>
        /// <returns><c>true</c> if the identity and module accounts are a match; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The identityAccount parameter is null.
        /// or
        /// The moduleAccount parameter is null.
        /// </exception>
        private static bool IsAccountMatch(IAccount identityAccount, ModuleAccount moduleAccount)
        {
            identityAccount.AssertNotNull(nameof(identityAccount));
            moduleAccount.AssertNotNull(nameof(moduleAccount));

            return string.Equals(identityAccount.Username, moduleAccount.Username, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    identityAccount.HomeAccountId?.Identifier,
                    moduleAccount.ExtendedProperties.GetProperty(ExtendedPropertyType.HomeAccountId),
                    StringComparison.OrdinalIgnoreCase);
        }
    }
}