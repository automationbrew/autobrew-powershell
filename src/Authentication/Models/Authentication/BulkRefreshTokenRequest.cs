namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents the request for a bulk refresh token.
    /// </summary>
    internal sealed class BulkRefreshTokenRequest
    {
        /// <summary>
        /// Gets or sets the display name for the provisioning package.
        /// </summary>
        [JsonPropertyName("name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the expiration for the bulk refresh token.
        /// </summary>
        [JsonPropertyName("exp")]
        public string Expiration { get; set; }

        /// <summary>
        /// Gets or sets the unique package identifier.
        /// </summary>
        [JsonPropertyName("pid")]
        public string PackageId { get; set; }
    }
}