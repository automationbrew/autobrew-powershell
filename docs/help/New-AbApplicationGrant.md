---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbApplicationGrant.md
schema: 2.0.0
---

# New-AbApplicationGrant

## SYNOPSIS

Creates a new application grant object that is used as part of the request to consent for an Azure Active Directory to access resources in an Azure Active Directory tenant.

## SYNTAX

```
New-AbApplicationGrant -EnterpriseApplicationId <String> -Scope <String> [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION

Creates a new application grant object that is used as part of the request to consent for an Azure Active Directory to access resources in an Azure Active Directory tenant.

## EXAMPLES

### Example 1

```powershell
PS C:\> New-AbApplicationGrant -EnterpriseApplicationId '00000003-0000-0000-c000-000000000000' -Scope 'DeviceManagementConfiguration.Read.All'
```

Creates a new application grant object for Microsoft Graph that includes the DeviceManagementConfiguration.Read.All scope.

### Example 2

```powershell
PS C:\> New-AbApplicationGrant -EnterpriseApplicationId '797f4846-ba00-4fd7-ba43-dac1f8f63013' -Scope 'user_impersonation'
```

Creates a new application grant object for Azure Service Management that includes the user_impersonation scope.

## PARAMETERS

### -EnterpriseApplicationId

The identifier of the enterprise application associated with the application grant.

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

### -Scope

A comma delimited list of scopes that are associated with the application grant.

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

### AutoBrew.PowerShell.Models.Applications.ApplicationGrant

## NOTES

## RELATED LINKS
