---
external help file: AutoBrew.PowerShell.dll-Help.xml
Module Name: Ab
online version: https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/Invoke-AbWebDriverAction.md
schema: 2.0.0
---

# Invoke-AbWebDriverAction

## SYNOPSIS

Automates a series of interactions with a web browser based on the defined template.

## SYNTAX

### TemplateParameterSet (Default)

```powershell
Invoke-AbWebDriverAction [-Credential <PSCredential>] [-Parameters <Hashtable>] -NavigationUrl <String>
 -Template <String> [<CommonParameters>]
```

### TemplateFileParameterSet

```powershell
Invoke-AbWebDriverAction [-Credential <PSCredential>] [-Parameters <Hashtable>] -NavigationUrl <String>
 -TemplateFile <String> [<CommonParameters>]
```

## DESCRIPTION

Automates a series of interactions with a web browser based on the defined template.

## EXAMPLES

### Example 1

```powershell
PS C:\> $acceptResellerRelationTemplate = @"
[
  {
    "ActionType": "authenticate",
    "Sequence": 0,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "click",
    "Sequence": 1,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": ".ms-Checkbox-text > span",
    "WebElementLocatorType": "styleSheet"
  },
  {
    "ActionType": "click",
    "Sequence": 2,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//button[@data-automation-id='PartnerInvitationDetails,Mca_Authorize_ButtonBtn']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "click",
    "Sequence": 3,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//button[@data-automation-id='PartnerInvitationDetails,Partner_Authorize_ButtonBtn']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 4,
    "WaitThresholdInSeconds": 15
  }
]
"@
PS C:\>
PS C:\> $relationshipId = 'f3de4c36-bf5d-4c43-960f-799f410c6916'
PS C:\>
PS C:\>PS C:\> Invoke-AbWebDriverAction -Credential $credential -NavigationUrl "https://admin.microsoft.com/Adminportal/Home?invType=ResellerRelationship&partnerId=$relationshipId&msppId=0&DAP=false#/BillingAccounts/partner-invitation" -Template $acceptResellerRelationTemplate
```

Automates the interactions with a web browser to accept a reseller relationship.

### Example 2

```powershell
PS C:\> $activateMicrosoftDefenderTemplate = @"
[
  {
    "ActionType": "authenticate",
    "Sequence": 0,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 1,
    "WaitThresholdInSeconds": 300
  }
]
"@
PS C:\>
PS C:\>PS C:\> Invoke-AbWebDriverAction -Credential $credential -NavigationUrl "https://security.microsoft.com/preferences2/integration" -Template $activateMicrosoftDefenderTemplate
```

Automates the interactions with a web browser to activate Microsoft Defender for Endpoint.

### Example 3

```powershell
PS C:\> $approveAdminRelationshipTemplate = @"
[
  {
    "ActionType": "authenticate",
    "Sequence": 0,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "click",
    "Sequence": 1,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//button[@data-automation-id='GdapInvitation,PrimaryButtonBtn']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "click",
    "Sequence": 2,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//span/button/span/span/span",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 3,
    "WaitThresholdInSeconds": 15
  }
]
"@
PS C:\>
PS C:\> $relationshipId = 'c19db779-978c-4b8d-a160-1c150bd3628b-52155959-1252-4bdc-85af-691a6e1c83ed'
PS C:\>
PS C:\> Invoke-AbWebDriverAction -Credential $credential -NavigationUrl "https://admin.microsoft.com/AdminPortal/Home#/partners/invitation/granularAdminRelationships/$relationshipId" -Template $approveAdminRelationshipTemplate
```

Automates the interactions with a web browser to approve an admin relationship.

### Example 4

