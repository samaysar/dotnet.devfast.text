#### [DevFast.Net.Text](index.md 'index')

## DevFast.Net.Text.Json Namespace

Under this [namespace](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/namespace 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/namespace') various JSON data related artifacts are implemented.

| Classes | |
| :--- | :--- |
| [JsonConst](DevFast.Net.Text.Json.JsonConst.md 'DevFast.Net.Text.Json.JsonConst') | Static class holding constant or fixed values for JSON text processing. |
| [JsonReader](DevFast.Net.Text.Json.JsonReader.md 'DevFast.Net.Text.Json.JsonReader') | Static class to create JSON reader instance. |

| Structs | |
| :--- | :--- |
| [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson') | Raw JSON output that provide a valid [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType') and corresponding [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value').   When [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') is [Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined'), [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value') is an empty [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') array. |

| Interfaces | |
| :--- | :--- |
| [IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader') | Interface dictating implementation of parsing individual items of a JSON Array with the possibility to parse individual elements (as defined in [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType')) in a JSON sequence. Parsing of such elements produces [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson') representing entire value-form (including structural characters, string quotes etc.) as [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value'), of single element at a time, of a known [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType'). |

| Enums | |
| :--- | :--- |
| [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType') | Various JSON types as defined in https://datatracker.ietf.org/doc/html/rfc7159 (also mentioned at https://www.json.org/json-en.html). |
