namespace AutoBrew.PowerShell.Factories
{
    using AutoBrew.PowerShell.Network;
    using Microsoft.Graph;
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
        /// Creates a new instance of the <see cref="RestServiceClient" /> class used to communicate with a REST endpoint.
        /// </summary>
        /// <param name="requestData">The data used to request an access token.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="RestServiceClient" /> class used to communicate with a REST endpoint.</returns>
        Task<IRestServiceClient> CreateRestServiceClientAsync(TokenRequestData requestData, CancellationToken cancellationToken = default);
    }
}