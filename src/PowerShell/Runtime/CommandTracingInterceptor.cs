namespace AutoBrew.PowerShell.Runtime
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Microsoft.Rest;

    /// <summary>
    /// Provides the ability to trace operations performed by commands for debugging and informational purposes.
    /// </summary>
    internal class CommandTracingInterceptor : IServiceClientTracingInterceptor
    {
        /// <summary>
        /// The queue used to track messages that should be rendered in the console.
        /// </summary>
        private readonly ConcurrentQueue<string> messageQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTracingInterceptor" /> class.
        /// </summary>
        /// <param name="queue">The queue used to track messages that should be rendered in the console.</param>
        /// <exception cref="ArgumentNullException">
        /// The queue parameter is null.
        /// </exception>
        public CommandTracingInterceptor(ConcurrentQueue<string> queue)
        {
            queue.AssertNotNull(nameof(queue));

            messageQueue = queue;
        }

        /// <summary>
        /// Writes information to the trace when probing for a configuration.
        /// </summary>
        /// <param name="source">The source of the configuration.</param>
        /// <param name="name">The name of the configuration.</param>
        /// <param name="value">The value of the configuration.</param>
        public void Configuration(string source, string name, string value)
        {
        }

        /// <summary>
        /// Writes invocation information to the trace when entering a method.
        /// </summary>
        /// <param name="invocationId">The identifier for the invocation of the method.</param>
        /// <param name="instance">The instnace that contains the method.</param>
        /// <param name="method">The name of the methond that was invoked.</param>
        /// <param name="parameters">The collection that represents the parameters that were used when invoking the method.</param>
        public void EnterMethod(string invocationId, object instance, string method, IDictionary<string, object> parameters)
        {
        }

        /// <summary>
        /// Writes invocation information to the trace when exiting a method.
        /// </summary>
        /// <param name="invocationId">The identifier for the invocation of the method.</param>
        /// <param name="returnValue">The value returned from the method.</param>
        public void ExitMethod(string invocationId, object returnValue)
        {
        }

        /// <summary>
        /// Writes informational messages to the trace.
        /// </summary>
        /// <param name="message">The message to included in the trace.</param>
        /// <exception cref="ArgumentException">
        /// The message parameter is empty or null.
        /// </exception>
        public void Information(string message)
        {
            message.AssertNotEmpty();

            messageQueue.Enqueue(message);
        }

        /// <summary>
        /// Writes information to the trace when a HTTP response has been received.
        /// </summary>
        /// <param name="invocationId">The identifier for the invocation of the method.</param>
        /// <param name="response">The response from the HTTP operation.</param>
        /// <exception cref="ArgumentException">
        /// The invocationId parameter is empty or null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The response parameter is null.
        /// </exception>
        public void ReceiveResponse(string invocationId, HttpResponseMessage response)
        {
            invocationId.AssertNotEmpty(nameof(invocationId));
            response.AssertNotNull(nameof(response));

            StringBuilder output = new();

            output.AppendLine($"============================ HTTP RESPONSE ============================");
            output.AppendLine($"Status Code:{Environment.NewLine}{response.StatusCode}{Environment.NewLine}");
            output.AppendLine($"Headers:");

            foreach (KeyValuePair<string, IEnumerable<string>> item in response.Headers.ToDictionary(h => h.Key, h => h.Value).ToArray())
            {
                output.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-30}: {1}",
                    item.Key,
                    string.Join(",", item.Value)));
            }

            if (response.Content is not null)
            {
                output.AppendLine(string.Empty);
                output.AppendLine("Body:");
                output.AppendLine(TryFormatJson(response.Content.AsString()));
            }

            messageQueue.Enqueue(output.ToString());
        }

        /// <summary>
        /// Writes information to the trace when sending a HTTP request.
        /// </summary>
        /// <param name="invocationId">The identifier for the invocation of the method.</param>
        /// <param name="request">The HTTP request that is being sent.</param>
        /// <exception cref="ArgumentException">
        /// The invocationId parameter is empty or null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The request parameter is null.
        /// </exception>
        public void SendRequest(string invocationId, HttpRequestMessage request)
        {
            invocationId.AssertNotEmpty(nameof(invocationId));
            request.AssertNotNull(nameof(request));

            StringBuilder output = new();

            output.AppendLine($"============================ HTTP REQUEST ============================");
            output.AppendLine($"HTTP Method:{Environment.NewLine}{request.Method}{Environment.NewLine}");
            output.AppendLine($"Absolute Uri:{Environment.NewLine}{request.RequestUri}{Environment.NewLine}");
            output.AppendLine($"Headers:");

            foreach (KeyValuePair<string, IEnumerable<string>> item in request.Headers.Where(h => !h.Key.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase)).ToDictionary(h => h.Key, h => h.Value).ToArray())
            {
                output.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-30}: {1}",
                    item.Key,
                    string.Join(",", item.Value)));
            }

            if (request.Content is not null)
            {
                output.AppendLine(string.Empty);
                output.AppendLine("Body:");
                output.AppendLine(TryFormatJson(request.Content.AsString()));
            }

            messageQueue.Enqueue(output.ToString());
        }

        /// <summary>
        /// Writes information to the trace when an error has been encountered.
        /// </summary>
        /// <param name="invocationId">The identifier for the invocation of the method.</param>
        /// <param name="exception">The exception that was thrown during the invocation of the method.</param>
        public void TraceError(string invocationId, Exception exception)
        {
        }

        /// <summary>
        /// Formats the specified content using pretty printing.
        /// </summary>
        /// <param name="content">The content to be formatted using pretty printing.</param>
        /// <returns>The content formatted using pretty print when it can be serialized; otherwise, the original content.</returns>
        private static string TryFormatJson(string content)
        {
            try
            {
                return JsonNode.Parse(content).ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception)
            {
                return content;
            }
        }
    }
}