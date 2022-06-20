---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Add-AbEnvironment.md
schema: 2.0.0
---

# Add-AbEnvironment

## SYNOPSIS

Adds metadata for connecting to a given cloud.

## SYNTAX

```powershell
Add-AbEnvironment -ActiveDirectoryAuthority <String> -MicrosoftGraphEndpoint <String> -Name <String> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION

Adds metadata for connecting to a given cloud.

## EXAMPLES

### Example 1

```powershell
PS C:\> Add-AbEnvironment -ActiveDirectoryAuthority 'https://login.microsoftonline.com/' -MicrosoftGraphEndpoint 'https://graph.microsoft.com' -Name 'MyEnvironment'
```

Adds metadata for connecting to a given cloud.

## PARAMETERS

### -ActiveDirectoryAuthority

The Active Directory authority for the environment.

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

### -MicrosoftGraphEndpoint

The Microsoft Graph endpoint for the environment.

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

### -Name

The name for the environment.

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

### System.Object

## NOTES

## RELATED LINKS
