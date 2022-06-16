namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents the type of HTTP client factory used by the Microsoft Authentication Library (MSAL).
    /// </summary>
    public enum MsalHttpClientFactoryType
    {
        /// <summary>
        /// Represents the HTTP client factory is intended to proxy requests.
        /// </summary>
        Proxy,

        /// <summary>
        /// Represents the HTTP client factory is intended to request an access token using a refresh token.
        /// </summary>
        RefreshToken
    }
}