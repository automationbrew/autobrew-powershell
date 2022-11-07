namespace AutoBrew.PowerShell.Commands
{
    using System.Globalization;
    using System.Management.Automation;
    using System.Net.NetworkInformation;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using Configuration;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Rest;
    using Models;
    using Models.Configuration;
    using Properties;

    /// <summary>
    /// Base for commands that should be performed synchronously.
    /// </summary>
    public abstract class ModuleCmdlet : PSCmdlet, IDisposable
    {
        /// <summary>
        /// The link that provides additional information about breaking changes for the module.
        /// </summary>
        private const string BREAKING_CHANGE_ATTRIBUTE_INFORMATION_LINK = "https://github.com/automationbrew/autobrew-powershell/blob/main/docs/breaking-defined.md";

        /// <summary>
        /// The name for the event that is captured by the telemetry if enabled.
        /// </summary>
        private const string EVENT_NAME = "cmdletInvocation";

        /// <summary>
        /// The connectiong string for the telemetry client.
        /// </summary>
        private const string TELEMETRY_CONNECTION_STRING = "InstrumentationKey=e48b6543-12b1-45ba-8416-4b8e917ab0cf;IngestionEndpoint=https://westus2-2.in.applicationinsights.azure.com/;LiveEndpoint=https://westus2.livediagnostics.monitor.azure.com/";

        /// <summary>
        /// The client that provides the ability to capture telemetry if enabled.
        /// </summary>
        private static readonly TelemetryClient telemetryClient = new(telemetryConfiguration);

        /// <summary>
        /// The configuration for the telemetry client.
        /// </summary>
        private static readonly TelemetryConfiguration telemetryConfiguration = new()
        {
            ConnectionString = TELEMETRY_CONNECTION_STRING
        };

        /// <summary>
        /// The provider of configurations for the module.
        /// </summary>
        private readonly IConfigurationProvider configuration;

        /// <summary>
        /// The resource lock used to synchronize mutation of the telemetry.
        /// </summary>
        private readonly object resourceLock = new();

        /// <summary>
        /// A flag indicating if the object has already been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The quality of service event that will be captured by telemetry if enabled.
        /// </summary>
        private QosEvent qosEvent;

        /// <summary>
        /// The hash of the MAC address for the device invoking the command.
        /// </summary>
        private string hashMacAddress;

        /// <summary>
        /// Gets the cancellation token used to propagate a notification that operations should be canceled.
        /// </summary>
        protected CancellationToken CancellationToken => CancellationTokenSource.Token;

        /// <summary>
        /// Get the resource that signals to a <see cref="System.Threading.CancellationToken"/> that it should be canceled.
        /// </summary>
        internal CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Gets the hash of the MAC address for the device invoking the command.
        /// </summary>
        private string HashMacAddress
        {
            get
            {
                lock (resourceLock)
                {
                    try
                    {
                        hashMacAddress = null;

                        if (string.IsNullOrEmpty(hashMacAddress))
                        {
                            string value = NetworkInterface.GetAllNetworkInterfaces()?
                                 .FirstOrDefault(nic => nic != null &&
                                     nic.OperationalStatus == OperationalStatus.Up &&
                                     !string.IsNullOrWhiteSpace(nic.GetPhysicalAddress()?.ToString()))?.GetPhysicalAddress()?.ToString();

                            hashMacAddress = string.IsNullOrWhiteSpace(value) ? null : GenerateSha256HashString(value)?.Replace("-", string.Empty)?.ToLowerInvariant();
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore errors with obtaining the MAC address
                    }
                }

                return hashMacAddress;
            }
        }

        /// <summary>
        /// Gets the quality of service event.
        /// </summary>
        protected QosEvent QosEvent => qosEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCmdlet" /> class.
        /// </summary>
        public ModuleCmdlet()
        {
            CancellationTokenSource = new();

            if (ModuleSession.Instance.TryGetComponent(ComponentType.Configuration, out configuration) == false)
            {
                throw new ModuleException("Unable to locate the configuration provider.", ModuleExceptionCategory.Configuration);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                CancellationTokenSource?.Dispose();
            }

            disposed = true;
        }

        /// <summary>
        /// Performs the actions when processing for the command has begun.
        /// </summary>
        protected override void BeginProcessing()
        {
            string commandAlias = GetType().Name;

            if (MyInvocation != null && MyInvocation.MyCommand != null)
            {
                commandAlias = MyInvocation.MyCommand.Name;
            }

            qosEvent = new()
            {
                CommandName = commandAlias,
                IsSuccess = true,
                ModuleVersion = GetType().Assembly.GetName().Version.ToString(),
                ParameterSetName = ParameterSetName
            };

            if (MyInvocation != null && MyInvocation.BoundParameters != null && MyInvocation.BoundParameters.Keys != null)
            {
                qosEvent.Parameters = string.Join(" ", MyInvocation.BoundParameters.Keys.Select(
                    s => string.Format(CultureInfo.InvariantCulture, "-{0} ***", s)));
            }

            ProcessBreakingChangeAttributes(GetType(), MyInvocation);
        }

        /// <summary>
        /// Confirms the action should be performed.
        /// </summary>
        /// <param name="action">Name of the action which is being performed.</param>
        /// <param name="target">Name of the target resource being acted upon.</param>
        /// <param name="method">The method to be performed once the action has been confirmed.</param>
        /// <exception cref="ArgumentException">
        /// The action parameter is empty or null.
        /// or
        /// The target parameter is empty or null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The method parameter is null.
        /// </exception>
        protected virtual void ConfirmAction(string action, string target, Action method)
        {
            action.AssertNotEmpty(nameof(action));
            method.AssertNotNull(nameof(method));
            target.AssertNotEmpty(nameof(target));

            if (qosEvent is not null)
            {
                qosEvent.PauseQoSTimer();
            }

            if (ShouldProcess(target, action) == false)
            {
                return;
            }

            if (qosEvent is not null)
            {
                qosEvent.ResumeQoSTimer();
            }

            method();
        }

        /// <summary>
        /// Performs the actions when processing for the command has ended.
        /// </summary>
        protected override void EndProcessing()
        {
            if (CancellationTokenSource.IsCancellationRequested == false)
            {
                CancellationTokenSource.Cancel();
            }

            LogQosEvent();
        }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected abstract void PerformCmdlet();

        /// <summary>
        /// Performs the actions associated with the command and handles exceptions accordingly.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                PerformCmdlet();
            }
            catch (Exception ex) when (IsTerminatingError(ex) == false)
            {
                WriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
            }
        }

        /// <summary>
        /// Performs the actions to stop processing the command.
        /// </summary>
        protected override void StopProcessing()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
        }

        /// <summary>
        /// Generates a SHA256 hash for the specified input.
        /// </summary>
        /// <param name="input">The input that should be hashed.</param>
        /// <returns>A string that represents the SHA256 hash for the specified input.</returns>
        private static string GenerateSha256HashString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            string result = null;

            try
            {
                using SHA256CryptoServiceProvider sha256 = new();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                result = BitConverter.ToString(bytes);
            }
            catch
            {
                // do not throw if CryptoProvider is not provided
            }

            return result;
        }

        /// <summary>
        /// Gets all the breaking change attributes for the specified type.
        /// </summary>
        /// <param name="type">The type to check for breaking change attributes.</param>
        /// <param name="invocationInfo">The description for how and where the command was invoked.</param>
        /// <returns>A collection of breaking change attributes for the specified type.</returns>
        private static IEnumerable<BreakingChangeAttribute> GetAllBreakingChangeAttributes(Type type, InvocationInfo invocationInfo)
        {
            List<BreakingChangeAttribute> attributeList = new();

            attributeList.AddRange(type.GetCustomAttributes(typeof(BreakingChangeAttribute), false).Cast<BreakingChangeAttribute>());

            foreach (MethodInfo m in type.GetRuntimeMethods())
            {
                attributeList.AddRange((m.GetCustomAttributes(typeof(BreakingChangeAttribute), false).Cast<BreakingChangeAttribute>()));
            }

            foreach (FieldInfo f in type.GetRuntimeFields())
            {
                attributeList.AddRange(f.GetCustomAttributes(typeof(BreakingChangeAttribute), false).Cast<BreakingChangeAttribute>());
            }

            foreach (PropertyInfo p in type.GetRuntimeProperties())
            {
                attributeList.AddRange(p.GetCustomAttributes(typeof(BreakingChangeAttribute), false).Cast<BreakingChangeAttribute>());
            }

            return invocationInfo == null ? attributeList : attributeList.Where(e => e.IsApplicableToInvocation(invocationInfo));
        }

        /// <summary>
        /// Gets a flag that indicates if the exception is a terminating error.
        /// </summary>
        /// <param name="ex">An instance of the <see cref="Exception" /> class that represents the exception that was thrown.</param>
        /// <returns><c>true</c> if the exception is a terminating error; otherwise, <c>false</c>.</returns>
        private bool IsTerminatingError(Exception ex)
        {
            return ex is PipelineStoppedException pipelineStoppedEx && pipelineStoppedEx.InnerException == null;
        }

        /// <summary>
        /// Logs the exception if the telemetry for the module is enabled.
        /// </summary>
        private void LogExceptionEvent()
        {
            if (qosEvent == null)
            {
                return;
            }

            ExceptionTelemetry exceptionTelemetry = new(qosEvent.Exception)
            {
                Message = "The message has been removed due to PII"
            };

            if (qosEvent.Exception is RestException)
            {
                object ex = qosEvent.Exception;
                HttpResponseMessageWrapper response = ex.GetType().GetProperty("Response").GetValue(ex, null) as HttpResponseMessageWrapper;

                PopulatePropertiesFromResponse(exceptionTelemetry.Properties, response);
            }

            exceptionTelemetry.Metrics.Add("Duration", qosEvent.Duration.TotalMilliseconds);
            PopulatePropertiesFromQos(exceptionTelemetry.Properties);

            try
            {
                telemetryClient.TrackException(exceptionTelemetry);
            }
            catch
            {
                // Ignore any error with capturing the telemetry
            }
        }

        /// <summary>
        /// Logs the quality of service event if the telemetry for the module is enabled.
        /// </summary>
        private void LogQosEvent()
        {
            if (qosEvent == null)
            {
                return;
            }

            qosEvent.FinishQosEvent();

            (_, bool isTelemetryEnabled) = configuration.GetConfigurationValue<bool>(ConfigurationKey.DataCollection);

            if (isTelemetryEnabled == false)
            {
                return;
            }

            PageViewTelemetry pageViewTelemetry = new()
            {
                Name = EVENT_NAME,
                Duration = qosEvent.Duration,
                Timestamp = qosEvent.StartTime
            };

            pageViewTelemetry.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            PopulatePropertiesFromQos(pageViewTelemetry.Properties);

            try
            {
                telemetryClient.TrackPageView(pageViewTelemetry);
            }
            catch
            {
                // Ignore any error with capturing the telemetry
            }

            if (qosEvent.Exception != null)
            {
                LogExceptionEvent();
            }
        }

        /// <summary>
        /// Populates the event properties based on the quality of service event.
        /// </summary>
        /// <param name="eventProperties">The collection to be populates based on the quality of service event.</param>
        private void PopulatePropertiesFromQos(IDictionary<string, string> eventProperties)
        {
            if (qosEvent == null)
            {
                return;
            }

            eventProperties.Add("Command", qosEvent.CommandName);
            eventProperties.Add("CommandParameterSetName", qosEvent.ParameterSetName);
            eventProperties.Add("CommandParameters", qosEvent.Parameters);
            eventProperties.Add("HashMacAddress", HashMacAddress);
            eventProperties.Add("HostVersion", qosEvent.HostVersion);
            eventProperties.Add("IsSuccess", qosEvent.IsSuccess.ToString());
            eventProperties.Add("ModuleVersion", qosEvent.ModuleVersion);
            eventProperties.Add("PowerShellVersion", Host.Version.ToString());

            if (!string.IsNullOrEmpty(ModuleSession.Instance.Context?.Account?.Tenant))
            {
                eventProperties.Add("TenantId", ModuleSession.Instance.Context.Account.Tenant);
            }
        }

        /// <summary>
        /// Populates the event properties based on the HTTP response.
        /// </summary>
        /// <param name="eventProperties">The collection to be populates based on the HTTP response.</param>
        /// <param name="response">The HTTP response to be used when populating the event properties.</param>
        private void PopulatePropertiesFromResponse(IDictionary<string, string> eventProperties, HttpResponseMessageWrapper response)
        {
            if (response == null)
            {
                return;
            }

            eventProperties.Add("ReasonPhrase", response.ReasonPhrase);
            eventProperties.Add("StatusCode", response.StatusCode.ToString());
        }

        /// <summary>
        /// Processes any breaking change attributes defined within the command.
        /// </summary>
        /// <param name="type">The type for the command that was invoked invoked.</param>
        /// <param name="invocationInfo">The description for how and where the command was invoked.</param>
        private void ProcessBreakingChangeAttributes(Type type, InvocationInfo invocationInfo)
        {
            List<BreakingChangeAttribute> attributes = new(GetAllBreakingChangeAttributes(type, invocationInfo));
            StringBuilder output = new();

            if (attributes.Count > 0)
            {
                output.AppendLine(
                    string.Format(
                        Resources.BreakingChangesAttributesHeaderMessage,
                        BreakingChangeAttribute.GetNameFromCmdletType(type)));

                foreach (BreakingChangeAttribute attribute in attributes)
                {
                    attribute.PrintCustomAttributeInfo(type, false, (string value) => output.Append(value));
                }

                string.Format(Resources.BreakingChangesAttributesFooterMessage, BREAKING_CHANGE_ATTRIBUTE_INFORMATION_LINK);

                (_, bool isDisplayBreakingChangesEnabled) = configuration.GetConfigurationValue<bool>(ConfigurationKey.DisplayBreakingChanges);

                if (isDisplayBreakingChangesEnabled)
                {
                    WriteWarning(output.ToString());
                }
            }
        }
    }
}