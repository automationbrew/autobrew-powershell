namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Interop;
    using Properties;

    /// <summary>
    /// Cmdlet that registers a device with a management service.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "AbDevice", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterAbDevice : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the access token for the user registering the device.
        /// </summary>
        [Parameter(HelpMessage = "The access token for the user registering the device.", Mandatory = true)]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the address for the management service.
        /// </summary>
        [Parameter(HelpMessage = "The address for the management service.", Mandatory = true)]
        public string ManagementUri { get; set; }

        /// <summary>
        /// Gets or sets the user principal name (UPN) for the user registering the device.
        /// </summary>
        [Alias("UPN")]
        [Parameter(HelpMessage = "The user principal name (UPN) for the user registering the device.", Mandatory = true)]
        [ValidatePattern(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", Options = RegexOptions.IgnoreCase)]
        public string UserPrincipalName { get; set; }

        /// <inheritdoc />
        protected override void PerformCmdlet()
        {
            ConfirmAction(Resources.RegisterDeviceAction, Environment.MachineName, () =>
            {
                int hr = MdmRegistration.RegisterDeviceWithManagement(UserPrincipalName, ManagementUri, AccessToken);

                if (hr != 0)
                {
                    throw new ModuleException(string.Format("0x{0:X}", hr), ModuleExceptionCategory.Interop);
                }
            });
        }
    }
}