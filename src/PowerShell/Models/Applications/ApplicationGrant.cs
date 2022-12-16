namespace AutoBrew.PowerShell.Models.Applications
{
    /// <summary>
    /// Represents an application grant used as part of the consent process for an Azure Active Directory application.
    /// </summary>
    public sealed class ApplicationGrant
    {
        /// <summary>
        /// Gets or sets the identifier of the enterprise application associated with the application grant.
        /// </summary>
        public string EnterpriseApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a comma delimited list of scopes that are associated with the application grant. 
        /// </summary>
        public string Scope { get; set; }
    }
}