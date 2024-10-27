#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json.NamingPolicy](DevFast.Net.Text.Json.NamingPolicy.md 'DevFast.Net.Text.Json.NamingPolicy')

## JsonSnakeCaseNamingPolicy Class

Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide Snake-Case names
(e.g.: AbcDef to abc_def, MyTKiBd to my_tki_bd, ABC to abc etc) for JSON data.

```csharp
public sealed class JsonSnakeCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
```
- *Properties*
  - **[SnakeCase](DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.SnakeCase 'DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.SnakeCase')**
- *Methods*
  - **[ConvertName(string)](DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.ConvertName(string) 'DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.ConvertName(string)')**

## JsonSnakeCaseNamingPolicy Class

Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide Snake-Case names
(e.g.: AbcDef to abc_def, MyTKiBd to my_tki_bd, ABC to abc etc) for JSON data.

```csharp
public sealed class JsonSnakeCaseNamingPolicy : System.Text.Json.JsonNamingPolicy
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') &#129106; JsonSnakeCaseNamingPolicy
### Properties

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.SnakeCase'></a>

## JsonSnakeCaseNamingPolicy.SnakeCase Property

Returns the naming policy for snake-casing.

```csharp
public static System.Text.Json.JsonNamingPolicy SnakeCase { get; }
```

#### Property Value
[System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy')
### Methods

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.ConvertName(string)'></a>

## JsonSnakeCaseNamingPolicy.ConvertName(string) Method

Converts provided [name](DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.ConvertName(string).name 'DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.ConvertName(string).name') to snake-case
(e.g.: AbcDef to abc_def, MyTKiBd to my_tki_bd, ABC to abc etc).

```csharp
public override string ConvertName(string name);
```
#### Parameters

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.ConvertName(string).name'></a>

`name` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

String value to convert to snake-case.

#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')