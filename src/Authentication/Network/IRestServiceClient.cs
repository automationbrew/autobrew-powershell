namespace AutoBrew.PowerShell.Network
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the ability to interact with a rest based service.
    /// </summary>
    internal interface IRestServiceClient
    {
        /// <summary>
        /// Performs a HTTP GET operation using the specified parameters.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">A string that represents the URI for the request.</param>
        /// <param name="parameters">The collection of parameters that will be used to construct the query string parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The response from the request.</returns>
        Task<TResponse> GetAsync<TResponse>(Uri requestUri, IDictionary<string, string> parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a HTTP POST operation using the specified parameters.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">A string that represents the URI for the request.</param>
        /// <param name="content">The content to be included in the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The response from the request.</returns>
        Task<TResponse> PostAsync<TRequest, TResponse>(Uri requestUri, TRequest content, CancellationToken cancellationToken = default);
    }
}