#### [DevFast.Net.Text](index.md 'index')
### [DevFast.Net.Text.Json](DevFast.Net.Text.Json.md 'DevFast.Net.Text.Json')

## RawJson Struct

Raw JSON output that provide a valid [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType') and corresponding
[Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value').

When [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') is [Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined'), [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value') is
an empty [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') array.

```csharp
public readonly struct RawJson :
System.IEquatable<DevFast.Net.Text.Json.RawJson>
```

Implements [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')

### Remarks
Create an instance with JSON type and corresponding raw value.
### Constructors

<a name='DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType,byte[])'></a>

## RawJson(JsonType, byte[]) Constructor

Raw JSON output that provide a valid [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType') and corresponding
[Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value').

When [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') is [Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined'), [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value') is
an empty [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') array.

```csharp
public RawJson(DevFast.Net.Text.Json.JsonType type, byte[] value);
```
#### Parameters

<a name='DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType,byte[]).type'></a>

`type` [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType')

[JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType') of the [value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType,byte[]).value 'DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType, byte[]).value')

<a name='DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType,byte[]).value'></a>

`value` [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')

Byte sequence of the associated [JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType')

### Remarks
Create an instance with JSON [type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType,byte[]).type 'DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType, byte[]).type') and corresponding raw [value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType,byte[]).value 'DevFast.Net.Text.Json.RawJson.RawJson(DevFast.Net.Text.Json.JsonType, byte[]).value').
### Properties

<a name='DevFast.Net.Text.Json.RawJson.Type'></a>

## RawJson.Type Property

JSON type of raw [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value').

```csharp
public DevFast.Net.Text.Json.JsonType Type { get; }
```

#### Property Value
[JsonType](DevFast.Net.Text.Json.JsonType.md 'DevFast.Net.Text.Json.JsonType')

<a name='DevFast.Net.Text.Json.RawJson.Value'></a>

## RawJson.Value Property

Raw JSON value. When [Type](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Type 'DevFast.Net.Text.Json.RawJson.Type') is [Undefined](DevFast.Net.Text.Json.JsonType.md#DevFast.Net.Text.Json.JsonType.Undefined 'DevFast.Net.Text.Json.JsonType.Undefined'), [Value](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Value 'DevFast.Net.Text.Json.RawJson.Value') is
an empty [System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte') array.

```csharp
public byte[] Value { get; }
```

#### Property Value
[System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')
### Methods

<a name='DevFast.Net.Text.Json.RawJson.Equals(DevFast.Net.Text.Json.RawJson)'></a>

## RawJson.Equals(RawJson) Method

Indicates if provided [other](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Equals(DevFast.Net.Text.Json.RawJson).other 'DevFast.Net.Text.Json.RawJson.Equals(DevFast.Net.Text.Json.RawJson).other') instance is equal to current instance.

```csharp
public bool Equals(DevFast.Net.Text.Json.RawJson other);
```
#### Parameters

<a name='DevFast.Net.Text.Json.RawJson.Equals(DevFast.Net.Text.Json.RawJson).other'></a>

`other` [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

Another instance

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.RawJson.Equals(object)'></a>

## RawJson.Equals(object) Method

Indicates if provided [obj](DevFast.Net.Text.Json.RawJson.md#DevFast.Net.Text.Json.RawJson.Equals(object).obj 'DevFast.Net.Text.Json.RawJson.Equals(object).obj') instance is equal to current instance.

```csharp
public override bool Equals(object? obj);
```
#### Parameters

<a name='DevFast.Net.Text.Json.RawJson.Equals(object).obj'></a>

`obj` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

Another instance

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.RawJson.GetHashCode()'></a>

## RawJson.GetHashCode() Method

Returns hashcode of current instance.

```csharp
public override int GetHashCode();
```

#### Returns
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Operators

<a name='DevFast.Net.Text.Json.RawJson.op_Equality(DevFast.Net.Text.Json.RawJson,DevFast.Net.Text.Json.RawJson)'></a>

## RawJson.operator ==(RawJson, RawJson) Operator

Equality operator to compare 2 instances.

```csharp
public static bool operator ==(DevFast.Net.Text.Json.RawJson left, DevFast.Net.Text.Json.RawJson right);
```
#### Parameters

<a name='DevFast.Net.Text.Json.RawJson.op_Equality(DevFast.Net.Text.Json.RawJson,DevFast.Net.Text.Json.RawJson).left'></a>

`left` [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

Instance left to the operator

<a name='DevFast.Net.Text.Json.RawJson.op_Equality(DevFast.Net.Text.Json.RawJson,DevFast.Net.Text.Json.RawJson).right'></a>

`right` [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

Instance right to the operator

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='DevFast.Net.Text.Json.RawJson.op_Inequality(DevFast.Net.Text.Json.RawJson,DevFast.Net.Text.Json.RawJson)'></a>

## RawJson.operator !=(RawJson, RawJson) Operator

Inequality operation to compare 2 instances.

```csharp
public static bool operator !=(DevFast.Net.Text.Json.RawJson left, DevFast.Net.Text.Json.RawJson right);
```
#### Parameters

<a name='DevFast.Net.Text.Json.RawJson.op_Inequality(DevFast.Net.Text.Json.RawJson,DevFast.Net.Text.Json.RawJson).left'></a>

`left` [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

Instance left to the operator

<a name='DevFast.Net.Text.Json.RawJson.op_Inequality(DevFast.Net.Text.Json.RawJson,DevFast.Net.Text.Json.RawJson).right'></a>

`right` [RawJson](DevFast.Net.Text.Json.RawJson.md 'DevFast.Net.Text.Json.RawJson')

Instance right to the operator

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')