namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Represents the details for an account used for authentication.
    /// </summary>
    public sealed class ModuleAccount : IExtensibleModel
    {
        /// <summary>
        /// Gets or sets the type of account.
        /// </summary>
        public ModuleAccountType AccountType { get; set; }

        /// <summary>
        /// Gets the extended properties for the account.
        /// </summary>
        public IDictionary<ExtendedPropertyType, string> ExtendedProperties { get; } = new ConcurrentDictionary<ExtendedPropertyType, string>();

        /// <summary>
        /// Gets or sets the Azure Active Directory tenant identifier for the tenant.
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the username for the account.
        /// </summary>
        public string Username { get; set; }
    }
}