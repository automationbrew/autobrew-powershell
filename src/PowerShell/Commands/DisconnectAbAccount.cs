namespace AutoBrew.PowerShell.Commands
{
    using System.Globalization;
    using System.Management.Automation;
    using Cache;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to disconnect an account and removes all associated credentials.
    /// </summary>
    [Cmdlet(VerbsCommunications.Disconnect, "AbAccount", SupportsShouldProcess = true)]
    [OutputType(typeof(ModuleAccount))]
    public class DisconnectAbAccount : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            if (ModuleSession.Instance.TryGetComponent(ComponentType.TokenCache, out TokenCacheProvider tokenCache) == false)
            {
                throw new ModuleException("Reload the module becuase there was an issue with the token cache.", ModuleExceptionCategory.Authentication);
            }

            ModuleAccount account = ModuleSession.Instance?.Context?.Account;

            if (account == null)
            {
                ModuleSession.Instance.Context = null;
                return;
            }

            await ConfirmActionAsync(
                string.Format(CultureInfo.InvariantCulture, Resources.LogoutTarget, account.Username),
                Resources.DisconnectAccountTarget,
                async () =>
                {
                    await tokenCache.RemoveAccountAsync(ModuleSession.Instance.Context.Account);
                    ModuleSession.Instance.Context = null;

                    WriteObject(account);
                }).ConfigureAwait(false);

        }
    }
}