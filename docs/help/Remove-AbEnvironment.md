---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Remove-AbEnvironment.md
schema: 2.0.0
---

# Remove-AbEnvironment

## SYNOPSIS

Removes the metadata for connecting to given cloud.

## SYNTAX

```powershell
Remove-AbEnvironment -Name <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION

Removes the metadata for connecting to given cloud.

## EXAMPLES

### Example 1

```powershell
PS C:\> Remove-AbEnvironment -Name MyEnvironment
```

Removes the metadata for connecting to the cloud represented by the environment named MyEnvironment.

## PARAMETERS

### -Name

The name for the environment.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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

Shows what would happen if the cmdlet runs.
The cmdlet is not run.

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

### System.String

## OUTPUTS

### AutoBrew.PowerShell.Models.Authentication.ModuleEnvironment

## NOTES

## RELATED LINKS
