---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbRandomPassword.md
schema: 2.0.0
---

# New-AbRandomPassword

## SYNOPSIS

Generate a secure random password.

## SYNTAX

```powershell
New-AbRandomPassword [-Length <Int32>] [-NumberOfNonAlphanumericCharacters <Int32>] [<CommonParameters>]
```

## DESCRIPTION

Generate a secure random password.

## EXAMPLES

### Example 1

```powershell
PS C:\> New-AbRandomPassword -Length 24 -NumberOfNonAlphanumericCharacters 6
```

Generate a secure random password.

## PARAMETERS

### -Length

The number of characters in the generated password. The length must be between 8 and 128 characters.

```yaml
Type: System.Nullable`1[System.Int32]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NumberOfNonAlphanumericCharacters

The minimum number of non-alphanumeric characters (such as @, #, !, %, &, and so on) in the generated password. The number must be 2 or more.

```yaml
Type: System.Nullable`1[System.Int32]
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

### System.Security.SecureString

## NOTES

## RELATED LINKS