```powershell
PS C:\> $enableIntuneConnectionTemplate = @"
[
  {
    "ActionType": "authenticate",
    "Sequence": 0,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "click",
    "Sequence": 1,
    "WaitThresholdInSeconds": 30,
    "WebElementLocator": "//span[@aria-label='Microsoft Intune connection']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "click",
    "Sequence": 2,
    "WaitThresholdInSeconds": 30,
    "WebElementLocator": "//button[@data-track-id='AdvancedFeaturesSave']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 3,
    "WaitThresholdInSeconds": 30
  }
]
"@
PS C:\>
PS C:\>PS C:\> Invoke-AbWebDriverAction -Credential $credential -NavigationUrl "https://security.microsoft.com/preferences2/integration" -Template $enableIntuneConnectionTemplate
```

Automates the interactions with a web browser to activate Microsoft Defender for Endpoint.

### Example 5

```powershell
PS C:\> $updateTenantDisplayNameTemplate = @"
[
  {
    "ActionType": "authenticate",
    "Sequence": 0,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "click",
    "Sequence": 1,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//input[@data-automation-id='OrganizationName']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "sendKeys",
    "Sequence": 2,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//input[@data-automation-id='OrganizationName']",
    "WebElementLocatorType": "xpath",
    "WebElementValues": [
      {
        "KeyValue": 57353,
        "Text": "a"
      },
      {
        "KeyValue": 57347
      },
      {
        "Text": "{{OrganizationName}}"
      }
    ]
  },
  {
    "ActionType": "click",
    "Sequence": 3,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//div[3]/div/div/button/span/span/span",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 4,
    "WaitThresholdInSeconds": 15
  }
]
"@
PS C:\>
PS C:\>PS C:\> Invoke-AbWebDriverAction -Credential $credential -NavigationUrl "https://admin.microsoft.com/#/Settings/OrganizationProfile/:/Settings/L1/OrganizationInformation" -Template $updateTenantDisplayNameTemplate -Parameters @{ OrganizationName = 'New Organization Name' }
```

Automates the interactions with a web browser to update the organization display name.

### Examples 6

```powershell
PS C:\> $promoCodeTemplate = @"
[
  {
    "ActionType": "sendKeys",
    "Sequence": 0,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "TextField3",
    "WebElementLocatorType": "id",
    "WebElementValues": [
      {
        "Text": "{{EmailAddress}}"
      }
    ]
  },
  {
    "ActionType": "click",
    "Sequence": 1,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//button[@data-bi-id='CollectEmailNext']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "click",
    "Sequence": 2,
    "WaitThresholdInSeconds": 15,
    "WebElementLocator": "//button[@data-bi-id='AccountExistSignIn']",
    "WebElementLocatorType": "xpath"
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 3,
    "WaitThresholdInSeconds": 15
  },
  {
      "ActionType": "switchWindow",
      "Sequence": 4
  },
  {
      "ActionType": "authenticateWithPassword",
      "Sequence": 5
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 6,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "switchWindow",
    "Sequence": 7
  },
  {
    "ActionType": "timeDelay",
    "Sequence": 8,
    "WaitThresholdInSeconds": 15
  },
  {
    "ActionType": "click",
    "Sequence": 9,
    "WaitThresholdInSeconds": 20,
    "WebElementLocator": "MultiPageLayout_Next",
    "WebElementLocatorType": "id"
  }
]
"@
PS C:\>
PS C:\> Invoke-AbWebDriverAction -Credential $credential -NavigationUrl 'https://signup.microsoft.com/get-started/signup?products=redactedValue&pc=redactedValue&ru=https://aka.ms/mdesmb' -Template $promoCodeTemplate -Parameters @{ EmailAddress = 'meganb@contoso.onmicrosoft.com' }
```

## PARAMETERS

### -Credential

The credentials to be used for authentication after navigating to the web address.

```yaml
Type: System.Management.Automation.PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NavigationUrl

The web address where the web driver will navigate.

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

### -Parameters

The parameters used to populate variables in the template.

```yaml
Type: System.Collections.Hashtable
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Template

The template, represented by JSON, that contains the web driver actions to be performed.

```yaml
Type: System.String
Parameter Sets: TemplateParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateFile

The path for the template file that contains the web drive actions to be performed.

```yaml
Type: System.String
Parameter Sets: TemplateFileParameterSet
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

### System.Void

## NOTES

## RELATED LINKS
