namespace AutoBrew.PowerShell.Factories
{
    using Microsoft.Graph;
    using Models.Authentication;

    /// <summary>
    /// Represents the factory used to create clients used to communicate with online services.
    /// </summary>
    public interface IClientFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="GraphServiceClient" /> used to communicate with Microsoft Graph.
        /// </summary>
        /// <param name="account">An instance of the <see cref="ModuleAccount" /> class that provides information used to authenticate.</param>
        /// <returns>An instance of the <see cref="GraphServiceClient" /> used to communicate with Microsoft Graph.</returns>
        GraphServiceClient CreateGraphServiceClient(ModuleAccount account);
    }
}