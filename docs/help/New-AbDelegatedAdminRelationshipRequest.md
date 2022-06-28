---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/New-AbDelegatedAdminRelationshipRequest.md
schema: 2.0.0
---

# New-AbDelegatedAdminRelationshipRequest

## SYNOPSIS

Create a new delegated admin relationship request for approval by the tenant.

## SYNTAX

```powershell
New-AbDelegatedAdminRelationshipRequest -RelationshipId <String> [<CommonParameters>]
```

## DESCRIPTION

Create a new delegated admin relationship request for approval by the tenant.

## EXAMPLES

### Example 1

```powershell
PS C:\> New-AbDelegatedAdminRelationshipRequest -RelationshipId '804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba'
```

Create a new delegated admin relationship request for approval by the tenant.

## PARAMETERS

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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.Graph.DelegatedAdminRelationshipRequestObject

## NOTES

## RELATED LINKS
