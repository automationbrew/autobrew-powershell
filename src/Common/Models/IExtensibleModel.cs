namespace AutoBrew.PowerShell.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a model with extensible properties, to allow backward compatible updates to the model.
    /// </summary>
    public interface IExtensibleModel
    {
        /// <summary>
        /// Gets the extended properties.
        /// </summary>
        IDictionary<ExtendedPropertyType, string> ExtendedProperties { get; }
    }
}