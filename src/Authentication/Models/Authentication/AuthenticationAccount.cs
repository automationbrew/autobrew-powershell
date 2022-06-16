namespace AutoBrew.PowerShell.Models.Authentication
{
    using Azure.Identity;
    using Microsoft.Identity.Client;

    /// <summary>
    /// Represents an account used for authentication.
    /// </summary>
    internal sealed class AuthenticationAccount : IAccount
    {
        /// <summary>
        /// The authentication used to populate account properties.
        /// </summary>
        private readonly AuthenticationRecord record;

        /// <summary>
        /// Gets the value that uniquely identifies the across Azure Active Directory tenants.
        /// </summary>
        public AccountId HomeAccountId
        {
            get
            {
                AccountId accountId; ;

                string[] homeAccountSegments = record.HomeAccountId.Split('.');

                accountId = homeAccountSegments.Length == 2
                    ? new AccountId(record.HomeAccountId, homeAccountSegments[0], homeAccountSegments[1])
                    : new AccountId(record.HomeAccountId);

                return accountId;
            }
        }

        /// <summary>
        /// Gets a string containing the identity provider for this account.
        /// </summary>
        public string Environment => record.Authority;

        /// <summary>
        /// Gets a string containing the displayable value in user principal name (UPN) format.
        /// </summary>
        public string Username => record.Username;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationAccount" /> class.
        /// </summary>
        /// <param name="record">An instance of the <see cref="AuthenticationRecord" /> class used to populate account properties.</param>
        /// <exception cref="ArgumentNullException">
        /// The record argument is null.
        /// </exception>
        public AuthenticationAccount(AuthenticationRecord record)
        {
            record.AssertNotNull(nameof(record));
            this.record = record;
        }

        /// <summary>
        /// Converts an instance of the <see cref="AuthenticationRecord" /> class to an instance of the <see cref="AuthenticationAccount" /> class.
        /// </summary>
        /// <param name="record">An instance of the <see cref="AuthenticationRecord" /> class used to populate account properties.</param>
        public static explicit operator AuthenticationAccount(AuthenticationRecord record)
        {
            return new(record);
        }
    }
}