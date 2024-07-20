#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json](DevFast.Net.Text.Json.md 'DevFast.Net.Text.Json')

## IJsonArrayReader Interface

Interface dictating implementation of parsing individual items of a JSON Array
with the possibility to parse individual elements (as defined in [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType')) in a JSON sequence.
Parsing of such elements produces [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson') representing entire value-form
(including structural characters, string quotes etc.) as [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value'), of single element at a time,
of a known [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType').

```csharp
public interface IJsonArrayReader :
System.IDisposable
```

Derived  
&#8627; [JsonArrayReader](DevFast.Net.Text.Json.Utf8.JsonArrayReader.md 'DevFast.Net.Text.Json.Utf8.JsonArrayReader')  
&#8627; [MemJsonArrayReader](DevFast.Net.Text.Json.Utf8.MemJsonArrayReader.md 'DevFast.Net.Text.Json.Utf8.MemJsonArrayReader')

Implements [System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable 'System.IDisposable')
### Properties

<a name='DevFast.Net.Text.Json.IJsonArrayReader.Capacity'></a>

## IJsonArrayReader.Capacity Property

Current capacity as total number of [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')s.

```csharp
int Capacity { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.Current'></a>

## IJsonArrayReader.Current Property

[System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') value of current position of reader. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null') when
            reader has reached [EoJ](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EoJ 'DevFast.Net.Text.Json.IJsonArrayReader.EoJ').

```csharp
System.Nullable<byte> Current { get; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.EoJ'></a>

## IJsonArrayReader.EoJ Property

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') indicating that reader has reached end of JSON input,
            otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool').

```csharp
bool EoJ { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.Position'></a>

## IJsonArrayReader.Position Property

Total number of [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')s observed by the reader since the very beginning (0-based position).

```csharp
long Position { get; }
```

#### Property Value
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')
### Methods

<a name='DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken)'></a>

## IJsonArrayReader.EnumerateJsonArray(bool, CancellationToken) Method

Provides a convenient way to enumerate over elements of a JSON array (one at a time).
For every iteration, such mechanism produces [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson'), where [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value') represents
entire value-form (including structural characters, string quotes etc.) of such an individual
element & [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') indicates underlying JSON element type.
Any standard JSON serializer can be used to deserialize [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value')
to obtain an instance of corresponding .Net type.

```csharp
System.Collections.Generic.IEnumerable<DevFast.Net.Text.Json.RawJson> EnumerateJsonArray(bool ensureEoj, System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken).ensureEoj'></a>

`ensureEoj` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ignore leftover JSON after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte').
            [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ensure that no data is present after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'). However, both
            single line and multiline comments are allowed after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte') until [EoJ](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EoJ 'DevFast.Net.Text.Json.IJsonArrayReader.EoJ').

<a name='DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe.

#### Returns
[System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken)'></a>

## IJsonArrayReader.ReadIsBeginArray(CancellationToken) Method

Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') if value is [ArrayBeginByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayBeginByte 'DevFast.Net.Text.Json.JsonConst.ArrayBeginByte'). If the value matches,
then reader advances its current position to next [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') in the sequence or to end of JSON.
Otherwise, it returns [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') when current byte is NOT [ArrayBeginByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayBeginByte 'DevFast.Net.Text.Json.JsonConst.ArrayBeginByte') and
reader position is maintained on the current byte.

```csharp
bool ReadIsBeginArray(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArray(System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken)'></a>

## IJsonArrayReader.ReadIsBeginArrayWithVerify(CancellationToken) Method

Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it checks
if value is [ArrayBeginByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayBeginByte 'DevFast.Net.Text.Json.JsonConst.ArrayBeginByte'). If the value matches, then reader advances
its current position to next [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') in the sequence or to end of JSON. If the value does NOT match,
reader position is maintained on the current byte and an error
(of type [System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')) is thrown.

```csharp
void ReadIsBeginArrayWithVerify(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsBeginArrayWithVerify(System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken)'></a>

## IJsonArrayReader.ReadIsEndArray(bool, CancellationToken) Method

Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') if value is [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'). If the value matches,
then reader advances its current position to next [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') in the sequence or to end of JSON.
Otherwise, it returns [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') when current byte is NOT [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte') and
reader position is maintained on the current byte.

```csharp
bool ReadIsEndArray(bool ensureEoj, System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken).ensureEoj'></a>

`ensureEoj` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ignore any text (JSON or not) after
            observing [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte').
            [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to ensure that no data is present after [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'). However, both
            single line and multiline comments are allowed before [EoJ](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EoJ 'DevFast.Net.Text.Json.IJsonArrayReader.EoJ').

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadIsEndArray(bool,System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken)'></a>

## IJsonArrayReader.ReadRaw(bool, CancellationToken) Method

Reads the current JSON element as [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson'). If it reaches [EoJ](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EoJ 'DevFast.Net.Text.Json.IJsonArrayReader.EoJ') or
encounters [ArrayEndByte](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.ArrayEndByte 'DevFast.Net.Text.Json.JsonConst.ArrayEndByte'), it returns [Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined') as
[Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type').

One should prefer [EnumerateJsonArray(bool, CancellationToken)](DevFast.Net.Text.Json.IJsonArrayReader.md#DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool,System.Threading.CancellationToken) 'DevFast.Net.Text.Json.IJsonArrayReader.EnumerateJsonArray(bool, System.Threading.CancellationToken)') to parse well-structured JSON stream over this method.
This method is to parse non-standard chain of JSON elements separated by ',' (or not).

```csharp
DevFast.Net.Text.Json.RawJson ReadRaw(bool withVerify=true, System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken).withVerify'></a>

`withVerify` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to verify the presence of ',' or ']' (but not ',]')
            after successfully parsing the current JSON element; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') otherwise.

<a name='DevFast.Net.Text.Json.IJsonArrayReader.ReadRaw(bool,System.Threading.CancellationToken).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe.

#### Returns
[RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

#### Exceptions

[System.Text.Json.JsonException](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonException 'System.Text.Json.JsonException')