---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Get-AbDelegatedAdminRelationship.md
schema: 2.0.0
---

# Get-AbDelegatedAdminRelationship

## SYNOPSIS

Gets the delegated admin relationships associated with the authenticated tenant.

## SYNTAX

```powershell
Get-AbDelegatedAdminRelationship [-RelationshipId <String>] [<CommonParameters>]
```

## DESCRIPTION

Gets the delegated admin relationships associated with the authenticated tenant.

## EXAMPLES

### Example 1

```powershell
PS C:\> Get-AbDelegatedAdminRelationship
```

Gets the delegated admin relationships associated with the authenticated tenant.

### Example 2

```powershell
PS C:\> Get-AbDelegatedAdminRelationship -RelationshipId '2e994e88-bd27-42f0-afd1-46ef84689f66-993a7446-aaf8-4307-a84b-9075e23c5e63'
```

Gets the specified delegated admin relationships associated with the authenticated tenant.

## PARAMETERS

### -RelationshipId

The identifier for the delegated admin relationship.

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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.Graph.DelegatedAdminRelationship

## NOTES

## RELATED LINKS
