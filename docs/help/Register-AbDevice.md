---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Register-AbDevice.md
schema: 2.0.0
---

# Register-AbDevice

## SYNOPSIS

Registers the device with a MDM service, using the Mobile Device Enrollment Protocol.

## SYNTAX

```powershell
Register-AbDevice -AccessToken <String> -UserPrincipalName <String> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION

Registers the device with a MDM service, using the Mobile Device Enrollment Protocol.

## EXAMPLES

### Example 1

```powershell
PS C:\> $token = New-AbAccessToken -ApplicationId '29d9ed98-a469-4536-ade2-f981bc1d605e' -Scopes 'urn:ms-drs:enterpriseregistration.windows.net/.default' -UseDeviceAuthentication
PS C:\>
PS C:\> Register-AbDevice -AccessToken $token.AccessToken -UserPrincipalName 'mbowen@contoso.onmicrosoft.com'
```

Registers the device with a MDM service, using the Mobile Device Enrollment Protocol.

## PARAMETERS

### -AccessToken

The access token for the user registering the device.

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

### -UserPrincipalName

The user principal name (UPN) for the user registering the device.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: UPN

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

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
