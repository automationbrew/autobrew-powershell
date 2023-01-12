namespace AutoBrew.PowerShell.Network
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;

    /// <summary>
    /// Provides the ability to interact with a REST service.
    /// </summary>
    internal class RestServiceClient : ServiceClient<RestServiceClient>, IRestServiceClient
    {
        /// <summary>
        /// The media type for requests that involve JSON content. 
        /// </summary>
        private const string JSON_MEDIA_TYPE = "application/json";

        /// <summary>
        /// Options to control the behavior during parsing.
        /// </summary>
        private static readonly JsonSerializerOptions serializerOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// The credentials used to access a secure resource.
        /// </summary>
        private readonly ServiceClientCredentials serviceCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceClient" /> class.
        /// </summary>
        /// <param name="credentials">The credentials used to access a secure resource.</param>
        /// <param name="httpClient">The client used to perform HTTP operations.</param>
        /// <param name="disposeHttpClient">A flag indicating whether the instance of the <see cref="HttpClient" /> class should be disposed.</param>
        /// <exception cref="ArgumentNullException">
        /// The credentials parameter is null.
        /// or
        /// The httpClient parameter is null.
        /// </exception>
        public RestServiceClient(ServiceClientCredentials credentials, HttpClient httpClient, bool disposeHttpClient)
            : base(httpClient, disposeHttpClient)
        {
            credentials.AssertNotNull(nameof(credentials));
            httpClient.AssertNotNull(nameof(httpClient));

            serviceCredentials = credentials;
        }

        /// <summary>
        /// Performs a HTTP GET operation using the specified parameters.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">A string that represents the URI for the request.</param>
        /// <param name="parameters">The collection of parameters that will be used to construct the query string parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The response from the request.</returns>
        public async Task<TResponse> GetAsync<TResponse>(Uri requestUri, IDictionary<string, string> parameters = null, CancellationToken cancellationToken = default)
        {
            string invocationId = null;

            if (parameters != null && parameters.Count > 0)
            {
                UriBuilder builder = new(requestUri)
                {
                    Query = $"{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}"
                };

                requestUri = builder.Uri;
            }

            using HttpRequestMessage request = new(HttpMethod.Get, requestUri);

            cancellationToken.ThrowIfCancellationRequested();
            await serviceCredentials.ProcessHttpRequestAsync(request, cancellationToken).ConfigureAwait(false);

            if (ServiceClientTracing.IsEnabled)
            {
                invocationId = ServiceClientTracing.NextInvocationId.ToString();

                Dictionary<string, object> tracingParameters = new()
                {
                    { "requestUri", requestUri },
                    { "parameters", parameters },
                };

                ServiceClientTracing.Enter(invocationId, this, HttpMethod.Get.ToString(), tracingParameters);
            }

            return await ParseResponseAsync<TResponse>(invocationId, request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs a HTTP POST operation using the specified parameters.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">A string that represents the URI for the request.</param>
        /// <param name="content">The content to be included in the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The response from the request.</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(Uri requestUri, TRequest content, CancellationToken cancellationToken = default)
        {
            string invocationId = null;

            using HttpRequestMessage request = new(HttpMethod.Post, requestUri);

            cancellationToken.ThrowIfCancellationRequested();
            await serviceCredentials.ProcessHttpRequestAsync(request, cancellationToken).ConfigureAwait(false);

            request.Content = JsonContent.Create(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(JSON_MEDIA_TYPE);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JSON_MEDIA_TYPE));

            if (ServiceClientTracing.IsEnabled)
            {
                invocationId = ServiceClientTracing.NextInvocationId.ToString();

                Dictionary<string, object> tracingParameters = new()
                {
                    { "requestUri", requestUri },
                    { "content", content }
                };

                ServiceClientTracing.Enter(invocationId, this, HttpMethod.Post.ToString(), tracingParameters);
            }

            return await ParseResponseAsync<TResponse>(invocationId, request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Parses the response from the HTTP operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="invocationId">The identifier for the invocation of the function.</param>
        /// <param name="request">An instance of the <see cref="HttpRequestMessage" /> class that represents the request to be made.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The response from the request.</returns>
        private async Task<TResponse> ParseResponseAsync<TResponse>(string invocationId, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            bool shouldTrace = ServiceClientTracing.IsEnabled;

            if (shouldTrace)
            {
                ServiceClientTracing.SendRequest(invocationId, request);
            }

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseMessage response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(invocationId, response);
            }

            string responseContent = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return typeof(TResponse) == typeof(HttpResponseMessage)
                    ? (TResponse)Convert.ChangeType(response, typeof(TResponse)) : JsonSerializer.Deserialize<TResponse>(responseContent, serializerOptions);
            }

            RestServiceException exception = new(response.ReasonPhrase)
            {
                Request = new HttpRequestMessageWrapper(request, null),
                Response = new HttpResponseMessageWrapper(response, responseContent)
            };

            if (shouldTrace)
            {
                ServiceClientTracing.Error(invocationId, exception);
            }

            throw exception;
        }
    }
}