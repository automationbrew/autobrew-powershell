namespace AutoBrew.PowerShell.Models.Parameters
{
    using System.Security;
    using Authentication;

    /// <summary>
    /// Represents the parameters used when using the username and password combination for authentication.
    /// </summary>
    internal sealed class UsernamePasswordParameters : AuthenticationParameters
    {
        /// <summary>
        /// Gets the password to be used for the authentication request.
        /// </summary>
        public SecureString Password { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePasswordParameters" /> class.
        /// </summary>
        /// <param name="requestData">The request data to be used when requesting an access token.</param>
        public UsernamePasswordParameters(TokenRequestData requestData) : base(requestData)
        {
            Password = requestData.Password;
        }
    }
}