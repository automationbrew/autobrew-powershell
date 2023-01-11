namespace AutoBrew.PowerShell.Commands
{
    using System;
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Interop;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to register a deivce for management.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "AbDevice", DefaultParameterSetName = AccessTokenParameterSetName, SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterAbDevice : ModuleAsyncCmdlet
    {
        /// <summary>
        /// The name for the access token parameter set.
        /// </summary>
        private const string AccessTokenParameterSetName = "AccessTokenParameterSet";

        /// <summary>
        /// The name for the credentials parameter set.
        /// </summary>
        private const string CredentialsParameterSetName = "CredentialsParameterSetName";

        /// <summary>
        /// The identifier for the Device Management Client application.
        /// </summary>
        private const string DeviceManagementApplicationId = "de50c81f-5f80-4771-b66b-cebd28ccdfc1";

        /// <summary>
        /// Gets or sets the access token to be used by the management service to validate the user.
        /// </summary>
        [Parameter(HelpMessage = "The access token to be used by the management service to validate the user.", Mandatory = true, ParameterSetName = AccessTokenParameterSetName)]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the credentials to be used by the management service to validate the user.
        /// </summary>
        [Parameter(HelpMessage = "The credentials to be used by the management service to validate the user.", Mandatory = true, ParameterSetName = CredentialsParameterSetName)]
        public PSCredential Credentials { get; set; }

        /// <summary>
        /// Gets or sets the name of the environment to be used for authentication.
        /// </summary>
        [EnvironmentCompleter]
        [Parameter(HelpMessage = "The name of the environment to be used for authentication.", Mandatory = true, ParameterSetName = CredentialsParameterSetName)]
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the address for the management service.
        /// </summary>
        [Parameter(HelpMessage = "The address for the management service.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ManagementUri { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the tenant to be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the tenant to be used for authentication.", Mandatory = true, ParameterSetName = CredentialsParameterSetName)]
        [ValidateNotNullOrEmpty]
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the user principal name of the user requesting the registration.
        /// </summary>
        [Parameter(HelpMessage = "The user principal name of the user requesting the registration.", Mandatory = true)]

        [ValidatePattern(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", Options = RegexOptions.IgnoreCase)]
        public string UserPrincipalName { get; set; }

        /// <inheritdoc />
        protected override async Task PerformCmdletAsync()
        {
            string accessToken;
            int hr;

            await ConfirmActionAsync(Resources.RegisterDeviceAction, System.Environment.MachineName, async () =>
            {
                accessToken = ParameterSetName.Equals(CredentialsParameterSetName, StringComparison.InvariantCultureIgnoreCase) ?
                    await GetAccessTokenAsync().ConfigureAwait(false) : AccessToken;

                hr = MdmRegistration.RegisterDeviceWithManagement(UserPrincipalName, ManagementUri, accessToken);

                if (hr != 0)
                {
                    throw new ModuleException(string.Format("0x{0:X}", hr), ModuleExceptionCategory.Interop);
                }
            }).ConfigureAwait(false);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            ModuleAccount account = new()
            {
                AccountType = ModuleAccountType.User,
                Tenant = Tenant,
                Username = Credentials.UserName
            };

            ModuleSession.Instance.TryGetEnvironment(Environment, out ModuleEnvironment environment);

            account.SetProperty(KnownExtendedPropertyKeys.ApplicationId, DeviceManagementApplicationId);

            ModuleAuthenticationResult authResult = await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                new TokenRequestData(account, environment, new[] { $"https://enrollment.{ManagementUri}//.default" })
                {
                    IncludeRefreshToken = false,
                    Password = Credentials.Password
                },
                (string value) => WriteWarning(value),
                CancellationToken).ConfigureAwait(false);

            return authResult.AccessToken;
        }
    }
}