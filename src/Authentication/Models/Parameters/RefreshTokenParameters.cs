namespace AutoBrew.PowerShell.Models.Parameters
{
    using System.Security;
    using Authentication;

    /// <summary>
    /// Represents the parameters used when using a refresh token for authentication.
    /// </summary>
    internal sealed class RefreshTokenParameters : AuthenticationParameters
    {
        /// <summary>
        /// Gets the refresh token to be used for the authentication request.
        /// </summary>
        public SecureString RefreshToken { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenParameters" /> class.
        /// </summary>
        /// <param name="requestData">The request data to be used when requesting an access token.</param>
        public RefreshTokenParameters(TokenRequestData requestData) : base(requestData)
        {
            RefreshToken = requestData.RefreshToken;
        }
    }
}