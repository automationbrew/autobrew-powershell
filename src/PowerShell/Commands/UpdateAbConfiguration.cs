namespace AutoBrew.PowerShell.Commands
{
    using System.Collections.ObjectModel;
    using System.Management.Automation;
    using Configuration;
    using Models;
    using Models.Configuration;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to list configurations.
    /// </summary>
    [Cmdlet(VerbsData.Update, "AbConfiguration", SupportsShouldProcess = true)]
    [OutputType(typeof(PSConfiguration))]
    public class UpdateAbConfiguration : ModuleCmdlet, IDynamicParameters
    {
        /// <summary>
        /// The provider for configurations that are used by the module.
        /// </summary>
        private readonly IConfigurationProvider provider;

        /// <summary>
        /// The dictionary of runtime defined parameters used track the dynamic parameters for this command.
        /// </summary>
        private readonly RuntimeDefinedParameterDictionary parameters;

        /// <summary>
        /// Gets or sets the scope for the configuration.
        /// </summary>
        [Parameter(HelpMessage = "The scope for the configuration", Mandatory = true)]
        [ValidateSet(nameof(ConfigurationScope.CurrentUser), nameof(ConfigurationScope.Process))]
        public ConfigurationScope Scope { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAbConfiguration" /> class.
        /// </summary>
        /// <exception cref="ModuleException">
        /// Unable to locate the configuration provider.
        /// </exception>
        public UpdateAbConfiguration()
        {
            if (ModuleSession.Instance.TryGetComponent(ComponentType.Configuration, out provider) == false)
            {
                throw new ModuleException("Unable to locate the configuration provider.", ModuleExceptionCategory.Configuration);
            }

            parameters = new();
        }

        /// <summary>
        /// Gets the collection of runtime-based parameters for the command.
        /// </summary>
        /// <returns>A collection of runtime-defined parameters that are keyed based on the name of the parameter.</returns>
        public object GetDynamicParameters()
        {
            parameters.Clear();

            foreach (ConfigurationData item in provider.ListConfiguration())
            {
                parameters.Add(item.Definition.Key, new RuntimeDefinedParameter(
                        item.Definition.Key,
                        item.Definition.ValueType,
                        new Collection<Attribute>
                        {
                            new ParameterAttribute
                            {
                                HelpMessage = item.Definition.Description,
                                ValueFromPipelineByPropertyName = true
                            }
                        }));
            }

            return parameters;
        }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            var userDefinedParameters = parameters.Values.Where(p => p.IsSet);

            ConfirmAction(
                string.Format(Resources.UpdateConfigurationAction, string.Join(", ", userDefinedParameters.Select(p => p.Name))),
                Scope.ToString(),
                () =>
                {
                    foreach (var parameter in parameters.Values.Where(p => p.IsSet))
                    {
                        provider.UpdateConfiguration(parameter.Name, Scope, parameter.Value);
                    }
                });
        }
    }
}