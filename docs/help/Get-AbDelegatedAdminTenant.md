---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Get-AbDelegatedAdminTenant.md
schema: 2.0.0
---

# Get-AbDelegatedAdminTenant

## SYNOPSIS

Gets the delegated admin tenants associated with the authenticated tenant.

## SYNTAX

```powershell
Get-AbDelegatedAdminTenant [-Tenant <String>] [<CommonParameters>]
```

## DESCRIPTION

Gets the delegated admin tenants associated with the authenticated tenant.

## EXAMPLES

### Example 1

```powershell
PS C:\> Get-AbDelegatedAdminTenant
```

Gets the delegated admin tenants associated with the authenticated tenant.

### Example 2

```powershell
PS C:\> Get-AbDelegatedAdminTenant -Tenant '993a7446-aaf8-4307-a84b-9075e23c5e63'
```

Gets the specified delegated admin tenant associated with the authenticated tenant.

## PARAMETERS

### -Tenant

The Azure Active Directory tenant identifier for the tenant.

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

### Microsoft.Graph.DelegatedAdminCustomer

## NOTES

## RELATED LINKS
