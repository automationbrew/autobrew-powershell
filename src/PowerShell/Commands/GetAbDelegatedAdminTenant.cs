namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Microsoft.Graph;
    using Models;

    /// <summary>
    /// Cmdlet that gets the available delegated admin tenants.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AbDelegatedAdminTenant")]
    [OutputType(typeof(DelegatedAdminCustomer))]
    public class GetAbDelegatedAdminTenant : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the Azure Active Directory tenant identifier for the tenant.
        /// </summary>
        [Parameter(HelpMessage = "The Azure Active Directory tenant identifier for the tenant.", Mandatory = false)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string Tenant { get; set; }

        /// <inheritdoc/>
        public override bool ValidateConnection => true;

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            GraphServiceClient client = ModuleSession.Instance.ClientFactory.CreateGraphServiceClient(ModuleSession.Instance.Context.Account);

            if (string.IsNullOrEmpty(Tenant))
            {
                ITenantRelationshipDelegatedAdminCustomersCollectionPage data = await client
                    .TenantRelationships
                    .DelegatedAdminCustomers
                    .Request()
                    .GetAsync(CancellationToken)
                    .ConfigureAwait(false);

                List<DelegatedAdminCustomer> tenants = new(data.CurrentPage);

                while (data.NextPageRequest != null)
                {
                    data = await data.NextPageRequest.GetAsync(CancellationToken).ConfigureAwait(false);
                    tenants.AddRange(data.CurrentPage);
                }

                WriteObject(tenants, true);
            }
            else
            {
                DelegatedAdminCustomer tenant = await client
                    .TenantRelationships
                    .DelegatedAdminCustomers[Tenant]
                    .Request()
                    .GetAsync(CancellationToken)
                    .ConfigureAwait(false);

                WriteObject(tenant);
            }
        }
    }
}