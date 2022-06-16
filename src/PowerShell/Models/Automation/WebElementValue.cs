namespace AutoBrew.PowerShell.Models.Automation
{
    /// <summary>
    /// Represents the value that should be sent to a web element when using the send keys method.
    /// </summary>
    public sealed class WebElementValue
    {
        /// <summary>
        /// Gets or sets value for a key on the keyboard to be sent to the web element.
        /// </summary>
        public int? KeyValue { get; set; }

        /// <summary>
        /// Gets or sets the text value to be sent to the web element.
        /// </summary>
        public string Text { get; set; }
    }
}