namespace AutoBrew.PowerShell.Models
{
    using System.Collections.Concurrent;
    using Models.Authentication;

    /// <summary>
    /// Represents the context used to perform various tasks.
    /// </summary>
    public sealed class ModuleContext : IExtensibleModel
    {
        /// <summary>
        /// Gets or sets the account details for the context.
        /// </summary>
        public ModuleAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the environment for the context.
        /// </summary>
        public ModuleEnvironment Environment { get; set; }

        /// <summary>
        /// Gets the extended properties for the context.
        /// </summary>
        public IDictionary<string, string> ExtendedProperties { get; } = new ConcurrentDictionary<string, string>();
    }
}