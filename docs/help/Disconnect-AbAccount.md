---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Disconnect-AbAccount.md
schema: 2.0.0
---

# Disconnect-AbAccount

## SYNOPSIS

Disconnects a connected account and removes all credentials and contexts associated with that account.

## SYNTAX

```powershell
Disconnect-AbAccount [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION

Disconnects a connected account and removes all credentials and contexts associated with that account.

## EXAMPLES

### Example 1

```powershell
PS C:\> Disconnect-AbAccount
```

Disconnects a connected account and removes all credentials and contexts associated with that account.

## PARAMETERS

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

### AutoBrew.PowerShell.Models.Authentication.ModuleAccount

## NOTES

## RELATED LINKS
