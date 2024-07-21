﻿namespace DevFast.Net.Text.Json.Utf8;

/// <summary>
/// Class implementing <see cref="IJsonArrayReader"/> for standard Utf-8 JSON data encoding
/// based on https://datatracker.ietf.org/doc/html/rfc7159 (grammar shown at https://www.json.org/json-en.html).
/// <para>
/// This implementation support both single line comments (starting with '//' and ending in either Carriage return '\r'
/// or newline '\n') and multiline comments (starting with '/*' and ending with '*/').
/// </para>
/// </summary>
public sealed class JsonArrayReader : IJsonArrayReader
{
    private readonly bool _disposeStream;
    private Stream? _stream;
    private byte[] _buffer;
    private int _begin, _end, _current;
    private long _bytesConsumed;

    internal JsonArrayReader(Stream stream, byte[] buffer, int begin, int end, bool disposeStream)
    {
        _stream = stream;
        _buffer = buffer;
        _current = _begin = begin;
        _bytesConsumed = _begin;
        _end = end;
        _disposeStream = disposeStream;
    }

    /// <summary>
    /// <see langword="true"/> indicating that reader has reached end of JSON input,
    /// otherwise <see langword="false"/>.
    /// </summary>
    public bool EoJ => _stream == null && _current == _end;

    /// <summary>
    /// <see cref="byte"/> value of current position of reader. <see langword="null"/> when
    /// reader has reached <see cref="EoJ"/>.
    /// </summary>
    public byte? Current => EoJ ? null : _buffer[_current];

    /// <summary>
    /// Total number of <see cref="byte"/>s observed by the reader since the very beginning (0-based position).
    /// </summary>
    public long Position => _bytesConsumed + (Math.Min(_current, _end) - _begin);

    /// <summary>
    /// Current capacity as total number of <see cref="byte"/>s.
    /// </summary>
    public int Capacity => _buffer.Length;

    private bool InRange => _current < _end;

    /// <summary>
    /// Provides a convenient way to enumerate over elements of a JSON array (one at a time).
    /// For every iteration, such mechanism produces <see cref="RawJson"/>, where <see cref="RawJson.Value"/> represents
    /// entire value-form (including structural characters, string quotes etc.) of such an individual
    /// element &amp; <see cref="RawJson.Type"/> indicates underlying JSON element type.
    /// Any standard JSON serializer can be used to deserialize <see cref="RawJson.Value"/>
    /// to obtain an instance of corresponding .Net type.
    /// </summary>
    /// <param name="ensureEoj"><see langword="false"/> to ignore leftover JSON after <see cref="JsonConst.ArrayEndByte"/>.
    /// <see langword="true"/> to ensure that no data is present after <see cref="JsonConst.ArrayEndByte"/>. However, both
    /// single line and multiline comments are allowed after <see cref="JsonConst.ArrayEndByte"/> until <see cref="EoJ"/>.</param>
    /// <param name="token">Cancellation token to observe.</param>
    /// <exception cref="JsonEx"></exception>
    public IEnumerable<RawJson> EnumerateJsonArray(bool ensureEoj,
        Token token = default)
    {
        ReadIsBeginArrayWithVerify(token);
        while (!ReadIsEndArray(ensureEoj, token))
        {
            RawJson next = ReadRaw(true, token);
            yield return next.Type == JsonType.Undefined
                ? throw new JsonEx("Expected a valid JSON element or end of JSON array. " +
                    $"0-Based Position = {Position}.")
                : next;
        }
    }

