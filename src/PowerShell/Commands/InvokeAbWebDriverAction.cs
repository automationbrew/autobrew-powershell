namespace AutoBrew.PowerShell.Commands
{
    using System.Collections;
    using System.Globalization;
    using System.Management.Automation;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.RegularExpressions;
    using Models.Automation;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtras.WaitHelpers;

    /// <summary>
    /// Invokes a series of web driver actions defined by a template.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "AbWebDriverAction", DefaultParameterSetName = TemplateParameterSetName)]
    public sealed class InvokeAbWebDriverAction : ModuleAsyncCmdlet
    {
        /// <summary>
        /// The default wait threshold for a web element to be visible represented in seconds.
        /// </summary>
        private const int DefaultWaitThresholdInSeconds = 15;

        /// <summary>
        /// The name for the template parameter set.
        /// </summary>
        private const string TemplateParameterSetName = "TemplateParameterSet";

        /// <summary>
        /// The name for the template file parameter set.
        /// </summary>
        private const string TemplateFileParameterSetName = "TemplateFileParameterSet";

        /// <summary>
        /// Gets or sets the credentials to be used for authentication after navigating to the web address.
        /// </summary>
        [Parameter(HelpMessage = "The credentials to be used for authentication after navigating to the web address.", Mandatory = false)]
        [ValidateNotNull]
        public PSCredential Credential { get; set; }

        /// <summary>
        /// Gets or sets the parameters used to populate variables in the template.
        /// </summary>
        [Parameter(HelpMessage = "The parameters used to populate variables in the template.", Mandatory = false)]
        [ValidateNotNull]
        public Hashtable Parameters { get; set; }

        /// <summary>
        /// Gets or sets the web address where the web driver will navigate.
        /// </summary>
        [Parameter(HelpMessage = "The web address where the web driver will navigate.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string NavigationUrl { get; set; }

        /// <summary>
        /// Gets or sets the template, represented by JSON, that contains the web driver actions to be performed.
        /// </summary>
        [Parameter(HelpMessage = "The template, represented by JSON, that contains the web driver actions to be performed.", Mandatory = true, ParameterSetName = TemplateParameterSetName)]
        [ValidateNotNullOrEmpty]
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the path for the template file that contains the web drive actions to be performed.
        /// </summary>
        [Parameter(HelpMessage = "The path for the template file that contains the web drive actions to be performed.", Mandatory = true, ParameterSetName = TemplateFileParameterSetName)]
        [ValidateNotNullOrEmpty]
        public string TemplateFile { get; set; }

        /// <summary>
        /// Performs the operations associated with the cmdlet.
        /// </summary>
        /// <exception cref="ModuleException">The {action.ActionType} action type is not supported.</exception>
        protected override async Task PerformCmdletAsync()
        {
            using EdgeDriver driver = InitializeWebDrive() as EdgeDriver;
            JsonSerializerOptions options;
            List<WebDriverAction> driverActions;

            // Navigate to the web addressed that was request through the NavigationUrl parameter for the cmdlet.
            driver.Navigate().GoToUrl(NavigationUrl);

            // Initialize the serializer options and configure a converter to translate enumeration values to strings.
            options = new JsonSerializerOptions
            {
                Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
            };

            if (string.IsNullOrEmpty(Template) == false)
            {
                // Deserialize the template, which is represented by JSON, so the actions can be invoked.
                driverActions = JsonSerializer.Deserialize<List<WebDriverAction>>(Template, options);
            }
            else
            {
                // Read the template file and deserialize the contents, so the actions can be invoked.
                driverActions = JsonSerializer.Deserialize<List<WebDriverAction>>(File.ReadAllText(TemplateFile), options);
            }

            foreach (WebDriverAction driverAction in driverActions.OrderBy(a => a.Sequence))
            {
                if (driverAction.ActionType == WebDriverActionType.Authenticate)
                {
                    PerformAuthentication(driver, WebDriverActionType.Authenticate);
                }
                else if (driverAction.ActionType == WebDriverActionType.AuthenticatePasswordOnly)
                {
                    PerformAuthentication(driver, WebDriverActionType.AuthenticatePasswordOnly);
                }
                else if (driverAction.ActionType == WebDriverActionType.Click)
                {
                    WaitForElementToBeVisible(driver, driverAction).Click();
                }
                else if (driverAction.ActionType == WebDriverActionType.SendKeys)
                {
                    IWebElement element = WaitForElementToBeVisible(driver, driverAction);

                    foreach (WebElementValue value in driverAction.WebElementValues)
                    {
                        if (value.KeyValue.HasValue && string.IsNullOrEmpty(value.Text) == false)
                        {
                            element.SendKeys($"{Convert.ToString(Convert.ToChar(value.KeyValue.Value, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)}{value.Text}");
                        }
                        else if (value.KeyValue.HasValue && string.IsNullOrEmpty(value.Text))
                        {
                            element.SendKeys(Convert.ToString(Convert.ToChar(value.KeyValue.Value, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
                        }
                        else if (string.IsNullOrEmpty(value.Text) == false)
                        {
                            element.SendKeys(DecodeVariable(value.Text));
                        }
                        else
                        {
                            throw new ModuleException(
                                "When using the send keys web driver action type a key value or text needs to be specified.",
                                ModuleExceptionCategory.WebDriver);
                        }
                    }
                }
                else if (driverAction.ActionType == WebDriverActionType.SwitchWindow)
                {
                    // Switch to the window that is represented by the last window handle.
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                }
                else if (driverAction.ActionType == WebDriverActionType.TimeDelay)
                {
                    await Task.Delay(driverAction.WaitThresholdInSeconds * 1000).ConfigureAwait(false);
                }
                else
                {
                    throw new ModuleException(
                        $"The {driverAction.ActionType} action type is not supported.",
                        ModuleExceptionCategory.WebDriver);
                }
            }
        }

        /// <summary>
        /// Decodes any variables that are present in the specified text.
        /// </summary>
        /// <param name="text">The text that might contain variables that need to be decoded.</param>
        /// <returns>A string representing the text where the variables have been decoded.</returns>
        private string DecodeVariable(string text)
        {
            Regex regex = new(@"(?<=\{\{)[^}]*(?=\}\})", RegexOptions.Compiled);
            string value = text;

            if (Parameters == null)
            {
                return text;
            }

            foreach (Match match in regex.Matches(value))
            {
                if (Parameters.ContainsKey(match.Value))
                {
                    value = value.Replace("{{" + match.Value + "}}", Parameters[match.Value].ToString());
                }
            }

            return value;
        }

        /// <summary>
        /// Gets an instance of the <see cref="By" /> class that represents the strategy for how a web element should be located.
        /// </summary>
        /// <param name="locatorType">The type of locator to be used to locate the web element.</param>
        /// <param name="webElementToFind">The value that represents how the web element will be located.</param>
        /// <returns>
        /// An instance of the <see cref="By" /> class that represents the strategy for how a web element should be located.
        /// </returns>
        /// <exception cref="ArgumentNullException">The locatorType parameter is null.</exception>
        /// <exception cref="ArgumentException">The webElement parameter is empty or null.</exception>
        /// <exception cref="ModuleException">The {locatorType} locator type is not supported.</exception>
        private static By GetByParameter(WebElementLocatorType locatorType, string webElementToFind)
        {
            locatorType.AssertNotNull(nameof(locatorType));
            webElementToFind.AssertNotEmpty(nameof(webElementToFind));

            if (locatorType == WebElementLocatorType.Id)
            {
                return By.Id(webElementToFind);
            }
            else if (locatorType == WebElementLocatorType.Name)
            {
                return By.Name(webElementToFind);
            }
            else if (locatorType == WebElementLocatorType.StyleSheet)
            {
                return By.CssSelector(webElementToFind);
            }
            else if (locatorType == WebElementLocatorType.XPath)
            {
                return By.XPath(webElementToFind);
            }

            throw new ModuleException($"The {locatorType} locator type is not supported.", ModuleExceptionCategory.WebDriver);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="WebDriver" /> class.
        /// </summary>
        /// <returns>An initializes instance of the <see cref="WebDriver" /> class.</returns>
        private static IWebDriver InitializeWebDrive()
        {
            EdgeOptions options = new();

            options.AddArguments("--disable-gpu");
            options.AddArguments("--inprivate");
            options.AddArguments("--no-sandbox");

            return new EdgeDriver(options);
        }

        /// <summary>
        /// Waits until the specified web element is visible.
        /// </summary>
        /// <param name="driver">The driver used to automate actions within a web browser.</param>
        /// <param name="locator">An instance of the <see cref="By" /> class that represent how the web element will be located.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// The driver parameter is null.
        /// or 
        /// The locator parameter is null.
        /// </exception>
        private static IWebElement WaitForElementToBeVisible(IWebDriver driver, By locator)
        {
            WebDriverWait driverWait;

            driver.AssertNotNull(nameof(driver));
            locator.AssertNotNull(nameof(locator));

            driverWait = new(driver, TimeSpan.FromSeconds(DefaultWaitThresholdInSeconds));

            return driverWait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>
        /// Waits until the specified web element is visible.
        /// </summary>
        /// <param name="driver">The driver used to automate actions within a web browser.</param>
        /// <param name="driverAction">An instance of the <see cref="WebDriverAction" /> class that defined the intended action.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// The driver parameter is null.
        /// or 
        /// The driverAction parameter is null.
        /// </exception>
        private static IWebElement WaitForElementToBeVisible(IWebDriver driver, WebDriverAction driverAction)
        {
            WebDriverWait driverWait;

            driver.AssertNotNull(nameof(driver));
            driverAction.AssertNotNull(nameof(driverAction));

            driverWait = new(driver, TimeSpan.FromSeconds(driverAction.WaitThresholdInSeconds));

            return driverWait.Until(
                ExpectedConditions.ElementIsVisible(GetByParameter(driverAction.WebElementLocatorType, driverAction.WebElementLocator)));
        }

        /// <summary>
        /// Performs the actions using an instance of the <see cref="WebDriver" /> class to authenticate.
        /// </summary>
        /// <param name="driver">The driver used to automate actions within a web browser.</param>
        /// <param name="actionType">The type of action to perform.</param>
        /// <exception cref="ArgumentNullException">
        /// The driver parameter is null.
        /// or
        /// The actionType parameter is null.
        /// </exception>
        /// <exception cref="ModuleException">
        /// The action type {actionType} cannot be used for authentication.
        /// or
        /// Unable to perform the operations associated with authentication because the credential parameter is null.
        /// </exception>
        private void PerformAuthentication(IWebDriver driver, WebDriverActionType actionType)
        {
            driver.AssertNotNull(nameof(driver));
            actionType.AssertNotNull(nameof(WebDriverActionType));

            if (Credential == null)
            {
                throw new ModuleException(
                    "Unable to perform the operations associated with authentication because the credential parameter is null.",
                    ModuleExceptionCategory.WebDriver);
            }

            if (actionType.HasFlag(WebDriverActionType.Authenticate | WebDriverActionType.AuthenticatePasswordOnly) == false)
            {
                throw new ModuleException(
                    $"The action type {actionType} cannot be used for authentication.",
                    ModuleExceptionCategory.WebDriver);
            }

            if (actionType == WebDriverActionType.Authenticate)
            {
                // Wait until the username text box is visible and obtain a reference to it.
                IWebElement txtUser = WaitForElementToBeVisible(driver, By.Name("loginfmt"));

                // Set the focus to the username text box and populate it with the specified username. 
                txtUser.Click();
                txtUser.SendKeys(Credential.UserName);

                // Wait until the next button is visible and then click it.
                WaitForElementToBeVisible(driver, By.Id("idSIButton9")).Click();
            }

            // Wait until the password text box is visible and obtain a reference to it.
            IWebElement txtPassword = WaitForElementToBeVisible(driver, By.Id("i0118"));

            // Set the focus to the password text box and populate it with the specified password.
            txtPassword.Click();
            txtPassword.SendKeys(Credential.Password.AsString());

            // Wait until the submit button is visible and then click it.
            WaitForElementToBeVisible(driver, By.Id("idSIButton9")).Click();

            // Wait until the stay signed in confirmation button is visible and then click it.
            WaitForElementToBeVisible(driver, By.Id("idSIButton9")).Click();
        }
    }
}