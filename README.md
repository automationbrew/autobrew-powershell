# AutomationBrew PowerShell

[![.NET workflow](https://github.com/automationbrew/autobrew-powershell/actions/workflows/dotnet.yml/badge.svg)](https://github.com/automationbrew/autobrew-powershell/actions/workflows/dotnet.yml) [![CodeQL workflow](https://github.com/automationbrew/autobrew-powershell/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/automationbrew/autobrew-powershell/actions/workflows/codeql-analysis.yml) [![DevSkim workflow](https://github.com/automationbrew/autobrew-powershell/actions/workflows/devskim.yml/badge.svg)](https://github.com/automationbrew/autobrew-powershell/actions/workflows/devskim.yml) [![PSScriptAnalyzer workflow](https://github.com/automationbrew/autobrew-powershell/actions/workflows/powershell.yml/badge.svg)](https://github.com/automationbrew/autobrew-powershell/actions/workflows/powershell.yml)

[![Ab](https://img.shields.io/powershellgallery/v/Ab.svg?style=flat-square&label=Ab)](https://www.powershellgallery.com/packages/Ab/) [![GitHub issues](https://img.shields.io/github/issues/automationbrew/autobrew-powershell.svg)](https://github.com/automationbrew/autobrew-powershell/issues/) [![GitHub pull-requests](https://img.shields.io/github/issues-pr/automationbrew/autobrew-powershell.svg)](https://gitHub.com/automationbrew/autobrew-powershell/pull/)

AutomationBrew PowerShell is a module that provides cmdlets that are helpful for automating different aspects of testing.

## Requirements

AutomationBrew PowerShell works with PowerShell 5.1 or higher on Windows, or PowerShell Core 6.x and later on all platforms. If you aren't sure if you have PowerShell, or are on macOS or Linux, [install the latest version of PowerShell Core](https://docs.microsoft.com/powershell/scripting/install/installing-powershell#powershell-core).

To check your PowerShell version, run the command:

```powershell
$PSVersionTable.PSVersion
```

To run the AutomationBrew PowerShell module in PowerShell 5.1 on Windows:

1. Update to [Windows PowerShell 5.1](https://docs.microsoft.com/powershell/scripting/install/installing-windows-powershell#upgrading-existing-windows-powershell) if needed. If you're on Windows 10, you already
  have PowerShell 5.1 installed.
2. Install [.NET Framework 4.7.2 or later](https://docs.microsoft.com/dotnet/framework/install).

There are no additional requirements when using PowerShell Core.

## Install the AutomationBrew PowerShell module

The recommended install method is to only install for the active user:

```powershell
Install-Module -Name Ab -AllowClobber -Scope CurrentUser
```

If you want to install for all users on a system, this requires administrator privileges. From an elevated PowerShell session either
run as administrator or with the `sudo` command on macOS or Linux:

```powershell
Install-Module -Name Ab -AllowClobber -Scope AllUsers
```

By default, the PowerShell gallery isn't configured as a trusted repository for PowerShellGet. The first time you use the PSGallery you see the following prompt:

```output
Untrusted repository

You are installing the modules from an untrusted repository. If you trust this repository, change
its InstallationPolicy value by running the Set-PSRepository cmdlet.

Are you sure you want to install the modules from 'PSGallery'?
[Y] Yes  [A] Yes to All  [N] No  [L] No to All  [S] Suspend  [?] Help (default is "N"):
```

Answer `Yes` or `Yes to All` to continue with the installation.

### Discovering cmdlets

Use the `Get-Command` cmdlet to discover cmdlets within a specific module, or cmdlets that follow a specific search pattern:

```powershell
# List all cmdlets in the AutomationBrew PowerShell module
Get-Command -Module Ab

# List all cmdlets that contain Token
Get-Command -Name '*Token*'

# List all cmdlets that contain Token in the AutomationBrew PowerShell module
Get-Command -Module Ab -Name '*Token*'
```

### Cmdlet help and examples

To view the help content for a cmdlet, use the `Get-Help` cmdlet:

```powershell
# View the basic help content for New-AbAccessToken
Get-Help -Name New-AbAccessToken

# View the examples for New-AbAccessToken
Get-Help -Name New-AbAccessToken -Examples

# View the full help content for New-AbAccessToken
Get-Help -Name New-AbAccessToken -Full
```

## Telemetry

AutomationBrew PowerShell collects telemetry by default. The collection of this data is aggregated to identify patterns of usage to identify common issues and improve the experience with the module. No personal or private information is collected. While having this insight is helpful, it is understood that not everyone wants to send usage data. You can disable data collection with the [`Update-AbConfiguration`](docs/help/Update-AbConfiguration.md) cmdlet.
