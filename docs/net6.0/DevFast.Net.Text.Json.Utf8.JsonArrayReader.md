#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json.Utf8](DevFast.Net.Text.Json.Utf8.md 'DevFast.Net.Text.Json.Utf8')

## JsonArrayReader Class

Class implementing [IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader') for standard Utf-8 JSON data encoding
based on https://datatracker.ietf.org/doc/html/rfc7159 (grammar shown at https://www.json.org/json-en.html).

This implementation support both single line comments (starting with '//' and ending in either Carriage return '\r'
or newline '\n') and multiline comments (starting with '/*' and ending with '*/').

```csharp
public sealed class JsonArrayReader :
DevFast.Net.Text.Json.IJsonArrayReader,
System.IDisposable
```
- *Properties*
  - **[Capacity](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.Capacity 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.Capacity')**
  - **[Current](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.Current 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.Current')**
  - **[EoJ](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ')**
  - **[Position](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.Position 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.Position')**
- *Methods*
  - **[Dispose()](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.Dispose() 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.Dispose()')**
  - **[EnumerateJsonArray(bool, CancellationToken)](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool, System.Threading.CancellationToken)')**
  - **[ReadIsBeginArray(CancellationToken)](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken) 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken)')**
  - **[ReadIsBeginArrayWithVerify(CancellationToken)](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken) 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken)')**
  - **[ReadIsEndArray(bool, CancellationToken)](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsEndArray(bool, System.Threading.CancellationToken)')**
  - **[ReadRaw(bool, CancellationToken)](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadRaw(bool, System.Threading.CancellationToken)')**

## JsonArrayReader Class

Class implementing [IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader') for standard Utf-8 JSON data encoding
based on https://datatracker.ietf.org/doc/html/rfc7159 (grammar shown at https://www.json.org/json-en.html).

This implementation support both single line comments (starting with '//' and ending in either Carriage return '\r'
or newline '\n') and multiline comments (starting with '/*' and ending with '*/').

```csharp
public sealed class JsonArrayReader :
DevFast.Net.Text.Json.IJsonArrayReader,
System.IDisposable
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; JsonArrayReader

Implements [IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader'), [System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable 'System.IDisposable')
### Properties

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.Capacity'></a>

## JsonArrayReader.Capacity Property

Current capacity as total number of [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')s.

```csharp
public int Capacity { get; }
```

Implements [Capacity](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.Capacity 'DevFast.Net.Text.Json.IJsonArrayReader.Capacity')

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.Current'></a>

## JsonArrayReader.Current Property

[System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') value of current position of reader. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null') when
            reader has reached [EoJ](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ').

```csharp
public System.Nullable<byte> Current { get; }
```

Implements [Current](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.Current 'DevFast.Net.Text.Json.IJsonArrayReader.Current')

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ'></a>

## JsonArrayReader.EoJ Property

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') indicating that reader has reached end of JSON input,
            otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool').

```csharp
public bool EoJ { get; }
```

Implements [EoJ](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EoJ 'DevFast.Net.Text.Json.IJsonArrayReader.EoJ')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.Position'></a>

## JsonArrayReader.Position Property

Total number of [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')s observed by the reader since the very beginning (0-based position).

```csharp
public long Position { get; }
```

Implements [Position](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.Position 'DevFast.Net.Text.Json.IJsonArrayReader.Position')

#### Property Value
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')
### Methods

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.Dispose()'></a>

## JsonArrayReader.Dispose() Method

Clean up by releasing resources.

```csharp
public void Dispose();
```

Implements [Dispose()](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable.Dispose 'System.IDisposable.Dispose')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken)'></a>

## JsonArrayReader.EnumerateJsonArray(bool, CancellationToken) Method

Provides a convenient way to enumerate over elements of a JSON array (one at a time).
For every iteration, such mechanism produces [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson'), where [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value') represents
entire value-form (including structural characters, string quotes etc.) of such an individual
element & [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') indicates underlying JSON element type.
Any standard JSON serializer can be used to deserialize [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value')
to obtain an instance of corresponding .Net type.

```csharp
public System.Collections.Generic.IEnumerable<DevFast.Net.Text.Json.RawJson> EnumerateJsonArray(bool ensureEoj, System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken).ensureEoj'></a>

`ensureEoj` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ignore leftover JSON after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte').
            [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ensure that no data is present after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'). However, both
            single line and multiline comments are allowed after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte') until [EoJ](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ').

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe.

Implements [EnumerateJsonArray(bool, CancellationToken)](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool, System.Threading.CancellationToken)')

#### Returns
[System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken)'></a>

## JsonArrayReader.ReadIsBeginArray(CancellationToken) Method

Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') if value is [ArrayBeginByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayBeginByte 'DevFast.Net.Text.Json.JsonConst.ArrayBeginByte'). If the value matches,
then reader advances its current position to next [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') in the sequence or to end of JSON.
Otherwise, it returns [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') when current byte is NOT [ArrayBeginByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayBeginByte 'DevFast.Net.Text.Json.JsonConst.ArrayBeginByte') and
reader position is maintained on the current byte.

```csharp
public bool ReadIsBeginArray(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe

Implements [ReadIsBeginArray(CancellationToken)](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken) 'DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken)')

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken)'></a>

## JsonArrayReader.ReadIsBeginArrayWithVerify(CancellationToken) Method

Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it checks
if value is [ArrayBeginByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayBeginByte 'DevFast.Net.Text.Json.JsonConst.ArrayBeginByte'). If the value matches, then reader advances
its current position to next [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') in the sequence or to end of JSON. If the value does NOT match,
reader position is maintained on the current byte and an error
(of type [System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')) is thrown.

```csharp
public void ReadIsBeginArrayWithVerify(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe

Implements [ReadIsBeginArrayWithVerify(CancellationToken)](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken) 'DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken)')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken)'></a>

## JsonArrayReader.ReadIsEndArray(bool, CancellationToken) Method

Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') if value is [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'). If the value matches,
then reader advances its current position to next [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') in the sequence or to end of JSON.
Otherwise, it returns [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') when current byte is NOT [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte') and
reader position is maintained on the current byte.

```csharp
public bool ReadIsEndArray(bool ensureEoj, System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken).ensureEoj'></a>

`ensureEoj` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ignore any text (JSON or not) after
            observing [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte').
            [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ensure that no data is present after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'). However, both
            single line and multiline comments are allowed before [EoJ](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ').

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe

Implements [ReadIsEndArray(bool, CancellationToken)](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.IJsonArrayReader.ReadIsEndArray(bool, System.Threading.CancellationToken)')

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken)'></a>

## JsonArrayReader.ReadRaw(bool, CancellationToken) Method

Reads the current JSON element as [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson'). If reaches [EoJ](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EoJ') or
encounters [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'), returned [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') is
[Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined').

One should prefer [EnumerateJsonArray(bool, CancellationToken)](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md#DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.Utf8.JsonArrayReader.EnumerateJsonArray(bool, System.Threading.CancellationToken)')
to parse well-structured JSON stream over this method.
This method is to parse non-standard chain of JSON elements separated by ',' (or not).

```csharp
public DevFast.Net.Text.Json.RawJson ReadRaw(bool withVerify=true, System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken).withVerify'></a>

`withVerify` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to verify the presence of ',' or ']' (but not ',]')
            after successfully parsing the current JSON element; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') otherwise.

<a name='DevFast.Net.Text.Json.Utf8.JsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe.

Implements [ReadRaw(bool, CancellationToken)](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.IJsonArrayReader.ReadRaw(bool, System.Threading.CancellationToken)')

#### Returns
[RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')