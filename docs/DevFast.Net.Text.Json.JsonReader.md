#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json](DevFast.Net.Text.Json.md 'DevFast.Net.Text.Json')

## JsonReader Class

Static class to create JSON reader instance.

```csharp
public static class JsonReader
```
- *Methods*
  - **[CreateUtf8ArrayReaderAsync(Stream, CancellationToken, int, bool)](DevFast.Net.Text.Json.JsonReader.md#DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool) 'DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream, System.Threading.CancellationToken, int, bool)')**

## JsonReader Class

Static class to create JSON reader instance.

```csharp
public static class JsonReader
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; JsonReader
### Methods

<a name='DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool)'></a>

## JsonReader.CreateUtf8ArrayReaderAsync(Stream, CancellationToken, int, bool) Method

Convenient method, to create well initialized instance of type [IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader').

```csharp
public static System.Threading.Tasks.ValueTask<DevFast.Net.Text.Json.IJsonArrayReader> CreateUtf8ArrayReaderAsync(System.IO.Stream stream, System.Threading.CancellationToken token=default(System.Threading.CancellationToken), int size=512, bool disposeStream=false);
```
#### Parameters

<a name='DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool).stream'></a>

`stream` [System.IO.Stream](https://docs.microsoft.com/en-us/dotnet/api/System.IO.Stream 'System.IO.Stream')

A readable stream containing JSON array data.

<a name='DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool).token'></a>

`token` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

Cancellation token to observe.

<a name='DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool).size'></a>

`size` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

Initial size of underlying byte buffer. Any value less than
            [RawUtf8JsonPartReaderMinBuffer](DevFast.Net.Text.Json.JsonConst.md#DevFast.Net.Text.Json.JsonConst.RawUtf8JsonPartReaderMinBuffer 'DevFast.Net.Text.Json.JsonConst.RawUtf8JsonPartReaderMinBuffer') will be ignored.

<a name='DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool).disposeStream'></a>

`disposeStream` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') to dispose [stream](DevFast.Net.Text.Json.JsonReader.md#DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool).stream 'DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream, System.Threading.CancellationToken, int, bool).stream') when either
            current instance of [IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader') itself disposing or
            when [stream](DevFast.Net.Text.Json.JsonReader.md#DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream,System.Threading.CancellationToken,int,bool).stream 'DevFast.Net.Text.Json.JsonReader.CreateUtf8ArrayReaderAsync(System.IO.Stream, System.Threading.CancellationToken, int, bool).stream') is completely read; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') otherwise.

#### Returns
[System.Threading.Tasks.ValueTask&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.ValueTask-1 'System.Threading.Tasks.ValueTask`1')[IJsonArrayReader](DevFast.Net.Text.Json.IJsonArrayReader.md 'DevFast.Net.Text.Json.IJsonArrayReader')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.ValueTask-1 'System.Threading.Tasks.ValueTask`1')