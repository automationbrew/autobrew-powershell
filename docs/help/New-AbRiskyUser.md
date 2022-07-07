---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbRiskyUser.md
schema: 2.0.0
---

# New-AbRiskyUser

## SYNOPSIS

Generates a risky user event in Azure Active Directory by performing an authentication request that is proxied using the Tor Project.

## SYNTAX

### CredentialParameterSet

```powershell
New-AbRiskyUser -ApplicationId <String> -Credential <PSCredential> -Tenant <String> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

### UsernamePasswordParameterSet

```powershell
New-AbRiskyUser -ApplicationId <String> -Password <SecureString> -Tenant <String> -UserPrincipalName <String>
 [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION

Generates a risky user event in Azure Active Directory by performing an authentication request that is proxied using the Tor Project.

## EXAMPLES

### Example 1

```powershell
PS C:\> $password = New-AbRandomPassword -Length 24 -NumberOfNonAlphanumericCharacters 6
PS C:\> $user = Get-AzureADUser -ObjectId 'xxxx-xxxx-xxxx-xxxx'
PS C:\>
PS C:\> Set-AzureADUserPassword -ObjectId $user.ObjectId -Password $password -ForceChangePasswordNextLogin $false
PS C:\>
PS C:\> $credential = New-Object System.Management.Automation.PSCredential ($user.UserPrincipalName, $password)
PS C:\> New-AbRiskyUser -ApplicationId 'yyyy-yyyy-yyyy-yyyy' -Credential $credential -Tenant 'zzzz-zzzz-zzzz-zzzz'
```

Utilizes the Azure Active Directory PowerShell module to reset the password for a specific user. Then Generates a risky user event in Azure Active Directory by performing an authentication request that is proxied using the Tor Project.

## PARAMETERS

### -ApplicationId

The identifier of the application that will be used for authentication.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: ClientId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential

The credentials for the user where the risk event should be triggered.

```yaml
Type: System.Management.Automation.PSCredential
Parameter Sets: CredentialParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Password

The password for the user where the risk event should be triggered.

```yaml
Type: System.Security.SecureString
Parameter Sets: UsernamePasswordParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tenant

The Azure Active Directory tenant identifier for the tenant.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: Domain, TenantId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UserPrincipalName

The user principal name (UPN) for the user where the risk event should be triggered.

```yaml
Type: System.String
Parameter Sets: UsernamePasswordParameterSet
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
