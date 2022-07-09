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
Update-AbConfiguration -Scope <ConfigurationScope> [-WhatIf] [-Confirm] [-DataCollection <Boolean>]
 [-DisplayBreakingChanges <Boolean>] [<CommonParameters>]
```

## DESCRIPTION

Updates the configurations for the AutomationBrew PowerShell module.

## EXAMPLES

### Example 1

```powershell
PS C:\> Update-AbConfiguration -DataCollection $false -Scope CurrentUser
```

Sets the DataCollection configuration to false for the module. This will disable telemetry from being sent for the current, and all future, sessions.

### Example 2

```powershell
PS C:\> Update-AbConfiguration -DataCollection $false -Scope Process
```

Sets the DataCollection configuration to false for the module. This will disable telemetry from being sent for only the current session.

## PARAMETERS

### -DataCollection

Controls if data collection, to help improve Automation Brew cmdlets, is enabled. When enabled, telemetry is shared with the developers to identify patterns of usage to identify common issues and help improve the experience with the module. Automation Brew does not collect any personal or private information. This configuration is enabled by default, and you must opt-out if you which to disable this functionality.

```yaml
Type: System.Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DisplayBreakingChanges

Controls if warning messages for breaking changes are displayed or suppressed. When enabled, a breaking change warning is displayed when executing cmdlets with breaking changes in a future release.

```yaml
Type: System.Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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

### -Confirm

Prompts you for confirmation before running the cmdlet.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhatIf

Shows what would happen if the cmdlet runs. The cmdlet is not run.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Boolean

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
