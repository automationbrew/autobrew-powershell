---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbDelegatedAdminAccessAssignment.md
schema: 2.0.0
---

# New-AbDelegatedAdminAccessAssignment

## SYNOPSIS

Creates a new access assignment for the specified delegated admin relationship.

## SYNTAX

```powershell
New-AbDelegatedAdminAccessAssignment -AccessContainerId <String> -RelationshipId <String>
 -UnifiedRoles <String[]> [<CommonParameters>]
```

## DESCRIPTION

Creates a new access assignment for the specified delegated admin relationship.

## EXAMPLES

### Example 1

```powershell
PS C:\> New-AbDelegatedAdminAccessAssignment -AccessContainerId 'cb09b3b7-5cb1-4357-aa07-5fcb5fd8952d' -RelationshipId '804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba' -UnifiedRoles @('17315797-102d-40b4-93e0-432062caca18','29232cdf-9323-42fd-ade2-1d097af3e4de')
```

Creates the access assignment for the security group with the identifier of cb09b3b7-5cb1-4357-aa07-5fcb5fd8952d with the delegated admin relationship 804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba, where the Compliance Administrator and Exchange Administrators Azure Active Directory roles are included.

## PARAMETERS

### -AccessContainerId

The identifier for the access container.

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

### -RelationshipId

The identifier for the delegated admin relationship.

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

### -UnifiedRoles

The unified roles to be included in the access assignment.

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

### Microsoft.Graph.DelegatedAdminAccessAssignment

## NOTES

## RELATED LINKS
