namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Models.Applications;

    /// <summary>
    /// Cmdlet that generates a new in-memory application grant.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbApplicationGrant")]
    [OutputType(typeof(ApplicationGrant))]
    public class NewAbApplicationGrant : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier of the enterprise application associated with the application grant.
        /// </summary>
        [Parameter(HelpMessage = "The identifier of the enterprise application associated with the application grant.", Mandatory = true)]
        public string EnterpriseApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of scopes that are associated with the application grant. 
        /// </summary>
        [Parameter(HelpMessage = "A comma delimited list of scopes that are associated with the application grant.", Mandatory = true)]
        public string Scope { get; set; }

        /// <inheritdoc />
        protected override void PerformCmdlet()
        {
            WriteObject(new ApplicationGrant { EnterpriseApplicationId = EnterpriseApplicationId, Scope = Scope });
        }
    }
}