namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents the type of environments supported by the module.
    /// </summary>
    public enum ModuleEnvironmentType
    {
        /// <summary>
        /// Represents the type of environment is one that is built into the module.
        /// </summary>
        BuiltIn,

        /// <summary>
        /// Represents the type of environment has been defined by the user.
        /// </summary>
        UserDefined
    }
}