#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json.NamingPolicy](DevFast.Net.Text.Json.NamingPolicy.md 'DevFast.Net.Text.Json.NamingPolicy')

## JsonLongSnakeCaseNamingPolicy Class

Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide long Snake-Case names
(e.g.: AbcDef to abc_def, MyTKiBd to my_t_ki_bd, ABC to a_b_c etc) for JSON data.

```csharp
public sealed class JsonLongSnakeCaseNamingPolicy
```
- *Properties*
  - **[LongSnakeCase](DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.LongSnakeCase 'DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.LongSnakeCase')**
- *Methods*
  - **[ConvertName(string)](DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.ConvertName(string) 'DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.ConvertName(string)')**

## JsonLongSnakeCaseNamingPolicy Class

Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide long Snake-Case names
(e.g.: AbcDef to abc_def, MyTKiBd to my_t_ki_bd, ABC to a_b_c etc) for JSON data.

```csharp
public sealed class JsonLongSnakeCaseNamingPolicy
```

Inheritance [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') &#129106; JsonLongSnakeCaseNamingPolicy
### Properties

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.LongSnakeCase'></a>

## JsonLongSnakeCaseNamingPolicy.LongSnakeCase Property

Returns the naming policy for snake-casing.

```csharp
public static JsonNamingPolicy LongSnakeCase { get; }
```

#### Property Value
[System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy')
### Methods

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.ConvertName(string)'></a>

## JsonLongSnakeCaseNamingPolicy.ConvertName(string) Method

Converts provided [name](DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.md#DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.ConvertName(string).name 'DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.ConvertName(string).name') to snake-case
(e.g.: AbcDef to abc_def, MyTKiBd to my_t_ki_bd, ABC to a_b_c etc).

```csharp
public override string ConvertName(string name);
```
#### Parameters

<a name='DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.ConvertName(string).name'></a>

`name` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

String value to convert to snake-case.

#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')