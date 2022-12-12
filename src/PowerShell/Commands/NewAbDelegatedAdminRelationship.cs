namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that creates delegated admin relationships based on the specified input.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbDelegatedAdminRelationship")]
    [OutputType(typeof(DelegatedAdminRelationship))]
    public class NewAbDelegatedAdminRelationship : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the display name for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The display name for the delegated admin relationship.", Mandatory = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the duration for the delegated admin relationship in the ISO8601 format.
        /// </summary>
        [Parameter(HelpMessage = "The duration for the delegated admin relationship in the ISO8601 format.", Mandatory = true)]
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the Azure Active Directory tenant identifier for the tenant.
        /// </summary>
        [Alias("TenantId")]
        [Parameter(HelpMessage = "The Azure Active Directory tenant identifier for the tenant.", Mandatory = false)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the unified roles for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The unified roles for the delegated admin relationship.", Mandatory = true)]
        public string[] UnifiedRoles { get; set; }

        /// <inheritdoc/>
        public override bool ValidateConnection => true;

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            GraphServiceClient client = ModuleSession.Instance.ClientFactory.CreateGraphServiceClient(ModuleSession.Instance.Context.Account);

            DelegatedAdminRelationship relationship = await client.TenantRelationships.DelegatedAdminRelationships.Request().AddAsync(new DelegatedAdminRelationship
            {
                AccessDetails = new DelegatedAdminAccessDetails
                {
                    UnifiedRoles = UnifiedRoles.Select(r => new UnifiedRole { RoleDefinitionId = r }).ToList()
                },
                Customer = new DelegatedAdminRelationshipCustomerParticipant
                {
                    TenantId = Tenant
                },
                DisplayName = DisplayName,
                Duration = new Duration(Duration)
            }, CancellationToken).ConfigureAwait(false);

            WriteObject(relationship);
        }
    }
}
