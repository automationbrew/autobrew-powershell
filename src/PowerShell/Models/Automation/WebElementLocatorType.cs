namespace AutoBrew.PowerShell.Models.Automation
{
    /// <summary>
    /// Represents the locator strategy to utilize when getting a reference to a web element.
    /// </summary>
    public enum WebElementLocatorType
    {
        /// <summary>
        /// Represents the web element should be located using the identifier.
        /// </summary>
        Id,

        /// <summary>
        /// Represents the web element should be located using the name.
        /// </summary>
        Name,

        /// <summary>
        /// Represent the web element should be located using the style sheet.
        /// </summary>
        StyleSheet,

        /// <summary>
        /// Represents the web element should be located using the XPath.
        /// </summary>
        XPath,

        /// <summary>
        /// Represent the web element locator type is a future value or unknown.
        /// </summary>
        UnknownFutureValue
    }
}