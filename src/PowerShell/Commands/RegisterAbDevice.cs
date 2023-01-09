namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using Properties;
    using Interop;

    /// <summary>
    /// Cmdlet that registers a device with a MDM service, using the Mobile Device Enrollment Protocol.
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
                int error;

                if (MdmRegistration.DiscoverManagementService(UserPrincipalName, out IntPtr pInfo) == 0)
                {
                    MdmRegistration.ManagementServiceInfo info = (MdmRegistration.ManagementServiceInfo)Marshal.PtrToStructure(pInfo, typeof(MdmRegistration.ManagementServiceInfo));

                    error = MdmRegistration.RegisterDeviceWithManagement(UserPrincipalName, info.mdmServiceUri, AccessToken);
                }
                else
                {
                    throw new ModuleException($"Failed to discover the management service for {UserPrincipalName}");
                }

                if (error != 0)
                {
                    throw new ModuleException($"Device registration with the management service failed with error {error}");
                }
            });
        }
    }
}