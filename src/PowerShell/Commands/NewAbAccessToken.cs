namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Security;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Cmdlet that requests a new access token from Azure Active Directory.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbAccessToken", DefaultParameterSetName = DefaultParameterSetName)]
    [OutputType(typeof(ModuleAuthenticationResult))]
    public class NewAbAccessToken : ModuleAsyncCmdlet
    {
        /// <summary>
        /// The name for the authorization code parameter set.
        /// </summary>
        private const string AuthorizationCodeParameterSetName = "AuthorizationCodeParameterSet";

        /// <summary>
        /// The value for the common tenant.
        /// </summary>
        private const string CommonTenant = "organizations";

        /// <summary>
        /// The name for the default parameter set.
        /// </summary>
        private const string DefaultParameterSetName = "DefaultParameterSet";

        /// <summary>
        /// The name for the device code parameter set.
        /// </summary>
        private const string DeviceCodeParameterSetName = "DeviceCodeParameterSet";

        /// <summary>
        /// The name for the refresh token parameter set.
        /// </summary>
        private const string RefreshTokenParameterSetName = "RefreshTokenParameterSet";

        /// <summary>
        /// Gets or sets the identifier for the application to be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the application to be used for authentication.", Mandatory = false)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the environment to be used for authentication.
        /// </summary>
        [EnvironmentCompleter]
        [Parameter(HelpMessage = "The name of the environment to be used for authentication.", Mandatory = false)]
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the refresh token to be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The refresh token to be used for authentication.", Mandatory = true, ParameterSetName = RefreshTokenParameterSetName)]
        [ValidateNotNull]
        public SecureString RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the scopes to be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The scopes to be used for authentication.", Mandatory = true)]
        [ValidateNotNull]
        public string[] Scopes { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the tenant to be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the tenant to be used for authentication.", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the flag that indicates the authorization code flow should be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The flag that indicates the authorization code flow should be used for authentication.", Mandatory = true, ParameterSetName = AuthorizationCodeParameterSetName)]
        public SwitchParameter UseAuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets the flag that indicates the device code flow should be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The flag that indicates the device code flow should be used for authentication.", Mandatory = true, ParameterSetName = DeviceCodeParameterSetName)]
        public SwitchParameter UseDeviceAuthentication { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            ModuleAccount account = new()
            {
                AccountType = ModuleAccountType.User,
                Tenant = string.IsNullOrEmpty(Tenant) ? CommonTenant : Tenant
            };

            ModuleSession.Instance.TryGetEnvironment(Environment, out ModuleEnvironment environment);

            if (string.IsNullOrEmpty(ApplicationId) == false)
            {
                account.SetProperty(ExtendedPropertyType.ApplicationId, ApplicationId);
            }

            if (ParameterSetName.Equals(DefaultParameterSetName))
            {
                account.SetProperty(ExtendedPropertyType.UseDeviceCode, true.ToString());
            }
            if (UseAuthorizationCode.IsPresent)
            {
                account.SetProperty(ExtendedPropertyType.UseAuthorizationCode, true.ToString());
            }
            else if (UseDeviceAuthentication.IsPresent)
            {
                account.SetProperty(ExtendedPropertyType.UseDeviceCode, true.ToString());
            }

            WriteObject(await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                new TokenRequestData(account, environment, Scopes)
                {
                    IncludeRefreshToken = true,
                    RefreshToken = RefreshToken
                },
                (string value) => WriteWarning(value),
                CancellationToken).ConfigureAwait(false));
        }
    }
}