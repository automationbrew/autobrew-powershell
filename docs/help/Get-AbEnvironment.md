---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Get-AbConfiguration.md
schema: 2.0.0
---

# Get-AbEnvironment

## SYNOPSIS

Gets the metadata for an environment.

## SYNTAX

```powershell
Get-AbEnvironment [-Name <String>] [<CommonParameters>]
```

## DESCRIPTION

Gets the metadata for an environment.

## EXAMPLES

### Example 1

```powershell
PS C:\> Get-AbEnvironment
```

Gets the metadata for all environments.

### Example 2

```powershell
PS C:\> Get-AbEnvironment -Name AzureCloud
```

Gets the metadata for the environment with the name AzureCloud.

## PARAMETERS

### -Name

The name for the environment.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### AutoBrew.PowerShell.Models.Authentication.ModuleEnvironment

## NOTES

## RELATED LINKS
