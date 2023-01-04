namespace AutoBrew.PowerShell.Models.Applications
{
    /// <summary>
    /// Represents consent for an Azure Active Directory application.
    /// </summary>
    public sealed class ApplicationConsent
    {
        /// <summary>
        /// Gets or sets the application grants to be associated with the application consent.
        /// </summary>
        public List<ApplicationGrant> ApplicationGrants { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Azure Active Directory application.
        /// </summary>
        public string ApplicationId { get; set; }
    }
}