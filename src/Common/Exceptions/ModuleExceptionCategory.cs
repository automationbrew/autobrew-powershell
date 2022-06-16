namespace AutoBrew.PowerShell
{
    /// <summary>
    /// Represents the categories for an exception thrown by the module.
    /// </summary>
    public enum ModuleExceptionCategory
    {
        /// <summary>
        /// Represents the exception was caused by an authentication related issue.
        /// </summary>
        Authentication,

        /// <summary>
        /// Represents the exception was caused by an action related to certificates.
        /// </summary>
        Certificate,

        /// <summary>
        /// Represents the exception was cause by a configuration related issue.
        /// </summary>
        Configuration,

        /// <summary>
        /// Represents the exception was caused by a network related issue.
        /// </summary>
        Network,

        /// <summary>
        /// Represents the exception category was not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// Represents the exception was caused by a token being expired.
        /// </summary>
        TokenExpired,

        /// <summary>
        /// Represents the exception was caused by interaction with a web driver.
        /// </summary>
        WebDriver,

        /// <summary>
        /// Represents the exception was caused by an unknown or future value.
        /// </summary>
        UnknownFutureValue
    }
}