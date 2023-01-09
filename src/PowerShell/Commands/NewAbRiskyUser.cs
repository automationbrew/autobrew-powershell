namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Security;
    using System.Text.RegularExpressions;
    using Properties;
    using Factories;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Cmdlet that generates a risky user event in Azure Active Directory by performing an authentication request that is proxied using the Tor Project.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbRiskyUser", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class NewAbRiskyUser : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Name for the credential parameter set.
        /// </summary>
        private const string CredentialParameterSetName = "CredentialParameterSet";

        /// <summary>
        /// Name for the username and password parameter set.
        /// </summary>
        private const string UsernamePasswordParameterSetName = "UsernamePasswordParameterSet";

        /// <summary>
        /// Gets or sets the identifier of the application that will be used for authentication.
        /// </summary>
        [Alias("ClientId")]
        [Parameter(HelpMessage = "The identifier of the application that will be used for authentication.", Mandatory = true)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the credentials for the user where the risk event should be triggered.
        /// </summary>
        [Parameter(HelpMessage = "The credentials for the user where the risk event should be triggered.", Mandatory = true, ParameterSetName = CredentialParameterSetName)]
        [ValidateNotNull]
        public PSCredential Credential { get; set; }

        /// <summary>
        /// Gets or sets the password where the risk event should be triggered.
        /// </summary>
        [Parameter(HelpMessage = "The password for the user where the risk event should be triggered.", Mandatory = true, ParameterSetName = UsernamePasswordParameterSetName)]
        [ValidateNotNull]
        public SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the Azure Active Directory tenant identifier for the tenant.
        /// </summary>
        [Alias("Domain", "TenantId")]
        [Parameter(HelpMessage = "The Azure Active Directory tenant identifier for the tenant.", Mandatory = true)]
        [ValidateNotNull]
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the user principal name (UPN) for the user where the risk event should be triggered.
        /// </summary>
        [Alias("UPN")]
        [Parameter(HelpMessage = "The user principal name (UPN) for the user where the risk event should be triggered.", Mandatory = true, ParameterSetName = UsernamePasswordParameterSetName)]
        [ValidatePattern(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", Options = RegexOptions.IgnoreCase)]
        public string UserPrincipalName { get; set; }

        /// <inheritdoc/>
        public override bool ValidateConnection => true;

        /// <summary>
        /// Performs the operations associated with the cmdlet.
        /// </summary>
        protected override async Task PerformCmdletAsync()
        {
            string username = string.IsNullOrEmpty(UserPrincipalName) ? Credential.UserName : UserPrincipalName;

            await ConfirmActionAsync(Resources.NewRiskyUserAction, username, async () => 
            {
                ModuleAccount account = ModuleSession.Instance.Context.Account.Clone();
                account.Tenant = Tenant;

                GraphServiceClient client = ModuleSession.Instance.ClientFactory.CreateGraphServiceClient(account);

                IdentitySecurityDefaultsEnforcementPolicy securityDefaults = await client.Policies.IdentitySecurityDefaultsEnforcementPolicy.Request().GetAsync(CancellationToken).ConfigureAwait(false);

                // Temporarily disable security defaults if currently enabled. 

                if (securityDefaults.IsEnabled.HasValue && securityDefaults.IsEnabled.Value)
                {
                    await UpdateSecurityDefaultsAsync(client, false).ConfigureAwait(false);
                }

                // Generate a risky user event by performing an authentication request using a Tor proxy.

                IPublicClientApplication app = PublicClientApplicationBuilder.Create(ApplicationId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                    .WithHttpClientFactory(MsalHttpClientFactory.GetInstance(MsalHttpClientFactoryType.Proxy))
                    .Build();

                SecureString password = Password ?? Credential.Password;

                _ = await app.AcquireTokenByUsernamePassword(
                    new[] { $"{ModuleSession.Instance.Context.Environment.MicrosoftGraphEndpoint}/.default" },
                    username,
                    password.AsString()).ExecuteAsync(CancellationToken).ConfigureAwait(false);

                // Enable security defaults only in the event it was enabled before performing this operation.

                if (securityDefaults.IsEnabled.HasValue && securityDefaults.IsEnabled.Value)
                {
                    await UpdateSecurityDefaultsAsync(client, true).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Update the specified configuration for Identity Security Defaults.
        /// </summary>
        /// <param name="client">An instance of <see cref="GraphServiceClient" /> used to interact with Microsoft Graph.</param>
        /// <param name="isEnabled">A flag indicating whether Identity Security Default is enabled.</param>
        /// <returns>An instance of <see cref="Task" /> that represents the asynchronous operation.</returns>
        private async Task UpdateSecurityDefaultsAsync(GraphServiceClient client, bool isEnabled = true)
        {
            await client.Policies.IdentitySecurityDefaultsEnforcementPolicy.Request().UpdateAsync(new IdentitySecurityDefaultsEnforcementPolicy
            {
                IsEnabled = isEnabled
            }, CancellationToken);
        }
    }
}