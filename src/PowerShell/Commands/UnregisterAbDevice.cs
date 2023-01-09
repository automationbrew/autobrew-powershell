namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Properties;
    using Interop;

    /// <summary>
    /// Cmdlet that provides the ability to unregister the device with a management service.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Unregister, "AbDevice", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class UnregisterAbDevice : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier of the enrollment that should be unregistered.
        /// </summary>
        [Parameter(HelpMessage = "The identifier of the enrollment that should be unregistered.", Mandatory = true)]
        public string EnrollmentId { get; set; }

        /// <inheritdoc />
        protected override void PerformCmdlet()
        {
            ConfirmAction(Resources.UnregisterDeviceAction, Environment.MachineName, () =>
            {
                int hr = MdmRegistration.UnregisterDeviceWithManagement(EnrollmentId);

                if (hr != 0)
                {
                    throw new ModuleException($"Unregistering the device from the management service failed with error {hr}");
                }
            });
        }
    }
}