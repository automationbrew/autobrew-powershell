---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Update-AbConfiguration.md
schema: 2.0.0
---

# Update-AbConfiguration

## SYNOPSIS

Updates the configurations for the AutomationBrew PowerShell module.

## SYNTAX

```powershell
Update-AbConfiguration [-EnableTelemetry <Boolean>] -Scope <ConfigurationScope> [<CommonParameters>]
```

## DESCRIPTION

Updates the configurations for the AutomationBrew PowerShell module.

## EXAMPLES

### Example 1

```powershell
PS C:\> Update-AbConfiguration -EnableTelemetry $false -Scope CurrentUser 
```

Sets the EnableTelemetry configuration to false for the module. This will disable telemetry from being sent for the current, and all future, sessions.

### Example 2

```powershell
PS C:\> Update-AbConfiguration -EnableTelemetry $false -Scope Process 
```

Sets the EnableTelemetry configuration to false for the module. This will disable telemetry from being sent for only the current session.

## PARAMETERS

### -EnableTelemetry

A flag that indicates whether telemetry for the module is enabled

```yaml
Type: System.Nullable`1[System.Boolean]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope

The scope for the configuration

```yaml
Type: AutoBrew.PowerShell.Models.Configuration.ConfigurationScope
Parameter Sets: (All)
Aliases:
Accepted values: CurrentUser, Process

Required: True
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

### System.Object

## NOTES

## RELATED LINKS
