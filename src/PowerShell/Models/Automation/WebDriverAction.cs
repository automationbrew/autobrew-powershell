namespace AutoBrew.PowerShell.Models.Automation
{
    /// <summary>
    /// Represents an action that should be performed by a Selenium web driver.
    /// </summary>
    public sealed class WebDriverAction
    {
        /// <summary>
        /// Gets or sets the type of action to be performed.
        /// </summary>
        public WebDriverActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets an integer that represent the sequence order for the web driver action.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets the wait threshold for a web element to be visible represented in seconds.
        /// </summary>
        public int WaitThresholdInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the value used to locate the web element.
        /// </summary>
        public string WebElementLocator { get; set; }

        /// <summary>
        /// Gets or sets the type of locator used to locate the web element.
        /// </summary>
        public WebElementLocatorType WebElementLocatorType { get; set; }

        /// <summary>
        /// Gets or sets the value(s) that should be sent to a web element when using the send keys method.
        /// </summary>
        /// <remarks>
        /// This value is only used when the <see cref="WebDriverActionType" /> is send keys.
        /// </remarks>
        public List<WebElementValue> WebElementValues { get; set; }
    }
}