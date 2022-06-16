namespace AutoBrew.PowerShell.Models.Parameters
{
    using Authentication;

    /// <summary>
    /// Represents the parameters used when using a silent approach for authentication.
    /// </summary>
    internal sealed class SilentParameters : AuthenticationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SilentParameters" /> class.
        /// </summary>
        /// <param name="requestData">The request data to be used when requesting an access token.</param>
        public SilentParameters(TokenRequestData requestData) : base(requestData)
        {
        }
    }
}