---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbDelegatedAdminRelationship.md
schema: 2.0.0
---

# New-AbDelegatedAdminRelationship

## SYNOPSIS

Creates a new delegated admin relationship.

## SYNTAX

```powershell
New-AbDelegatedAdminRelationship -DisplayName <String> -Duration <String> [-Tenant <String>]
 -UnifiedRoles <String[]> [<CommonParameters>]
```

## DESCRIPTION

Creates a new delegated admin relationship.

## EXAMPLES

### Example 1

```powershell
PS C:\> New-AbDelegatedAdminRelationship -DisplayName 'Contoso - Support Tier 1' -Duration 'P2Y' -UnifiedRoles @('17315797-102d-40b4-93e0-432062caca18','29232cdf-9323-42fd-ade2-1d097af3e4de') -TenantId 'ba29897d-c9f6-4d7d-aa5a-534742c8b08e'
```

Creates a new delegated admin relationship that is valid for two years that includes the Compliance Administrator and Exchange Administrators Azure Active Directory roles.

## PARAMETERS

### -DisplayName

The display name for the delegated admin relationship.

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

### -Duration

The duration for the delegated admin relationship in the ISO8601 format.

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

The Azure Active Directory tenant identifier for the tenant.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: TenantId

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UnifiedRoles

The unified roles for the delegated admin relationship.

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

### Microsoft.Graph.DelegatedAdminRelationship

## NOTES

## RELATED LINKS
