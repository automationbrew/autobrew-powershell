namespace AutoBrew.PowerShell.Factories
{
    using Models.Authentication;

    /// <summary>
    /// Represents a factory used to perform authentication requests.
    /// </summary>
    public interface IAuthenticationFactory
    {
        /// <summary>
        /// Acquires a bulk refresh token from the authority.
        /// </summary>
        /// <param name="environment">The enivornment that will provide metadata used to acquire the bulk refresh token.</param>
        /// <param name="outputAction">The action that encapsulates the method used to write output.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="BulkRefreshToken" /> class that represents the acquired bulk refresh.</returns>
        Task<BulkRefreshToken> AcquireBulkRefreshTokenAsync(ModuleEnvironment environment, Action<string> outputAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Acquires an access token from the authority based on the specified request data.
        /// </summary>
        /// <param name="requestData">The data that will be used as part of the authentication request.</param>
        /// <param name="outputAction">The action that encapsulates the method used to write output.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="ModuleAuthenticationResult" /> class that represents the acquired access token.</returns>
        Task<ModuleAuthenticationResult> AcquireTokenAsync(TokenRequestData requestData, Action<string> outputAction = null, CancellationToken cancellationToken = default);
    }
}