---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Register-AbDevice.md
schema: 2.0.0
---

# Register-AbDevice

## SYNOPSIS

Registers the device for management.

## SYNTAX

### AccessTokenParameterSet (Default)

```powershell
Register-AbDevice -AccessToken <String> -ManagementUri <String> -UserPrincipalName <String> [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

### CredentialsParameterSetName

```powershell
Register-AbDevice -Credentials <PSCredential> [-Environment <String>] -ManagementUri <String> -Tenant <String>
 -UserPrincipalName <String> [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION

Registers the device with a MDM service, using the Mobile Device Enrollment Protocol.

## EXAMPLES

### Example 1

```powershell
PS C:\> $token = New-AbAccessToken -ApplicationId 'de50c81f-5f80-4771-b66b-cebd28ccdfc1' -Scopes 'https://enrollment.manage.microsoft.com//.default' -UseDeviceAuthentication
PS C:\>
PS C:\> Register-AbDevice -AccessToken $token.AccessToken -ManagementUri 'manage.microsoft.com' -UserPrincipalName 'mbowen@contoso.onmicrosoft.com'
```

Registers the device for management using an access token to validate the user.

### Example 2

```powershell
PS C:\> $credentials = Get-Credentials
PS C:\>
PS C:\> Register-AbDevice -Credentials $credentials -Environment AzureCloud -ManagementUri 'manage.microsoft.com' -Tenant 'deeccd13-8de7-4afe-8e00-b5606ce48120' -UserPrincipalName 'mbowen@contoso.onmicrosoft.com'
```

Registers the device for management using credentials to validate the user.

## PARAMETERS

### -AccessToken

The access token for the user registering the device.

```yaml
Type: System.String
Parameter Sets: AccessTokenParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credentials

The credentials to be used by the management service to validate the user.

```yaml
Type: System.Management.Automation.PSCredential
Parameter Sets: CredentialsParameterSetName
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Environment

The name of the environment to be used for authentication.

```yaml
Type: System.String
Parameter Sets: CredentialsParameterSetName
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ManagementUri

The address for the management service.

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

### -Tenant

The identifier for the tenant to be used for authentication.

```yaml
Type: System.String
Parameter Sets: CredentialsParameterSetName
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

### System.Void

## NOTES

## RELATED LINKS
