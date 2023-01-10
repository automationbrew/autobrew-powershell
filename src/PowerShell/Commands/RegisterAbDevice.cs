namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Runtime.InteropServices;
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
        [Parameter(HelpMessage = "The address for the management service.", Mandatory = false)]
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
                MdmRegistration.ManagementServiceInfo? info = default;
                int hr = 0;

                if (string.IsNullOrEmpty(ManagementUri))
                {
                    hr = MdmRegistration.DiscoverManagementService(UserPrincipalName, out IntPtr pInfo);
                    info = (MdmRegistration.ManagementServiceInfo)Marshal.PtrToStructure(pInfo, typeof(MdmRegistration.ManagementServiceInfo));
                }

                if (hr != 0)
                {
                    throw new ModuleException($"{hr}", ModuleExceptionCategory.Interop);
                }

                hr = MdmRegistration.RegisterDeviceWithManagement(
                    UserPrincipalName, 
                    info == null ? ManagementUri : info.Value.mdmServiceUri, 
                    AccessToken);

                if (hr != 0)
                {
                    throw new ModuleException($"{hr}", ModuleExceptionCategory.Interop);
                }
            });
        }
    }
}