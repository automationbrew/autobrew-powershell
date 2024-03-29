﻿namespace AutoBrew.PowerShell.Network
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Rest;

    /// <summary>
    /// Delegating handler that provides tracing the request and response.
    /// </summary>
    public sealed class TracingHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The response from the execution of the operation.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string invocationId = null;

            request.AssertNotNull(nameof(request));

            if (ServiceClientTracing.IsEnabled)
            {
                invocationId = ServiceClientTracing.NextInvocationId.ToString(CultureInfo.InvariantCulture);

                NameValueCollection queryParameters = HttpUtility.ParseQueryString(request.RequestUri.Query);
                Dictionary<string, object> tracingParameters = new();

                foreach (string key in queryParameters.AllKeys)
                {
                    tracingParameters.Add(key, queryParameters[key]);
                }

                ServiceClientTracing.Enter(invocationId, this, "Send", tracingParameters);
                ServiceClientTracing.SendRequest(invocationId, request);
            }

            cancellationToken.ThrowIfCancellationRequested();
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (ServiceClientTracing.IsEnabled)
            {
                ServiceClientTracing.ReceiveResponse(invocationId, response);
                ServiceClientTracing.Exit(invocationId, null);
            }

            return response;
        }
    }
}