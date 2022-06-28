---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Get-AbDelegatedAdminAccessAssignment.md
schema: 2.0.0
---

# Get-AbDelegatedAdminAccessAssignment

## SYNOPSIS

Gets delegated admin access assignments that exists for the given relationship.

## SYNTAX

```powershell
Get-AbDelegatedAdminAccessAssignment [-AccessAssignmentId <String>] -RelationshipId <String>
 [<CommonParameters>]
```

## DESCRIPTION

Gets delegated admin access assignments that exists for the given relationship.

## EXAMPLES

### Example 1

```powershell
PS C:\> Get-AbDelegatedAdminAccessAssignment -RelationshipId '804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba'
```

Gets the access assignments for the delegated admin relationship with 804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba for the identifier.

### Example 2

```powershell
PS C:\> Get-AbDelegatedAdminAccessAssignment -AccessAssignmentId '257e7c64-04f5-439b-80d3-10248a249c45' -RelationshipId '804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba'
```

Gets the access assignment with the identifier 257e7c64-04f5-439b-80d3-10248a249c45 for the delegated admin relationship with 804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba for the identifier.

## PARAMETERS

### -AccessAssignmentId

The identifier for the access assignment.

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

### Microsoft.Graph.DelegatedAdminAccessAssignment

## NOTES

## RELATED LINKS
