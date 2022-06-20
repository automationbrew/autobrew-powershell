namespace AutoBrew.PowerShell
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Language;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Attribute used to complete the name property from the <see cref="ModuleEnvironment" /> class when used as a parameter.
    /// </summary>
    internal class EnvironmentCompleterAttribute : ArgumentCompleterAttribute, IArgumentCompleter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentCompleterAttribute" /> class.
        /// </summary>
        public EnvironmentCompleterAttribute() : base(typeof(EnvironmentCompleterAttribute))
        {
        }

        /// <summary>
        /// Used to complete arguments that part of commands.
        /// </summary>
        /// <param name="commandName">The name of the command that needs argument completion.</param>
        /// <param name="parameterName">The name of the parameter that needs argument completion.</param>
        /// <param name="wordToComplete">The word being completed.</param>
        /// <param name="commandAst">The command abstract syntax tree in case it is needed for completion.</param>
        /// <param name="fakeBoundParameters">The collection of fake bound parameters.</param>
        /// <returns>A collection of completion results.</returns>
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            foreach (string item in ModuleSession.Instance.ListEnvironments().Select(e => e.Name))
            {
                yield return new CompletionResult(item, item, CompletionResultType.ParameterName, item);
            }
        }
    }
}