namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents the result data from Azure Active Directory for a bulk refresh token.
    /// </summary>
    internal sealed class BulkRefreshTokenResultData
    {
        /// <summary>
        /// Gets or sets the error that was encountered.
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the description for the error that was encountered.
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets the token issued that contains claims that carry information about the user.
        /// </summary>
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token, which can be used to obtain new access tokens.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration time of the refresh token in seconds since the response was generated.
        /// </summary>
        [JsonPropertyName("refresh_token_expires_in")]
        public long RefreshTokenExpiresIn { get; set; }
    }
}