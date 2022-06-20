---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Connect-AbAccount.md
schema: 2.0.0
---

# Connect-AbAccount

## SYNOPSIS

Connect to the cloud with an authenticated account.

## SYNTAX

### DefaultParameterSet (Default)

```powershell
Connect-AbAccount [-ApplicationId <String>] [-Environment <String>] [-Scopes <String[]>] [-Tenant <String>]
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

### RefreshTokenParameterSet

```powershell
Connect-AbAccount [-ApplicationId <String>] [-Environment <String>] -RefreshToken <SecureString>
 [-Scopes <String[]>] [-Tenant <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### AuthorizationCodeParameterSet

```powershell
Connect-AbAccount [-ApplicationId <String>] [-Environment <String>] [-Scopes <String[]>] [-Tenant <String>]
 [-UseAuthorizationCode] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### DeviceCodeParameterSet

```powershell
Connect-AbAccount [-ApplicationId <String>] [-Environment <String>] [-Scopes <String[]>] [-Tenant <String>]
 [-UseDeviceAuthentication] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION

Connect to the cloud with an authenticated account.

## EXAMPLES

### Example 1

```powershell
PS C:\> Connect-AbAccount
```

Establishes a connection to the cloud using an authenticated account.

### Example 2

```powershell
PS C:\> $refreshToken = '<refreshToken>'
PS C:\> Connect-AbAccount -ApplicationId 'xxxx-xxxx-xxxx-xxxx' -RefreshToken $refreshToken
```

Connects to the cloud using a refresh token that was generated using a [native application](https://docs.microsoft.com/azure/active-directory/develop/native-app).

## PARAMETERS

### -ApplicationId

The identifier of the application to be used for authentication.

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

### -Environment

The name of the environment to be used for authentication.

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

### -RefreshToken

The refresh token to be used for authentication.

```yaml
Type: System.Security.SecureString
Parameter Sets: RefreshTokenParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scopes

The scopes to be used for authentication.

```yaml
Type: System.String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tenant

The identifier for the tenant to be used for authentication.

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

### -UseAuthorizationCode

The flag that indicates the authorization code flow should be used for authentication.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: AuthorizationCodeParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UseDeviceAuthentication

The flag that indicates the device code flow should be used for authentication.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: DeviceCodeParameterSet
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

### AutoBrew.PowerShell.Models.ModuleContext

## NOTES

## RELATED LINKS