    /// <summary>
    /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it checks
    /// if value is <see cref="JsonConst.ArrayBeginByte"/>. If the value matches, then reader advances
    /// its current position to next <see cref="byte"/> in the sequence or to end of JSON. If the value does NOT match,
    /// reader position is maintained on the current byte and an error
    /// (of type <see cref="JsonEx"/>) is thrown.
    /// </summary>
    /// <param name="token">Cancellation token to observe</param>
    /// <exception cref="JsonEx"></exception>
    public void ReadIsBeginArrayWithVerify(Token token = default)
    {
        if (!ReadIsBeginArray(token))
        {
            if (InRange)
            {
                throw new JsonEx("Invalid byte value for JSON begin-array. " +
                                                        $"Expected = {JsonConst.ArrayBeginByte}, " +
                                                        $"Found = {(char)Current!}, " +
                                                        $"0-Based Position = {Position}.");
            }
            throw new JsonEx("Reached end, unable to find JSON begin-array. " +
                                                    $"0-Based Position = {Position}.");
        }
    }

    /// <summary>
    /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
    /// <see langword="true"/> if value is <see cref="JsonConst.ArrayBeginByte"/>. If the value matches,
    /// then reader advances its current position to next <see cref="byte"/> in the sequence or to end of JSON.
    /// Otherwise, it returns <see langword="false"/> when current byte is NOT <see cref="JsonConst.ArrayBeginByte"/> and
    /// reader position is maintained on the current byte.
    /// </summary>
    /// <param name="token">Cancellation token to observe</param>
    public bool ReadIsBeginArray(Token token = default)
    {
        return ReadIsGivenByte(JsonConst.ArrayBeginByte, token);
    }

    /// <summary>
    /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
    /// <see langword="true"/> if value is <see cref="JsonConst.ArrayEndByte"/>. If the value matches,
    /// then reader advances its current position to next <see cref="byte"/> in the sequence or to end of JSON.
    /// Otherwise, it returns <see langword="false"/> when current byte is NOT <see cref="JsonConst.ArrayEndByte"/> and
    /// reader position is maintained on the current byte.
    /// </summary>
    /// <param name="ensureEoj"><see langword="false"/> to ignore any text (JSON or not) after
    /// observing <see cref="JsonConst.ArrayEndByte"/>.
    /// <see langword="true"/> to ensure that no data is present after <see cref="JsonConst.ArrayEndByte"/>. However, both
    /// single line and multiline comments are allowed before <see cref="EoJ"/>.</param>
    /// <param name="token">Cancellation token to observe</param>
    /// <exception cref="JsonEx"></exception>
    public bool ReadIsEndArray(bool ensureEoj, Token token = default)
    {
        bool reply = ReadIsGivenByte(JsonConst.ArrayEndByte, token);
        if (ensureEoj && reply)
        {
            //we need to make sure only comments exists or we reached EOJ!
            SkipWhiteSpace(token);
            if (!EoJ)
            {
                throw new JsonEx("Expected End Of JSON after encountering ']'. " +
                                        $"Found = {(char)Current!}, " +
                                        $"0-Based Position = {Position}.");
            }
        }
        return reply;
    }

    /// <summary>
    /// Reads the current JSON element as <see cref="RawJson"/>. If reaches <see cref="EoJ"/> or
    /// encounters <see cref="JsonConst.ArrayEndByte"/>, returned <see cref="RawJson.Type"/> is
    /// <see cref="JsonType.Undefined"/>.
    /// <para>
    /// One should prefer <see cref="EnumerateJsonArray"/>
    /// to parse well-structured JSON stream over this method.
    /// This method is to parse non-standard chain of JSON elements separated by ',' (or not).
    /// </para>
    /// </summary>
    /// <param name="withVerify"><see langword="true"/> to verify the presence of ',' or ']' (but not ',]')
    /// after successfully parsing the current JSON element; <see langword="false"/> otherwise.</param>
    /// <param name="token">Cancellation token to observe.</param>
    /// <exception cref="JsonEx"></exception>
    public RawJson ReadRaw(bool withVerify = true, Token token = default)
    {
        SkipWhiteSpace(token);
        if (!InRange || _buffer[_current] == JsonConst.ArrayEndByte)
        {
            return new RawJson(JsonType.Undefined, Array.Empty<byte>());
        }

        ReDefineBuffer(0, token);
        JsonType type = SkipUntilNextRaw(token);
        byte[] currentRaw = new byte[_current - _begin];
        _buffer.CopyToUnSafe(currentRaw, _begin, currentRaw.Length, 0);

        if (withVerify)
        {
            ReadIsValueSeparationOrEndWithVerify(JsonConst.ArrayEndByte,
                    "array",
                    "',' or ']' (but not ',]')",
                    token);
        }
        else
        {
            _ = ReadIsGivenByte(JsonConst.ValueSeparatorByte, token);
        }
        return new RawJson(type, currentRaw);
    }

