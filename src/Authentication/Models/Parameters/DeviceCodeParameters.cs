namespace AutoBrew.PowerShell.Models.Parameters
{
    using Authentication;

    /// <summary>
    /// Represents the parameters used when using a device code for authentication.
    /// </summary>
    internal sealed class DeviceCodeParameters : AuthenticationParameters
    {
        /// <summary>
        /// Gets the action that handles the output for the device code callback.
        /// </summary>
        public Action<string> DeviceCodeCallbackOutput { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCodeParameters" /> class.
        /// </summary>
        /// <param name="requestData">The request data to be used when requesting an access token.</param>
        public DeviceCodeParameters(TokenRequestData requestData, Action<string> outputAction) : base(requestData)
        {
            DeviceCodeCallbackOutput = outputAction;
        }
    }
}