# Breaking Change Definition

The introduction of breaking changes will impact how users can interact with the module, and often it will require the user invest additional effort. Through use of the [breaking change attribute](https://github.com/automationbrew/autobrew-powershell/blob/main/src/PowerShell/Attributes/BreakingChangeAttribute.cs) we can help minimize the disruption by providing advanced warning of any upcoming breaking change.

## Surpressing the warning

Automation Brew PowerShell provides the functionality to surpress the warning message for any breaking change. If you would like to disable this functionality, then utilize one of the following commands.

```powershell
# Disables breaking change warnings the current and future PowerShell sessions.
Update-AbConfiguration -DisplayBreakingChanges $false -Scope Process 

# Disables breaking change warnings for only the current PowerShell session.
Update-AbConfiguration -DisplayBreakingChanges $false -Scope Process 
```

When this configuration is enabled no warning messages when be displayed when invoking commands. Disabling this functionality means you will not receive any advanced warning of breaking changes. In this circumstance it is recommended that you periodically review the [breaking changes](breaking-changes.md) documentation to gain insight into what will be changing.

## Definitions

### Commands

* Change in the `OutputType` or removal of the `OutputType` attribute
  * Before making the change update the command with the [OutputBreakingChangeAttribute]()
* Changing a command name without an alias to the original name
* Removing an attribute option such as `SupportsPaging` or `SupportShouldProcess`
* Removing or changing a command alias

## Examples

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

#### Effect at runtime

```output
Get-AbSomeObjectA <parms here>
...
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

#### Effect at runtime

```output
Get-AbSomeObjectA <parms here>
...
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

#### Effect at runtime

```output
Get-AbSomeObjectA  <parms here>
...
Breaking changes in the cmdlet : Get-AbSomeObjectA
 - The output type 'Foo' is changing
 - The following properties in the output type are being deprecated :
    'Prop3' 'Prop4'
```