    private JsonType SkipUntilNextRaw(Token token)
    {
        return _buffer[_current] switch
        {
            JsonConst.ArrayBeginByte => SkipArray(token),
            JsonConst.ObjectBeginByte => SkipObject(token),
            JsonConst.StringQuoteByte => SkipString(token),
            JsonConst.MinusSignByte => SkipNumber(token),
            JsonConst.Number0Byte => SkipNumber(token),
            JsonConst.Number1Byte => SkipNumber(token),
            JsonConst.Number2Byte => SkipNumber(token),
            JsonConst.Number3Byte => SkipNumber(token),
            JsonConst.Number4Byte => SkipNumber(token),
            JsonConst.Number5Byte => SkipNumber(token),
            JsonConst.Number6Byte => SkipNumber(token),
            JsonConst.Number7Byte => SkipNumber(token),
            JsonConst.Number8Byte => SkipNumber(token),
            JsonConst.Number9Byte => SkipNumber(token),
            JsonConst.LowerCaseTCharacterByte => SkipRueOfTrue(token),
            JsonConst.LowerCaseFCharacterByte => SkipAlseOfFalse(token),
            JsonConst.LowerCaseNCharacterByte => SkipUllOfNull(token),
            _ => throw new JsonEx("Invalid byte value for start of JSON element. " +
                                                         $"Found = {(char)Current!}, " +
                                                         $"0-Based Position = {Position}.")
        };
    }

    private JsonType SkipArray(Token token)
    {
        if (NextWithEnsureCapacity(token))
        {
            SkipWhiteSpaceWithVerify("]", token);
            while (_buffer[_current] != JsonConst.ArrayEndByte)
            {
                _ = SkipUntilNextRaw(token);
                ReadIsValueSeparationOrEndWithVerify(JsonConst.ArrayEndByte,
                        "array",
                        "',' or ']' (but not ',]')",
                        token);
            }
            _ = NextWithEnsureCapacity(token);
            return JsonType.Arr;
        }
        throw new JsonEx("Reached end, unable to find valid JSON end-array (']'). " +
                                       $"0-Based Position = {Position}.");
    }

    private JsonType SkipObject(Token token)
    {
        if (NextWithEnsureCapacity(token))
        {
            SkipWhiteSpaceWithVerify("}", token);
            while (_buffer[_current] != JsonConst.ObjectEndByte)
            {
                if (_buffer[_current] != JsonConst.StringQuoteByte)
                {
                    throw new JsonEx("Invalid byte value for start of Object Property Name. " +
                        $"Expected = {(char)JsonConst.StringQuoteByte}, " +
                        $"Found = {(char)Current!}, " +
                        $"0-Based Position = {Position}.");
                }
                _ = SkipString(token);
                SkipWhiteSpaceWithVerify(":", token);
                _current--;
                NextExpectedOrThrow(JsonConst.NameSeparatorByte, "Object property", token);
                _ = NextWithEnsureCapacity(token);
                SkipWhiteSpaceWithVerify("Object property value", token);
                _ = SkipUntilNextRaw(token);
                ReadIsValueSeparationOrEndWithVerify(JsonConst.ObjectEndByte,
                        "Object property",
                        "',' or '}' (but not ',}')",
                        token);
            }
            _ = NextWithEnsureCapacity(token);
            return JsonType.Obj;
        }
        throw new JsonEx("Reached end, unable to find valid JSON end-object ('}}'). " +
                                       $"0-Based Position = {Position}.");
    }

