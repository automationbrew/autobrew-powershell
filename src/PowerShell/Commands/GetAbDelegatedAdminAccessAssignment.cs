namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that provides the ability to get access assignments for a given delegated admin relationship.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AbDelegatedAdminAccessAssignment")]
    [OutputType(typeof(DelegatedAdminAccessAssignment))]
    public class GetAbDelegatedAdminAccessAssignment : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier for the access assignment.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the access assignment.", Mandatory = false)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string AccessAssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the delegated admin relationship.", Mandatory = true)]
        public string RelationshipId { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            GraphServiceClient client = ModuleSession.Instance.ClientFactory.CreateGraphServiceClient(ModuleSession.Instance.Context.Account);

            if (string.IsNullOrEmpty(AccessAssignmentId))
            {
                IDelegatedAdminRelationshipAccessAssignmentsCollectionPage data = await client
                    .TenantRelationships
                    .DelegatedAdminRelationships[RelationshipId]
                    .AccessAssignments
                    .Request()
                    .GetAsync(CancellationToken)
                    .ConfigureAwait(false);

                List<DelegatedAdminAccessAssignment> accessAssignments = new(data.CurrentPage);

                while (data.NextPageRequest != null)
                {
                    data = await data.NextPageRequest.GetAsync(CancellationToken).ConfigureAwait(false);
                    accessAssignments.AddRange(data.CurrentPage);
                }

                WriteObject(accessAssignments, true);
            }
            else
            {
                DelegatedAdminAccessAssignment accessAssignment = await client
                    .TenantRelationships
                    .DelegatedAdminRelationships[RelationshipId]
                    .AccessAssignments[AccessAssignmentId]
                    .Request()
                    .GetAsync(CancellationToken).ConfigureAwait(false);

                WriteObject(accessAssignment);
            }
        }
    }
}