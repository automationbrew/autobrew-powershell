namespace AutoBrew.PowerShell.Authenticators
{
    using Models.Authentication;
    using Models.Parameters;

    /// <summary>
    /// Represents the ability to perform authentication requests.
    /// </summary>
    internal interface IAuthenticator
    {
        /// <summary>
        /// Acquires an access token from the authority based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameter that will be used as part of the authentication request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="ModuleAuthenticationResult" /> class that represents the acquired access token.</returns>
        Task<ModuleAuthenticationResult> AuthenticateAsync(AuthenticationParameters parameters, CancellationToken cancellationToken = default);
    }
}