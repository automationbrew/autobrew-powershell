---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Unregister-AbDevice.md
schema: 2.0.0
---

# Unregister-AbDevice

## SYNOPSIS

Provides the ability to unregister the device with a management service.

## SYNTAX

```powershell
Unregister-AbDevice -EnrollmentId <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION

Provides the ability to unregister the device with a management service.

## EXAMPLES

### Example 1

```powershell
PS C:\> Unregister-AbDevice -EnrollmentId 'd244cff2-9dae-406d-90df-ad21dbff7118'
```

Performs the operation required to unregister the device from a management service associated with the specified enrollment identifier.

## PARAMETERS

### -EnrollmentId

The identifier of the enrollment that should be unregistered.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

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

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
