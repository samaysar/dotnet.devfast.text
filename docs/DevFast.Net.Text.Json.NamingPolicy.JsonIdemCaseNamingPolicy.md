#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json.NamingPolicy](DevFast.Net.Text.Json.NamingPolicy.md 'DevFast.Net.Text.Json.NamingPolicy')

## JsonIdemCaseNamingPolicy Class

Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide Identical (i.e. no change) names
(e.g.: AbcDef to AbcDef, MyTKiBd to MyTKiBd, ABC to ABC etc) for JSON data.

```csharp
public sealed class JsonIdemCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
```
- *Properties*
  - **[IdemCase](DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.IdemCase 'DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.IdemCase')**
- *Methods*
  - **[ConvertName(string)](DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.ConvertName(string) 'DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.ConvertName(string)')**

## JsonIdemCaseNamingPolicy Class

Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide Identical (i.e. no change) names
(e.g.: AbcDef to AbcDef, MyTKiBd to MyTKiBd, ABC to ABC etc) for JSON data.

```csharp
public sealed class JsonIdemCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') &#129106; JsonIdemCaseNamingPolicy
### Properties

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.IdemCase'></a>

## JsonIdemCaseNamingPolicy.IdemCase Property

Returns the naming policy which does not change the name.

```csharp
public static System.Text.Json.JsonNamingPolicy IdemCase { get; }
```

#### Property Value
[System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy')
### Methods

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.ConvertName(string)'></a>

## JsonIdemCaseNamingPolicy.ConvertName(string) Method

Does NOT convert provided [name](DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.ConvertName(string).name 'DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.ConvertName(string).name') and returns the
value as it is (e.g.: AbcDef to AbcDef, MyTKiBd to MyTKiBd, ABC to ABC etc).

```csharp
public override string ConvertName(string name);
```
#### Parameters

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.ConvertName(string).name'></a>

`name` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

String value to convert.

#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')