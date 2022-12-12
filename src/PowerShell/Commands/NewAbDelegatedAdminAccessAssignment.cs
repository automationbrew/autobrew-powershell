namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that creates a delegated admin relationship acccess assignment using the specified information.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbDelegatedAdminAccessAssignment")]
    [OutputType(typeof(DelegatedAdminAccessAssignment))]
    public class NewAbDelegatedAdminAccessAssignment : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier for the access container.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the access container.", Mandatory = true)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string AccessContainerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the delegated admin relationship.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string RelationshipId { get; set; }

        /// <summary>
        /// Gets or sets the unified roles to be included in the access assignment.
        /// </summary>
        [Parameter(HelpMessage = "The unified roles to be included in the access assignment.", Mandatory = true)]
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

            DelegatedAdminAccessAssignment accessAssignment = new()
            {
                AccessContainer = new DelegatedAdminAccessContainer
                {
                    AccessContainerId = AccessContainerId,
                    AccessContainerType = DelegatedAdminAccessContainerType.SecurityGroup
                },
                AccessDetails = new DelegatedAdminAccessDetails
                {
                    UnifiedRoles = UnifiedRoles.Select(r => new UnifiedRole { RoleDefinitionId = r })
                }
            };

            accessAssignment = await client
                .TenantRelationships
                .DelegatedAdminRelationships[RelationshipId]
                .AccessAssignments
                .Request()
                .AddAsync(accessAssignment, CancellationToken).ConfigureAwait(false);

            WriteObject(accessAssignment);
        }
    }
}