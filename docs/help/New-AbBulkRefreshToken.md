---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbBulkRefreshToken.md
schema: 2.0.0
---

# New-AbBulkRefreshToken

## SYNOPSIS

Acquires a bulk refresh token from Azure Active Directory that will be used to perform the cloud domain join or workspace join operation.

## SYNTAX

```powershell
New-AbBulkRefreshToken [-Environment <String>] [<CommonParameters>]
```

## DESCRIPTION

Acquires a bulk refresh token from Azure Active Directory that will be used to perform the cloud domain join or workspace join operation.

## EXAMPLES

### Example 1

```powershell
PS C:\> New-AbBulkRefreshToken
```

Acquires a bulk refresh token from Azure Active Directory that will be used to perform the cloud domain join or workspace join operation.

## PARAMETERS

### -Environment

The environment that will provide metadata used to acquire the bulk refresh token.

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

### AutoBrew.PowerShell.Models.Authentication.BulkRefreshToken

## NOTES

## RELATED LINKS
