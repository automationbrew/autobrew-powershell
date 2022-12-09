﻿namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Cmdlet that provides the ability to get an access token.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AbAccessToken")]
    [OutputType(typeof(ModuleAuthenticationResult))]
    public class GetAbAccessToken : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Get or sets the scopes to be used for authentication.
        /// </summary>
        [Parameter(HelpMessage = "The scopes to be used for authentication.", Mandatory = true)]
        public string[] Scopes { get; set; }

        /// <summary>
        /// Gets a flag that indicates whether the connection should be validated before processing the command.
        /// </summary>
        public override bool ValidateConnection => true;

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            WriteObject(await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                new TokenRequestData(ModuleSession.Instance.Context.Account, ModuleSession.Instance.Context.Environment, Scopes)
                {
                    IncludeRefreshToken = true
                },
                (string value) => WriteWarning(value),
                CancellationToken).ConfigureAwait(false));
        }
    }
}