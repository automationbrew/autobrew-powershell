namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that creates a new delegated admin relationship request for approval by the customer.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbDelegatedAdminRelationshipRequest")]
    [OutputType(typeof(DelegatedAdminRelationshipRequestObject))]
    public class NewAbDelegatedAdminRelationshipRequest : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the delegated admin relationship.", Mandatory = true)]
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

            DelegatedAdminRelationshipRequestObject request = await client
                .TenantRelationships
                .DelegatedAdminRelationships[RelationshipId]
                .Requests
                .Request()
                .AddAsync(new DelegatedAdminRelationshipRequestObject()
                {
                    Action = DelegatedAdminRelationshipRequestAction.LockForApproval
                }, CancellationToken).ConfigureAwait(false);

            WriteObject(request);
        }
    }
}