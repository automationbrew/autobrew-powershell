namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents a bulk refresh token obtained from Azure Active Directory.
    /// </summary>
    public sealed class BulkRefreshToken
    {
        /// <summary>
        /// Gets the error that was encountered.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets the description for the error that was encountered.
        /// </summary>
        public string ErrorDescription { get; }

        /// <summary>
        /// Gets the refresh token, which can be used to obtain new access tokens.
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        /// Gets the point in time in which the the refresh token ceases to be valid.
        /// </summary>
        public DateTimeOffset RefreshTokenExpiresOn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkRefreshToken" /> class.
        /// </summary>
        /// <param name="response">The response from the request for a bulk refresh token.</param>
        /// <exception cref="ArgumentNullException">
        /// The response parameter is null.
        /// </exception>
        internal BulkRefreshToken(BulkRefreshTokenResponse response)
        {
            response.AssertNotNull(nameof(response));

            Error = response.ResultData.Error;
            ErrorDescription = response.ResultData.ErrorDescription;
            RefreshToken = response.ResultData.RefreshToken;
            RefreshTokenExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(response.ResultData.RefreshTokenExpiresIn);
        }
    }
}