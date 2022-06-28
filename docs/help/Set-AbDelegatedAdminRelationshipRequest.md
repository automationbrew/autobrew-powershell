---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Set-AbDelegatedAdminRelationshipRequest.md
schema: 2.0.0
---

# Set-AbDelegatedAdminRelationshipRequest

## SYNOPSIS

Updates the request for the specified delegated admin relationship.

## SYNTAX

```powershell
Set-AbDelegatedAdminRelationshipRequest -Action <DelegatedAdminRelationshipRequestAction>
 -RelationshipId <String> [-RequestId <String>] [<CommonParameters>]
```

## DESCRIPTION

 Updates the request for the specified delegated admin relationship.

## EXAMPLES

### Example 1

```powershell
PS C:\> Set-AbDelegatedAdminRelationshipRequest -Action Approve -RelationshipId '804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba'
```

Approves the specified delegated admin relationship, which can only be done by the customer.

### Example 2

```powershell
PS C:\> Set-AbDelegatedAdminRelationshipRequest -Action Terminate -RelationshipId '804ae4a3-eb6a-42af-9c18-270663291a86-55dd7c23-5287-41e4-b760-6024da674dba'
```

Terminates the specified delegated admin relationship.

## PARAMETERS

### -Action

The action for the delegated admin relationship request.

```yaml
Type: Microsoft.Graph.DelegatedAdminRelationshipRequestAction
Parameter Sets: (All)
Aliases:
Accepted values: Approve, LockForApproval, Terminate

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

### -RequestId

The identifier for the delegated admin relationship request.

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

### Microsoft.Graph.DelegatedAdminRelationshipRequestObject

## NOTES

## RELATED LINKS
