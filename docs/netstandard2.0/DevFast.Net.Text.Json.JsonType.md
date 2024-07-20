#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json](DevFast.Net.Text.Json.md 'DevFast.Net.Text.Json')

## JsonType Enum

Various JSON types as defined in https://datatracker.ietf.org/doc/html/rfc7159
(also mentioned at https://www.json.org/json-en.html).

```csharp
public enum JsonType
```
### Fields

<a name='DevFast.Net.Text.Json.JsonType.Arr'></a>

`Arr` 2

Array/collection of other JSON types, containing everything including & in-between '[' and ']'.

<a name='DevFast.Net.Text.Json.JsonType.Bool'></a>

`Bool` 5

JSON 'true' or 'false' literal.

<a name='DevFast.Net.Text.Json.JsonType.Null'></a>

`Null` 6

JSON 'null' literal (NOT same as [Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined')).

<a name='DevFast.Net.Text.Json.JsonType.Num'></a>

`Num` 3

JSON numerical values defined as 'integer fraction exponent'. See https://www.json.org/json-en.html.

<a name='DevFast.Net.Text.Json.JsonType.Obj'></a>

`Obj` 1

JSON object containing everything including & in-between '{' and '}'.

<a name='DevFast.Net.Text.Json.JsonType.Str'></a>

`Str` 4

JSON string value containing everything including and in-between '"' and '"'.

<a name='DevFast.Net.Text.Json.JsonType.Undefined'></a>

`Undefined` 0

Absence of any other JSON types. Normally represents end of JSON data or absence of value.