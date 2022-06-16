namespace AutoBrew.PowerShell.Cache
{
    using System.Security;
    using System.Text.Json.Nodes;
    using Azure.Identity;
    using Microsoft.Identity.Client;

    /// <summary>
    /// Provides the ability to persist tokens.
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
            JsonNode node = JsonNode.Parse(stream);

            return node["RefreshToken"]?[$"{homeAccountId}-login.windows.net-refreshtoken-{clientId}--"]?["secret"]?.ToString().AsSecureString();
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
    }
}