    private JsonType SkipString(Token token)
    {
        while (NextWithEnsureCapacity(token))
        {
            switch (_buffer[_current])
            {
                case JsonConst.ReverseSlashByte:
                    if (NextWithEnsureCapacity(token))
                    {
                        switch (_buffer[_current])
                        {
                            case JsonConst.ReverseSlashByte:
                            case JsonConst.ForwardSlashByte:
                            case JsonConst.StringQuoteByte:
                            case JsonConst.LowerCaseBCharacterByte:
                            case JsonConst.LowerCaseFCharacterByte:
                            case JsonConst.LowerCaseNCharacterByte:
                            case JsonConst.LowerCaseTCharacterByte:
                            case JsonConst.LowerCaseRCharacterByte:
                                continue;
                            case JsonConst.LowerCaseUCharacterByte:
                                //JsonSerializer must handle validation!
                                //we skip 4 bytes
                                if (NextWithEnsureCapacity(token, 4))
                                {
                                    continue;
                                }
                                throw new JsonEx("Reached end, unable to find 4 characters after Hex escape \\u. " +
                                                               $"0-Based Position = {Position}.");
                            default:
                                throw new JsonEx("Bad JSON escape. " +
                                    $"Expected = \\{(char)JsonConst.ReverseSlashByte} or " +
                                    $"\\{(char)JsonConst.ForwardSlashByte} or " +
                                    $"\\{(char)JsonConst.StringQuoteByte} or " +
                                    $"\\{(char)JsonConst.LowerCaseBCharacterByte} or " +
                                    $"\\{(char)JsonConst.LowerCaseFCharacterByte} or " +
                                    $"\\{(char)JsonConst.LowerCaseNCharacterByte} or " +
                                    $"\\{(char)JsonConst.LowerCaseTCharacterByte} or " +
                                    $"\\{(char)JsonConst.LowerCaseRCharacterByte} or " +
                                    $"\\{(char)JsonConst.LowerCaseUCharacterByte}XXXX, " +
                                    $"Found = \\{(char)Current!}, " +
                                    $"0-Based Position = {Position}.");
                        }
                    }
                    throw new JsonEx("Reached end, unable to find valid escape character. " +
                                                   $"0-Based Position = {Position}.");
                case JsonConst.StringQuoteByte:
                    _ = NextWithEnsureCapacity(token);
                    return JsonType.Str;
            }
        }
        throw new JsonEx("Reached end, unable to find end-of-string quote '\"'. " +
                                       $"0-Based Position = {Position}.");
    }

    private JsonType SkipNumber(Token token)
    {
        //we just take everything until begin of next token (even if number is not valid!)
        //number parsing rules are too much to write here
        //Serializer will do its job during deserialization
        while (NextWithEnsureCapacity(token))
        {
            switch (_buffer[_current])
            {
                case JsonConst.MinusSignByte:
                case JsonConst.PlusSignByte:
                case JsonConst.ExponentLowerByte:
                case JsonConst.ExponentUpperByte:
                case JsonConst.DecimalPointByte:
                case JsonConst.Number0Byte:
                case JsonConst.Number1Byte:
                case JsonConst.Number2Byte:
                case JsonConst.Number3Byte:
                case JsonConst.Number4Byte:
                case JsonConst.Number5Byte:
                case JsonConst.Number6Byte:
                case JsonConst.Number7Byte:
                case JsonConst.Number8Byte:
                case JsonConst.Number9Byte:
                    continue;
                default: return JsonType.Num;
            }
        }
        return JsonType.Num;
    }

