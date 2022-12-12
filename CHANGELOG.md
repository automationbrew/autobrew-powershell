<!--
    Please leave this section at the top of the change log.

    Changes for the upcoming release should go under the section titled "Upcoming Release", and should adhere to the following format:

    ## Upcoming Release
    * Overview of change #1
        - Additional information about change #1
    * Overview of change #2
        - Additional information about change #2
        - Additional information about change #2
    * Overview of change #3
    * Overview of change #4
        - Additional information about change #4

    ## YYYY.MM.DD - Version X.Y.Z (Previous Release)
    * Overview of change #1
        - Additional information about change #1
-->

# Change Log

## 1.0.2 - December 2022

* Added optional parameters `DevTestLabName`, `KeyVaultName`, `ResourceGroupName`, and `SubscriptionId` to [Add-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Add-AbEnvironment.md)
* Added optional parameters `DevTestLabName`, `KeyVaultName`, `ResourceGroupName`, and `SubscriptionId` to [Set-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbEnvironment.md)
* Introduced logic to validate `Run-AbAccount` has been invoked successfully with commands that require a valid connection
* Prevent builtin environments from being updated using the [Set-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbEnvironment.md) command [#12](https://github.com/automationbrew/autobrew-powershell/issues/12)
* Resolved could not load the `System.Runtime.CompilerServices.Unsafe` assembly when using the `New-AbAccessToken` command and PowerShell 5.1 [#17](https://github.com/automationbrew/autobrew-powershell/issues/17)
* Resolved configuration issue when using the module with an Azure Function app [#9](https://github.com/automationbrew/autobrew-powershell/issues/9)
* Resolved null reference exception when using the [Set-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbEnvironment.md) command [#13](https://github.com/automationbrew/autobrew-powershell/issues/13)
* Resolved obsolete initialization of the telemetry client [#19](https://github.com/automationbrew/autobrew-powershell/issues/19)
* Updated the `Azure.Identity` dependency to version `1.8.0`
* Updated the `Microsoft.ApplicationInsights` dependency to version `2.21.0`
* Updated the `Microsoft.Graph.Beta` dependency to version `4.67.0-preview`
* Updated the `Microsoft.Identity.Client` dependency to version `4.48.0`
* Updated the `Microsoft.Identity.Client.Extensions.Msal` dependency to version `1.25.0`
* Updated the `Selenium.WebDriver` dependency to version `4.7.0`
* Updated the `Selenium.WebDriver.MSEdgeDriver` dependency to version `107.0.1418.42`
* Updated the `System.Diagnostics.DiagnosticSource` dependency to version `5.0.0`
* Updated the `System.Runtime.CompilerServices.Unsafe` dependency to version `6.0.0`
* Updated the `System.Text.Encodings.Web` and `System.Text.Json` dependencies to version `7.0.0`

## 1.0.1 - June 2022

* Added the `Get-AbDelegatedAdminAccessAssignment` command to get delegated admin access assignments that exists for the given relationship.
* Added the `Get-AbDelegatedAdminRelationship` command to get the delegated admin relationships associated with the authenticated tenant.
* Added the `Get-AbDelegatedAdminTenant` command to get the delegated admin tenants associated with the authenticated tenant.
* Added the `New-AbDelegatedAdminAccessAssignment` command to create a new access assignment for the specified delegated admin relationship.
* Added the `New-AbDelegatedAdminRelationship` command to create a new delegated admin relationship.
* Added the `New-AbDelegatedAdminRelationshipRequest` command to create a new delegated admin relationship request for approval by the tenant.
* Added the `New-AbRandomPassword` command to create a random secure password.
* Added the `New-AbRiskyUser` command to create an Azure Active Directory user risk event using a Tor proxy.
* Added the `Set-AbDelegatedAdminRelationshipRequest` command to update the request for the specified delegated admin relationship.
* Resolved the `Microsoft.Extensions.Primitives` assembly conflict [#5](https://github.com/automationbrew/autobrew-powershell/issues/5)
