---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Get-AbAccessToken.md
schema: 2.0.0
---

# Get-AbAccessToken

## SYNOPSIS

Gets an access token for the specified scopes.

## SYNTAX

```powershell
Get-AbAccessToken -Scopes <String[]> [<CommonParameters>]
```

## DESCRIPTION

Gets an access token for the specified scopes.

## EXAMPLES

### Example 1

```powershell
PS C:\> Get-AbAccessToken -Scopes 'https://graph.microsoft.com/.default'
```

Get an access token for the default scope of Microsoft Graph.

## PARAMETERS

### -Scopes

The scopes to be used for authentication.

```yaml
Type: System.String[]
Parameter Sets: (All)
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

### None

## OUTPUTS

### AutoBrew.PowerShell.Models.Authentication.ModuleAuthenticationResult

## NOTES

## RELATED LINKS
