namespace AutoBrew.PowerShell.Models.Parameters
{
    using Authentication;

    /// <summary>
    /// Represents the parameters used when using user interaction for authentication.
    /// </summary>
    internal sealed class InteractiveUserParameters : AuthenticationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveUserParameters" /> class.
        /// </summary>
        /// <param name="requestData">The request data to be used when requesting an access token.</param>
        public InteractiveUserParameters(TokenRequestData requestData) : base(requestData)
        {
        }
    }
}