# Breaking Change Definition

The introduction of breaking changes will impact how users can interact with the module, and often it will require the user invest additional effort. Through use of the [breaking change attribute](https://github.com/automationbrew/autobrew-powershell/blob/main/src/PowerShell/Attributes/BreakingChangeAttribute.cs) we can help minimize the disruption by providing advanced warning of any upcoming breaking change.

## Surpressing the warning

Automation Brew PowerShell provides the functionality to surpress the warning message for any breaking change. If you would like to disable this functionality, then utilize one of the following commands.

```powershell
# Disables breaking change warnings the current and future PowerShell sessions.
Update-AbConfiguration -DisplayBreakingChanges $false -Scope CurrentUser 

# Disables breaking change warnings for only the current PowerShell session.
Update-AbConfiguration -DisplayBreakingChanges $false -Scope Process 
```

When this configuration is enabled no warning messages when be displayed when invoking commands. Disabling this functionality means you will not receive any advanced warning of breaking changes. In this circumstance it is recommended that you periodically review the [breaking changes](breaking-changes.md) documentation to gain insight into what will be changing.

## Definitions

### Commands

* Change in the `OutputType` or removal of the `OutputType` attribute
  * Update the command, to apply the [OutputBreakingChangeAttribute](breaking-defined.md#outputbreakingchangeattribute) attribute. 
* Changing a command name without an alias to the original name
  * Update the command, to apply the [CommandDeprecationAttribute](breaking-defined.md#commanddeprecationattribute) attribute.
* Removing an attribute option such as `SupportsPaging` or `SupportShouldProcess`
  * Update the command, to apply the [BreakingChangeAttribute](breaking-defined.md#breakingchangeattribute) attribute specifically mentioning the parameter that be removed.
* Removing or changing a command alias
  * Update the command, to apply the [BreakingChangeAttribute](breaking-defined.md#breakingchangeattribute) attribute specifically mentioning the alias that will be depcreated.

### Parameters

* Adding a required parameter to an existing parameter set
* Changing parameter order for parameter sets with ordered parameters
* Changing the name of a parameter without an alias to the original parameter name
* Making parameter validation more exclusive (e.g., removing values from a `ValidateSet`)
* Removing a parameter
* Removing or changing a parameter alias
* Removing or changing existing parameter attribute values

### Types

* Adding additional required properties
* Adding required parameters, changing parameter names, or parameter types for methods or constructors
* Changing property names without an accompanying alias to the original name
* Changing return types of methods
* Removing properties

## Examples

### BreakingChangeAttribute

This attribute should be used when making a breaking change that is not handled by one of the specialized attributes below.

#### Behavioral change

```csharp
[BreakingChange("Adding the new mandatory parameter Category.", NewWay = "Get-AbSomeObjectA -Category Food", OldWay = "Get-AbSomeObjectA")]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - Adding the new mandatory parameter Category.
Cmdlet invocation changes :
    Old Way : Get-AbSomeObjectA
    New Way : Get-AbSomeObjectA -Category Food
```

#### Simple message

```csharp
[BreakingChange("This is a simple message.")]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - This is a simple message
```

#### Simple message with version

```csharp
[BreakingChange("This is a simple message.", "2.0.0.0")]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - This is a simple message
    The change is expected to take effect from the version : 2.0.0.0
```

#### Simple message with version and date

```csharp
[BreakingChange("This is a simple message.", "2.0.0.0", "07/01/2023")]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - This is a simple message
    NOTE : This change will take effect on '07/01/2023'
    The change is expected to take effect from the version : 2.0.0.0
```

### OutputBreakingChangeAttribute

This attirbute should be used when the output type for a command will be changing. 

#### Output type is changing

```csharp
[OutputBreakingChange(typeof(Foo), ReplacementOutputTypeName = "Dictionary<String, Foo>")]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The output type is changing from the existing type :'Foo' to the new type :'Dictionary<String, Foo>'
```

#### New properties are being added to the output type

```csharp
[OutputBreakingChange(typeof(Foo), NewOutputProperties = new String[] {"Prop1", "Prop2"})]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The output type 'Foo' is changing
 - The following properties are being added to the output type :
    'Prop1' 'Prop2'
```

#### Properties in the output type are being deprecated

```csharp
[OutputBreakingChange(typeof(Foo), DeprecatedOutputProperties = new String[] {"Prop3", "Prop4"})]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetAbSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA  <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The output type 'Foo' is changing
 - The following properties in the output type are being deprecated :
    'Prop3' 'Prop4'
```

### CommandDeprecationAttribute

This attirbute should be used when deprecating an alias or command.

#### There is a replacement

```csharp
[CmdletDeprecation(ReplacementCmdletName = "Get-AbSomeObjectB")]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <parms here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
The cmdlet 'Get-AbSomeObjectB' is replacing this cmdlet
```

#### There is not a replacement

```csharp
[CmdletDeprecation]
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <params here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
The cmdlet is being deprecated. There will be no replacement for it.
```

### ParameterBreakingChangeAttribute

This attribute should be used when the parameters for a command are changing.

#### Being mandatory

```csharp
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Gets or sets the name for the object.
    /// </summary>
    [Parameter(HelpMessage = "The name for the object.", Mandatory = false)]
    [ParameterBreakingChange(nameof(Name), IsBecomingMandatory = true)]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <params here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The parameter 'Name' is becoming mandatory
```

#### Being replaced

```csharp
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Gets or sets the name for the object.
    /// </summary>
    [Parameter(HelpMessage = "The name for the object.", Mandatory = false)]
    [ParameterBreakingChange(nameof(Name), IsBecomingMandatory = true, ReplacementParameterName = "ObjectId")]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <params here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The parameter 'Name' is  being replaced by mandatory parameter 'ObjectId'
```

#### Changing type

```csharp
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Gets or sets the name for the object.
    /// </summary>
    [Parameter(HelpMessage = "The name for the object.", Mandatory = false)]
    [ParameterBreakingChange(nameof(Name), NewParameterTypeName = "MyName", OldParamaterType = typeof(string))]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <params here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The parameter 'Name' is changing
    The type of the parameter is changing from 'string' to 'MyName'.
```

#### Simple message

```csharp
[Cmdlet(VerbsCommon.Get, "AbSomeObjectA"), OutputType(typeof(Foo))]
public class GetSomeObjectA : ModuleCmdlet
{
    /// <summary>
    /// Gets or sets the name for the object.
    /// </summary>
    [Parameter(HelpMessage = "The name for the object.", Mandatory = false)]
    [ParameterBreakingChange(nameof(Name), ChangeDescription = "This is a simple message.")]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    /// <summary>
    /// Performs the actions associated with the command.
    /// </summary>
    protected override void PerformCmdlet()
    {
    }
}
```

##### Effect at runtime

```output
Get-AbSomeObjectA <params here>

Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The parameter 'Name' is changing
    Change description : This is a simple message.
```
