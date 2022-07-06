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

## Upcoming Release

* Prevent builtin environments from being updated using the [Set-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbEnvironment.md) command [#12](https://github.com/automationbrew/autobrew-powershell/issues/12)
* Resolved configuration issue when using the module with an Azure Function app [#9](https://github.com/automationbrew/autobrew-powershell/issues/9)
* Resolved null reference exception when using the [Set-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbEnvironment.md) command [#13](https://github.com/automationbrew/autobrew-powershell/issues/13)
* Updated the `Selenium.WebDriver.MSEdgeDriver` package to version `103.0.1264.37`

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
