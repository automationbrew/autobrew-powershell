namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents commonly used commonly used constants for authentication purposes.
    /// </summary>
    internal static class AuthenticationConstants
    {
        /// <summary>
        /// The claim that should be included in access token requests when multi-factor authentication should be enforced.
        /// </summary>
        public const string MultiFactorAuthenticationClaim = @"{
                ""access_token"": {
                    ""acr"": { ""values"": [""urn:microsoft:policies:mfa""] }
                }
            }";
    }
}