---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbAccessToken.md
schema: 2.0.0
---

# New-AbAccessToken

## SYNOPSIS

Acquires an access token from Azure Active Directory.

## SYNTAX

### DefaultParameterSet (Default)

```powershell
New-AbAccessToken [-ApplicationId <String>] [-Environment <String>] -Scopes <String[]> [-Tenant <String>]
 [<CommonParameters>]
```

### RefreshTokenParameterSet

```powershell
New-AbAccessToken [-ApplicationId <String>] [-Environment <String>] -RefreshToken <SecureString>
 -Scopes <String[]> [-Tenant <String>] [<CommonParameters>]
```

### AuthorizationCodeParameterSet

```powershell
New-AbAccessToken [-ApplicationId <String>] [-Environment <String>] -Scopes <String[]> [-Tenant <String>]
 [-UseAuthorizationCode] [<CommonParameters>]
```

### DeviceCodeParameterSet

```powershell
New-AbAccessToken [-ApplicationId <String>] [-Environment <String>] -Scopes <String[]> [-Tenant <String>]
 [-UseDeviceAuthentication] [<CommonParameters>]
```

## DESCRIPTION

Acquires an access token from Azure Active Directory.

## EXAMPLES

### Example 1

```powershell
PS C:\> $credential = Get-Credential
PS C:\> New-AbAccessToken -ApplicationId 'xxxx-xxxx-xxxx-xxxx' -Scopes 'https://graph.microsoft.com/.default' -Tenant 'yyyy-yyyy-yyyy-yyyy' -UseAuthorizationCode
```

Requests a new access token from Azure Directory using the authorization code flow. By design the redirect URI will be generated automatically. This generation process will attempt to find a port between 8400 and 8999 that is not in use. Once an available port has been found, the redirect URL value will be constructed (e.g. <http://localhost:8400>). Which means, the redirect URI for the Azure Active Directory application must be configured accordingly.

### Example 2

```powershell
$refreshToken = Get-AzKeyVaultSecret -SecretName 'secretName' -VaultName 'vaultName'
New-AbAccessToken -ApplicationId 'xxxx-xxxx-xxxx-xxxx' -RefreshToken $refreshToken.SecretValue -Scopes 'https://graph.microsoft.com/.default' -Tenant 'yyyy-yyyy-yyyy-yyyy'
```

Obtains a refresh token from Azure KeyVault, and requests a new access from Azure Directory using that refresh token.

## PARAMETERS

### -ApplicationId

The identifier for the application to be used for authentication.

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

The environment to be used for authentication.

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

Required: True
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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### AutoBrew.PowerShell.Models.Authentication.ModuleAuthenticationResult

## NOTES

## RELATED LINKS
