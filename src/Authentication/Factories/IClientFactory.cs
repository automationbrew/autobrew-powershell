namespace AutoBrew.PowerShell.Factories
{
    using Microsoft.Graph;
    using Microsoft.Store.PartnerCenter;
    using Models.Authentication;

    /// <summary>
    /// Represents the factory used to create clients used to communicate with online services.
    /// </summary>
    public interface IClientFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="GraphServiceClient" /> class used to communicate with Microsoft Graph.
        /// </summary>
        /// <param name="account">An instance of the <see cref="ModuleAccount" /> class that provides information used to authenticate.</param>
        /// <returns>An instance of the <see cref="GraphServiceClient" /> class used to communicate with Microsoft Graph.</returns>
        GraphServiceClient CreateGraphServiceClient(ModuleAccount account);

        /// <summary>
        /// Creates a new instance of the <see cref="PartnerOperations" /> class used to communicate with Partner Center.
        /// </summary>
        /// <param name="account">An instance of the <see cref="ModuleAccount" /> class that provides information used to authenticate.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="PartnerOperations" /> class used to communicate with partner Center.</returns>
        Task<IPartner> CreatePartnerOperationsAsync(ModuleAccount account, CancellationToken cancellationToken = default);
    }
}