    private JsonType SkipRueOfTrue(Token token)
    {
        //JsonSerializer must handle literal validation!
        //we skip 3 bytes
        if (NextWithEnsureCapacity(token, 3))
        {
            //this one to move the pointer forward, we don't care
            //about EoF, that's handled by next read!
            _ = NextWithEnsureCapacity(token);
            return JsonType.Bool;
        }
        throw new JsonEx("Reached end while parsing 'true' literal. " +
                                                $"0-Based Position = {Position}.");
    }

    private JsonType SkipAlseOfFalse(Token token)
    {
        //JsonSerializer must handle literal validation!
        //we skip 4 bytes
        if (NextWithEnsureCapacity(token, 4))
        {
            //this one to move the pointer forward, we don't care
            //about EoF, that's handled by next read!
            _ = NextWithEnsureCapacity(token);
            return JsonType.Bool;
        }
        throw new JsonEx("Reached end while parsing 'false' literal. " +
                                                $"0-Based Position = {Position}.");
    }

    private JsonType SkipUllOfNull(Token token)
    {
        //JsonSerializer must handle literal validation!
        //we skip 3 bytes
        if (NextWithEnsureCapacity(token, 3))
        {
            //this one to move the pointer forward, we don't care
            //about EoF, that's handled by next read!
            _ = NextWithEnsureCapacity(token);
            return JsonType.Null;
        }
        throw new JsonEx("Reached end while parsing 'null' literal. " +
                                                $"0-Based Position = {Position}.");
    }

    private void ReadIsValueSeparationOrEndWithVerify(byte end, string partOf, string expected, Token token)
    {
        if (ReadIsValueSeparationOrEnd(end, token))
        {
            return;
        }

        if (InRange)
        {
            throw new JsonEx($"Invalid byte value for '{partOf}'. " +
                                                    $"Expected = {expected}, " +
                                                    $"Found = {(char)Current!}, " +
                                                    $"0-Based Position = {Position}.");
        }
        throw new JsonEx($"Reached end, unable to find '{(char)end}'. " +
                                                $"0-Based Position = {Position}.");
    }

    private bool ReadIsValueSeparationOrEnd(byte end, Token token)
    {
        SkipWhiteSpace(token);
        if (!InRange)
        {
            return false;
        }

        if (_buffer[_current] == end)
        {
            return true;
        }

        if (_buffer[_current] != JsonConst.ValueSeparatorByte)
        {
            return false;
        }

        _ = NextWithEnsureCapacity(token);
        SkipWhiteSpace(token);
        return InRange && _buffer[_current] != end;
    }

    private bool ReadIsGivenByte(byte match, Token token)
    {
        SkipWhiteSpace(token);
        if (!InRange || _buffer[_current] != match)
        {
            return false;
        }

        ReDefineBuffer(1, token);
        return true;
    }

    private void SkipWhiteSpaceWithVerify(string jsonToken, Token token)
    {
        SkipWhiteSpace(token);
        if (!InRange)
        {
            throw new JsonEx($"Reached end, expected to find '{jsonToken}'. " +
                                           $"0-Based Position = {Position}.");
        }
    }

    private void NextExpectedOrThrow(byte expected, string partOf, Token token)
    {
        if (NextWithEnsureCapacity(token) && _buffer[_current] == expected)
        {
            return;
        }
        throw new JsonEx($"Invalid byte value while parsing '{partOf}'. " +
                                                $"Expected = {expected}, " +
                                                $"Found = {(char)Current!}, " +
                                                $"0-Based Position = {Position}.");
    }

    private void SkipWhiteSpace(Token token)
    {
        while (EnsureCapacity(token))
        {
            switch (_buffer[_current])
            {
                case JsonConst.SpaceByte:
                case JsonConst.HorizontalTabByte:
                case JsonConst.NewLineByte:
                case JsonConst.CarriageReturnByte:
                    _ = NextWithEnsureCapacity(token);
                    continue;
                case JsonConst.ForwardSlashByte:
                    if (!NextWithEnsureCapacity(token))
                    {
                        throw new JsonEx("Reached end. " +
                                                       "Can not find correct comment format " +
                                                       "(neither single line comment token '//' " +
                                                       "nor multi-line comment token '/*').");
                    }
                    ReadComment(token);
                    continue;
                default: return;
            }
        }
    }

