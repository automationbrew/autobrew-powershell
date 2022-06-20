namespace AutoBrew.PowerShell.Models.Authentication
{
    internal static class ModuleEnvironmentConstants
    {
        public const string AzureActiveDirectoryAuthority = "https://login.microsoftonline.com/";

        public const string AzureCloud = "AzureCloud";

        public const string BulkRefreshTokenBeginEndpoint = "https://login.microsoftonline.com/webapp/bulkaadjtoken/begin";

        public const string BulkRefreshTokenPollEndpoint = "https://login.microsoftonline.com/webapp/bulkaadjtoken/poll";

        public const string MicrosoftGraphEndpoint = "https://graph.microsoft.com";
    }
}