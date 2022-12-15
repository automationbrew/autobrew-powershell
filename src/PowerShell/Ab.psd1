#
# Module manifest for module 'Ab'
#
# Generated by: Isaiah Williams
#
# Generated on: 12/13/2022
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'Ab.psm1'

# Version number of this module.
ModuleVersion = '1.0.3'

# Supported PSEditions
CompatiblePSEditions = @(
    'Core',
    'Desktop'
)

# ID used to uniquely identify this module
GUID = 'cd7772b5-5931-43a4-b335-f286b584b7ec'

# Author of this module
Author = 'Isaiah Williams'

# Company or vendor of this module
# CompanyName = ''

# Copyright statement for this module
Copyright = '(c) Isaiah Williams. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Provides cmdlets that are helpful for automating different aspects of testing.'

# Minimum version of the PowerShell engine required by this module
PowerShellVersion = '5.1'

# Name of the PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
# ClrVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @(
    'AutoBrew.PowerShell.Authentication.dll',
    'AutoBrew.PowerShell.Common.dll'
)

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @( 'Ab.format.ps1xml' )

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @()

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = @(
    'Add-AbEnvironment',
    'Connect-AbAccount',
    'Disconnect-AbAccount',
    'Get-AbAccessToken',
    'Get-AbConfiguration',
    'Get-AbDelegatedAdminAccessAssignment',
    'Get-AbDelegatedAdminRelationship',
    'Get-AbDelegatedAdminTenant',
    'Get-AbEnvironment',
    'Invoke-AbWebDriverAction',
    'New-AbAccessToken',
    'New-AbApplicationConsent',
    'New-AbApplicationGrant',
    'New-AbBulkRefreshToken',
    'New-AbDelegatedAdminAccessAssignment',
    'New-AbDelegatedAdminRelationship',
    'New-AbDelegatedAdminRelationshipRequest'
    'New-AbRandomPassword',
    'New-AbRiskyUser',
    'Remove-AbEnvironment',
    'Resolve-AbError',
    'Set-AbEnvironment',
    'Set-AbDelegatedAdminRelationshipRequest',
    'Update-AbConfiguration'
)

# Variables to export from this module
# VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        # Tags = @()

        # A URL to the license for this module.
        LicenseUri = 'https://github.com/automationbrew/autobrew-powershell/blob/main/LICENSE'

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/automationbrew/autobrew-powershell'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        ReleaseNotes = '* Added optional parameters `DevTestLabName`, `KeyVaultName`, `ResourceGroupName`, and `SubscriptionId` to [Add-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Add-AbEnvironment.md)
* Added optional parameters `DevTestLabName`, `KeyVaultName`, `ResourceGroupName`, and `SubscriptionId` to [Set-AbEnvironment](https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbEnvironment.md)
* Introduced logic to handle a null reference exception when invoking the `Disconnect-AbAccount` command before a valid connection has been established
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
* Updated the `System.Text.Encodings.Web` and `System.Text.Json` dependencies to version `7.0.0`'

        # Prerelease string of this module
        # Prerelease = ''

        # Flag to indicate whether the module requires explicit user acceptance for install/update/save
        # RequireLicenseAcceptance = $false

        # External dependent modules of this module
        # ExternalModuleDependencies = @()

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}