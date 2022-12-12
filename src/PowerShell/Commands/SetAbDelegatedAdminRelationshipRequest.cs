namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that updates the specified delegated admin relationship request.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AbDelegatedAdminRelationshipRequest")]
    [OutputType(typeof(DelegatedAdminRelationshipRequestObject))]
    public class SetAbDelegatedAdminRelationshipRequest : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the action for the delegated admin relationship request.
        /// </summary>
        [Parameter(HelpMessage = "The action for the delegated admin relationship request.", Mandatory = true)]
        [ValidateSet(nameof(DelegatedAdminRelationshipRequestAction.Approve), nameof(DelegatedAdminRelationshipRequestAction.LockForApproval), nameof(DelegatedAdminRelationshipRequestAction.Terminate))]
        public DelegatedAdminRelationshipRequestAction Action { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the delegated admin relationship.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the delegated admin relationship.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string RelationshipId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the delegated admin relationship request.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the delegated admin relationship request.")]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string RequestId { get; set; }

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
                .AddAsync(new DelegatedAdminRelationshipRequestObject() { Action = Action }, CancellationToken)
                .ConfigureAwait(false);

            WriteObject(request);
        }
    }
}