    private void ReadComment(Token token)
    {
        switch (_buffer[_current])
        {
            case JsonConst.ForwardSlashByte:
                while (NextWithEnsureCapacity(token))
                {
                    byte current = _buffer[_current];
                    if (current is not JsonConst.CarriageReturnByte and not JsonConst.NewLineByte)
                    {
                        continue;
                    }

                    _ = NextWithEnsureCapacity(token);
                    return;
                }
                //we don't throw if we reach EoJ, we consider comment ended there!
                //so we get out. If any other token was expected, further parsing will throw
                //proper error.
                break;
            case JsonConst.AsteriskByte:
                while (NextWithEnsureCapacity(token))
                {
                    if (_buffer[_current] != JsonConst.AsteriskByte)
                    {
                        continue;
                    }

                    if (NextWithEnsureCapacity(token) &&
                        _buffer[_current] == JsonConst.ForwardSlashByte)
                    {
                        _ = NextWithEnsureCapacity(token);
                        return;
                    }
                    _current--;
                }
                //we need to throw error even if we reached EoJ
                //coz the comment was not properly terminated!
                throw new JsonEx("Reached end. " +
                                               "Can not find end token of multi line comment(*/).");
            default:
                throw new JsonEx("Can not find correct comment format. " +
                    "Found single forward-slash '/' when expected " +
                    "either single line comment token '//' or multi-line comment token '/*'. " +
                    $"0-Based Position = {Position}.");
        }
    }

    private bool NextWithEnsureCapacity(Token token, int count = 1)
    {
        _current += count;
        return EnsureCapacity(token);
    }

    private bool EnsureCapacity(Token token)
    {
        return _current < _end || TryIncreasingBuffer(token);
    }

    private void ReDefineBuffer(int offsetIncrement, Token token)
    {
        IncreaseConsumption(offsetIncrement);
        if (_stream == null)
        {
            return;
        }

        if (_current > (_buffer.Length * 13 / 16))
        {
            _end = Math.Max(0, _end - _current);
            if (_end > 0)
            {
                _buffer.LiftNCopyUnSafe(_current, _end, 0);
            }

            _current = _begin = 0;
        }
        if (_end < _buffer.Length)
        {
            _ = FillBuffer(token);
        }
    }

    private bool TryIncreasingBuffer(Token token)
    {
        if (_stream == null)
        {
            return false;
        }

        if (_end == _buffer.Length || _current >= _buffer.Length)
        {
            _buffer = _buffer.DoubleByteCapacity();
        }

        return FillBuffer(token);
    }

    private bool FillBuffer(Token token)
    {
        //awaiting here kills the performance for the moment, we need to have a bounded capacity
        //stream (should go in DevFast.Net.Io module) otherwise we keep this sync version for the moment!
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        int end = _stream!.ReadAsync(_buffer.AsMemory(_end, _buffer.Length - _end), token).AsTask().GetAwaiter().GetResult();
#else
        int end = _stream!.ReadAsync(_buffer, _end, _buffer.Length - _end, token).GetAwaiter().GetResult();
#endif
        if (end == 0)
        {
            DisposeStream();
            return false;
        }

        _end += end;
        return _current < _end;
    }

    private void IncreaseConsumption(int offsetIncrement)
    {
        _current += offsetIncrement;
        _bytesConsumed += _current - _begin;
        _begin = _current;
    }

    /// <summary>
    /// Clean up by releasing resources.
    /// </summary>
    public void Dispose()
    {
        DisposeStream();
        _buffer = [];
    }

    private void DisposeStream()
    {
        if (_stream != null && _disposeStream)
        {
            _stream.Dispose();
        }

        _stream = null;
    }
}