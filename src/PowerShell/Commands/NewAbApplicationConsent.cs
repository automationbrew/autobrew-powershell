namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Models;
    using Models.Applications;
    using Models.Authentication;
    using Network;

    /// <summary>
    /// Cmdlet used to create a new application grant.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbApplicationConsent")]
    public class NewAbApplicationConsent : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the application grants that are associated with the application consent. 
        /// </summary>
        [Parameter(HelpMessage = "The application grants that are associated with the application consent.", Mandatory = true)]
        public ApplicationGrant[] ApplicationGrants { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Azure Active Directory application.
        /// </summary>
        [Parameter(HelpMessage = "The identifier of the Azure Active Directory application.", Mandatory = true)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the Azure Active Directory application.
        /// </summary>
        [Parameter(HelpMessage = "The display name of the Azure Active Directory application.", Mandatory = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the customer tenant.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the customer tenant.", Mandatory = true)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string TenantId { get; set; }

        /// <inheritdoc />
        public override bool ValidateConnection => true;

        /// <inheritdoc />
        protected override async Task PerformCmdletAsync()
        {
            ApplicationConsent consent = new()
            {
                ApplicationGrants = new List<ApplicationGrant>(ApplicationGrants),
                ApplicationId = ApplicationId
            };

            IRestServiceClient client = await ModuleSession.Instance.ClientFactory.CreateRestServiceClientAsync(
                new TokenRequestData(
                    ModuleSession.Instance.Context.Account,
                    ModuleSession.Instance.Context.Environment,
                    new[] { $"{ModuleSession.Instance.Context.Environment.MicrosoftPartnerCenterEndpoint}/user_impersonation" }),
                CancellationToken).ConfigureAwait(false);

            consent = await client.PostAsync<ApplicationConsent, ApplicationConsent>(
                new Uri(new Uri(ModuleSession.Instance.Context.Environment.MicrosoftPartnerCenterEndpoint), $"/v1/customers/{TenantId}/applicationconsents"),
                consent,
                CancellationToken).ConfigureAwait(false);

            WriteObject(consent);
        }
    }
}