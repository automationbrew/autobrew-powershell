namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that provides the ability to get delegated admin relationships.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AbDelegatedAdminRelationship")]
    [OutputType(typeof(DelegatedAdminRelationship))]
    public class GetAbDelegatedAdminRelationship : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the delegated admin relationship.", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string RelationshipId { get; set; }

        /// <inheritdoc/>
        public override bool ValidateConnection => true;

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            GraphServiceClient client = ModuleSession.Instance.ClientFactory.CreateGraphServiceClient(ModuleSession.Instance.Context.Account);

            if (string.IsNullOrEmpty(RelationshipId))
            {
                ITenantRelationshipDelegatedAdminRelationshipsCollectionPage data = await client
                    .TenantRelationships
                    .DelegatedAdminRelationships
                    .Request()
                    .GetAsync(CancellationToken)
                    .ConfigureAwait(false);

                List<DelegatedAdminRelationship> relationships = new(data.CurrentPage);

                while (data.NextPageRequest != null)
                {
                    data = await data.NextPageRequest.GetAsync(CancellationToken).ConfigureAwait(false);
                    relationships.AddRange(data.CurrentPage);
                }

                WriteObject(relationships, true);
            }
            else
            {
                DelegatedAdminRelationship relationship = await client
                    .TenantRelationships
                    .DelegatedAdminRelationships[RelationshipId]
                    .Request()
                    .GetAsync(CancellationToken)
                    .ConfigureAwait(false);

                WriteObject(relationship);
            }
        }
    }
}