---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Resolve-AbError.md
schema: 2.0.0
---

# Resolve-AbError

## SYNOPSIS

Displays detailed information about PowerShell errors, with extended details for cmdlet errors.

## SYNTAX

### AnyErrorParameterSet

```powershell
Resolve-AbError [[-Error] <ErrorRecord[]>] [<CommonParameters>]
```

### LastErrorParameterSet

```powershell
Resolve-AbError [-Last] [<CommonParameters>]
```

## DESCRIPTION

Displays detailed information about PowerShell errors, with extended details for cmdlet errors.

## EXAMPLES

### Example 1

```powershell
PS C:\> Resolve-AbError -Last
```

Get the details for the last error.

### Example 2

```powershell
PS C:\> Resolve-AbError
```

Get details for all errors that have occurred in the current session.

## PARAMETERS

### -Error

The error records to resolve.

```yaml
Type: System.Management.Automation.ErrorRecord[]
Parameter Sets: AnyErrorParameterSet
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Last

A flag indicating whether only detailed information for the last error should be shown.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: LastErrorParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Management.Automation.ErrorRecord[]

## OUTPUTS

### AutoBrew.PowerShell.Models.Errors.ModuleErrorRecord

### AutoBrew.PowerShell.Models.Errors.ModuleExceptionRecord

## NOTES

## RELATED LINKS
