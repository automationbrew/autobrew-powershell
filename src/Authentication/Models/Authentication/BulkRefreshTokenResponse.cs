namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Text.Json.Serialization;
    using Converters;

    /// <summary>
    /// Represents the response from Azure Active Directory for a bulk refresh token.
    /// </summary>
    internal sealed class BulkRefreshTokenResponse
    {
        /// <summary>
        /// Gets or sets the flow token for the bulk refresh token response.
        /// </summary>
        [JsonPropertyName("flowToken")]
        public string FlowToken { get; set; }

        /// <summary>
        /// Gets or sets the result data for the bulk refresh token response.
        /// </summary>
        [JsonConverter(typeof(NestedStringJsonConverter<BulkRefreshTokenResultData>))]
        [JsonPropertyName("resultData")]
        public BulkRefreshTokenResultData ResultData { get; set; }

        /// <summary>
        /// Gets or sets the state for the bulk refresh token response.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}