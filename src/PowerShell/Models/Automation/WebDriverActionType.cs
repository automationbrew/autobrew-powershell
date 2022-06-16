namespace AutoBrew.PowerShell.Models.Automation
{
    /// <summary>
    /// Represents the type for an instance of the <see cref="WebDriverAction" /> class.
    /// </summary>
    [Flags]
    public enum WebDriverActionType
    {
        /// <summary>
        /// Represents the intention for the web driver action is to perform operations associated with authentication.
        /// </summary>
        Authenticate = 0,

        /// <summary>
        /// Represents the intention for the web driver action is to perform authenticate using the password only.
        /// </summary>
        AuthenticatePasswordOnly = 1,

        /// <summary>
        /// Represents the intention for the web driver action is to click on a web element.
        /// </summary>
        Click = 2,

        /// <summary>
        /// Represents the intention for the web driver action is to find a web element.
        /// </summary>
        Find = 4,

        /// <summary>
        /// Represents the intention for the web driver action is to send key strokes to a web element.
        /// </summary>
        SendKeys = 8,

        /// <summary>
        /// Represents the intention for the web driver action is to switch to a different window.
        /// </summary>
        SwitchWindow = 16,

        /// <summary>
        /// Represents the intention is to create a task that will complete after a time delay.
        /// </summary>
        TimeDelay = 32,

        /// <summary>
        /// Represent the intention for the web driver action is a future value or unknown.
        /// </summary>
        UnknownFutureValue = 128
    }
}