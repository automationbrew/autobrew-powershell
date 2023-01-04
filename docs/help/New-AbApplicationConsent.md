---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbApplicationConsent.md
schema: 2.0.0
---

# New-AbApplicationConsent

## SYNOPSIS

Creates a new consent for an Azure Active Directory application in the specified Azure Active Directory tenant.

## SYNTAX

```powershell
New-AbApplicationConsent -ApplicationGrants <ApplicationGrant[]> -ApplicationId <String> -TenantId <String>
 [<CommonParameters>]
```

## DESCRIPTION

Creates a new consent for an Azure Active Directory application in the specified Azure Active Directory tenant. Prior to utilizing this command a valid connection must be established utilizing the Azure Active Directory application where consent should be granted.

## EXAMPLES

### Example 1

```powershell
PS C:\> $applicationId = 'xxxx-xxxx-xxxx-xxxx'
PS C:\>
PS C:\> Connect-AbAccount -ApplicationId $applicationId -UseDeviceAuthentication
PS C:\>
PS C:\> $grants = @()
PS C:\> $grants += New-AbApplicationGrant -EnterpriseApplicationId '00000003-0000-0000-c000-000000000000' -Scope 'DeviceManagementConfiguration.Read.All,DeviceManagementManagedDevices.Read.All'
PS C:\> $grants += New-AbApplicationGrant -EnterpriseApplicationId '797f4846-ba00-4fd7-ba43-dac1f8f63013' -Scope 'user_impersonation'
PS C:\>
PS C:\> New-AbApplicationConsent -ApplicationGrants $grants -ApplicationId $applicationId -TenantId 'yyyy-yyyy-yyyy-yyyy'
```

Creates a new consent for an Azure Active Directory application with grants for Microsoft Graph and Azure Service Management.

## PARAMETERS

### -ApplicationGrants

The application grants that are associated with the application consent.

```yaml
Type: AutoBrew.PowerShell.Models.Applications.ApplicationGrant[]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ApplicationId

The identifier of the Azure Active Directory application.

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

### -TenantId

The identifier for the customer tenant.

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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
