using System.Text.Json;
using DevFast.Net.Extensions.SystemTypes;

namespace DevFast.Net.Text.Json.Utf8
{
    /// <summary>
    /// Class implementing <see cref="IJsonArrayReader"/> for standard Utf-8 JSON data encoding
    /// based on https://datatracker.ietf.org/doc/html/rfc7159 (grammar shown at https://www.json.org/json-en.html).
    /// <para>
    /// This implementation support both single line comments (starting with '//' and ending in either Carriage return '\r'
    /// or newline '\n') and multiline comments (starting with '/*' and ending with '*/').
    /// </para>
    /// </summary>
    public sealed class MemJsonArrayReader : IJsonArrayReader
    {
        private readonly bool _disposeStream;
        private readonly int _end;
        private MemoryStream? _stream;
        private byte[] _buffer;
        private int _current;

        internal MemJsonArrayReader(MemoryStream stream, ArraySegment<byte> internalSegment, bool disposeStream)
        {
            _stream = stream;
            _buffer = internalSegment.Array!;
            _current = internalSegment.Offset;
            _end = internalSegment.Offset + internalSegment.Count;
            _disposeStream = disposeStream;
        }

        /// <summary>
        /// <see langword="true"/> indicating that reader has reached end of JSON input,
        /// otherwise <see langword="false"/>.
        /// </summary>
        public bool EoJ => !InRange;

        /// <summary>
        /// <see cref="byte"/> value of current position of reader. <see langword="null"/> when
        /// reader has reached <see cref="EoJ"/>.
        /// </summary>
        public byte? Current => EoJ ? null : _buffer[_current];

        /// <summary>
        /// Total number of <see cref="byte"/>s observed by the reader since the very beginning (0-based position).
        /// </summary>
        public long Position => Math.Min(_current, _end);

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
        /// <exception cref="JsonException"></exception>
        public IEnumerable<RawJson> EnumerateJsonArray(bool ensureEoj,
            CancellationToken token = default)
        {
            ReadIsBeginArrayWithVerify(token);
            while (!ReadIsEndArray(ensureEoj, token))
            {
                RawJson next = ReadRaw(true, token);
                yield return next.Type == JsonType.Undefined
                    ? throw new JsonException("Expected a valid JSON element or end of JSON array. " +
                                                            $"0-Based Position = {Position}.")
                    : next;
            }
        }

        /// <summary>
        /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it checks
        /// if value is <see cref="JsonConst.ArrayBeginByte"/>. If the value matches, then reader advances
        /// its current position to next <see cref="byte"/> in the sequence or to end of JSON. If the value does NOT match,
        /// reader position is maintained on the current byte and an error
        /// (of type <see cref="JsonException"/>) is thrown.
        /// </summary>
        /// <param name="token">Cancellation token to observe</param>
        /// <exception cref="JsonException"></exception>
        public void ReadIsBeginArrayWithVerify(CancellationToken token = default)
        {
            if (!ReadIsBeginArray(token))
            {
                if (InRange)
                {
                    throw new JsonException("Invalid byte value for JSON begin-array. " +
                                                            $"Expected = {JsonConst.ArrayBeginByte}, " +
                                                            $"Found = {(char)Current!}, " +
                                                            $"0-Based Position = {Position}.");
                }
                throw new JsonException("Reached end, unable to find JSON begin-array. " +
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
        public bool ReadIsBeginArray(CancellationToken token = default)
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
        /// <exception cref="JsonException"></exception>
        public bool ReadIsEndArray(bool ensureEoj, CancellationToken token = default)
        {
            bool reply = ReadIsGivenByte(JsonConst.ArrayEndByte, token);
            if (ensureEoj && reply)
            {
                //we need to make sure only comments exists or we reached EOJ!
                SkipWhiteSpace();
                if (!EoJ)
                {
                    throw new JsonException("Expected End Of JSON after encountering ']'. " +
                                            $"Found = {(char)Current!}, " +
                                            $"0-Based Position = {Position}.");
                }
            }
            return reply;
        }

        /// <summary>
        /// Reads the current JSON element as <see cref="RawJson"/>. If it reaches <see cref="EoJ"/> or
        /// encounters <see cref="JsonConst.ArrayEndByte"/>, it returns <see cref="JsonType.Undefined"/> as
        /// <see cref="RawJson.Type"/>.
        /// <para>
        /// One should prefer <see cref="EnumerateJsonArray"/> to parse well-structured JSON stream over this method.
        /// This method is to parse non-standard chain of JSON elements separated by ',' (or not).
        /// </para>
        /// </summary>
        /// <param name="withVerify"><see langword="true"/> to verify the presence of ',' or ']' (but not ',]')
        /// after successfully parsing the current JSON element; <see langword="false"/> otherwise.</param>
        /// <param name="token">Cancellation token to observe.</param>
        /// <exception cref="JsonException"></exception>
        public RawJson ReadRaw(bool withVerify = true, CancellationToken token = default)
        {
            SkipWhiteSpace();
            if (!InRange || _buffer[_current] == JsonConst.ArrayEndByte)
            {
                return new RawJson(JsonType.Undefined, []);
            }

            int begin = _current;
            JsonType type = SkipUntilNextRaw();
            token.ThrowIfCancellationRequested();
            byte[] currentRaw = new byte[_current - begin];
            _buffer.CopyToUnSafe(currentRaw, begin, currentRaw.Length, 0);

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

        private JsonType SkipUntilNextRaw()
        {
            return _buffer[_current] switch
            {
                JsonConst.ArrayBeginByte => SkipArray(),
                JsonConst.ObjectBeginByte => SkipObject(),
                JsonConst.StringQuoteByte => SkipString(),
                JsonConst.MinusSignByte => SkipNumber(),
                JsonConst.Number0Byte => SkipNumber(),
                JsonConst.Number1Byte => SkipNumber(),
                JsonConst.Number2Byte => SkipNumber(),
                JsonConst.Number3Byte => SkipNumber(),
                JsonConst.Number4Byte => SkipNumber(),
                JsonConst.Number5Byte => SkipNumber(),
                JsonConst.Number6Byte => SkipNumber(),
                JsonConst.Number7Byte => SkipNumber(),
                JsonConst.Number8Byte => SkipNumber(),
                JsonConst.Number9Byte => SkipNumber(),
                JsonConst.FirstOfTrueByte => SkipRueOfTrue(),
                JsonConst.FirstOfFalseByte => SkipAlseOfFalse(),
                JsonConst.FirstOfNullByte => SkipUllOfNull(),
                _ => throw new JsonException("Invalid byte value for start of JSON element. " +
                                                             $"Found = {(char)Current!}, " +
                                                             $"0-Based Position = {Position}.")
            };
        }

        private JsonType SkipArray()
        {
            _current++;
            SkipWhiteSpaceWithVerify("]");
            while (_buffer[_current] != JsonConst.ArrayEndByte)
            {
                _ = SkipUntilNextRaw();
                ReadIsValueSeparationOrEndWithVerify(JsonConst.ArrayEndByte,
                        "array",
                        "',' or ']' (but not ',]')",
                        default);
            }
            _ = NextWithEnsureCapacity();
            return JsonType.Arr;
        }

        private JsonType SkipObject()
        {
            _current++;
            SkipWhiteSpaceWithVerify("}");
            while (_buffer[_current] != JsonConst.ObjectEndByte)
            {
                if (_buffer[_current] != JsonConst.StringQuoteByte)
                {
                    throw new JsonException("Invalid byte value for start of Object Property Name. " +
                                                            $"Expected = {(char)JsonConst.StringQuoteByte}, " +
                                                            $"Found = {(char)Current!}, " +
                                                            $"0-Based Position = {Position}.");
                }
                _ = SkipString();
                SkipWhiteSpaceWithVerify(":");
                _current--;
                NextExpectedOrThrow(JsonConst.NameSeparatorByte, "Object property");
                _current++;
                SkipWhiteSpaceWithVerify("Object property value");
                _ = SkipUntilNextRaw();
                ReadIsValueSeparationOrEndWithVerify(JsonConst.ObjectEndByte,
                        "Object property",
                        "',' or '}' (but not ',}')",
                        default);
            }
            _ = NextWithEnsureCapacity();
            return JsonType.Obj;
        }

        private JsonType SkipString()
        {
            while (NextWithEnsureCapacity())
            {
                switch (_buffer[_current])
                {
                    case JsonConst.ReverseSlashByte:
                        if (NextWithEnsureCapacity())
                        {
                            switch (_buffer[_current])
                            {
                                case JsonConst.ReverseSlashByte:
                                case JsonConst.ForwardSlashByte:
                                case JsonConst.StringQuoteByte:
                                case JsonConst.LastOfBackspaceInStringByte:
                                case JsonConst.FirstOfFalseByte:
                                case JsonConst.FirstOfNullByte:
                                case JsonConst.FirstOfTrueByte:
                                case JsonConst.LastOfCarriageReturnInStringByte:
                                    continue;
                                case JsonConst.SecondOfHexDigitInStringByte:
                                    //JsonSerializer must handle validation!
                                    //we skip 4 bytes
                                    if (NextWithEnsureCapacity(4))
                                    {
                                        continue;
                                    }
                                    throw new JsonException("Reached end, unable to find 4 characters after Hex escape \\u. " +
                                                                   $"0-Based Position = {Position}.");
                                default:
                                    throw new JsonException("Bad JSON escape. " +
                                        $"Expected = \\{(char)JsonConst.ReverseSlashByte} or " +
                                        $"\\{(char)JsonConst.ForwardSlashByte} or " +
                                        $"\\{(char)JsonConst.StringQuoteByte} or " +
                                        $"\\{(char)JsonConst.LastOfBackspaceInStringByte} or " +
                                        $"\\{(char)JsonConst.FirstOfFalseByte} or " +
                                        $"\\{(char)JsonConst.FirstOfNullByte} or " +
                                        $"\\{(char)JsonConst.FirstOfTrueByte} or " +
                                        $"\\{(char)JsonConst.LastOfCarriageReturnInStringByte} or " +
                                        $"\\{(char)JsonConst.SecondOfHexDigitInStringByte}XXXX, " +
                                        $"Found = \\{(char)Current!}, " +
                                        $"0-Based Position = {Position}.");
                            }
                        }
                        throw new JsonException("Reached end, unable to find valid escape character. " +
                                                       $"0-Based Position = {Position}.");
                    case JsonConst.StringQuoteByte:
                        _ = NextWithEnsureCapacity();
                        return JsonType.Str;
                }
            }
            throw new JsonException("Reached end, unable to find end-of-string quote '\"'. " +
                                           $"0-Based Position = {Position}.");
        }

        private JsonType SkipNumber()
        {
            //we just take everything until begin of next token (even if number is not valid!)
            //number parsing rules are too much to write here
            //Serializer will do its job during deserialization
            while (NextWithEnsureCapacity())
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

        private JsonType SkipRueOfTrue()
        {
            //JsonSerializer must handle literal validation!
            //we skip 3 bytes
            if (NextWithEnsureCapacity(3))
            {
                //this one to move the pointer forward, we don't care
                //about EoF, that's handled by next read!
                _ = NextWithEnsureCapacity();
                return JsonType.Bool;
            }
            throw new JsonException("Reached end while parsing 'true' literal. " +
                                                    $"0-Based Position = {Position}.");
        }

        private JsonType SkipAlseOfFalse()
        {
            //JsonSerializer must handle literal validation!
            //we skip 4 bytes
            if (NextWithEnsureCapacity(4))
            {
                //this one to move the pointer forward, we don't care
                //about EoF, that's handled by next read!
                _ = NextWithEnsureCapacity();
                return JsonType.Bool;
            }
            throw new JsonException("Reached end while parsing 'false' literal. " +
                                                    $"0-Based Position = {Position}.");
        }

        private JsonType SkipUllOfNull()
        {
            //JsonSerializer must handle literal validation!
            //we skip 3 bytes
            if (NextWithEnsureCapacity(3))
            {
                //this one to move the pointer forward, we don't care
                //about EoF, that's handled by next read!
                _ = NextWithEnsureCapacity();
                return JsonType.Null;
            }
            throw new JsonException("Reached end while parsing 'null' literal. " +
                                                    $"0-Based Position = {Position}.");
        }

        private void ReadIsValueSeparationOrEndWithVerify(byte end, string partOf, string expected, CancellationToken token)
        {
            if (ReadIsValueSeparationOrEnd(end, token))
            {
                return;
            }

            if (InRange)
            {
                throw new JsonException($"Invalid byte value for '{partOf}'. " +
                                                        $"Expected = {expected}, " +
                                                        $"Found = {(char)Current!}, " +
                                                        $"0-Based Position = {Position}.");
            }
            throw new JsonException($"Reached end, unable to find '{(char)end}'. " +
                                                    $"0-Based Position = {Position}.");
        }

        private bool ReadIsValueSeparationOrEnd(byte end, CancellationToken token)
        {
            SkipWhiteSpace();
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

            _current++;
            SkipWhiteSpace();
            token.ThrowIfCancellationRequested();
            return InRange && _buffer[_current] != end;
        }

        private bool ReadIsGivenByte(byte match, CancellationToken token)
        {
            SkipWhiteSpace();
            token.ThrowIfCancellationRequested();
            if (!InRange || _buffer[_current] != match)
            {
                return false;
            }

            _current++;
            return true;
        }

        private void SkipWhiteSpaceWithVerify(string jsonToken)
        {
            SkipWhiteSpace();
            if (!InRange)
            {
                throw new JsonException($"Reached end, expected to find '{jsonToken}'. " +
                                                        $"0-Based Position = {Position}.");
            }
        }

        private void NextExpectedOrThrow(byte expected, string partOf)
        {
            if (NextWithEnsureCapacity() && _buffer[_current] == expected)
            {
                return;
            }

            throw new JsonException($"Invalid byte value while parsing '{partOf}'. " +
                                    $"Expected = {expected}, " +
                                    $"Found = {(char)Current!}, " +
                                    $"0-Based Position = {Position}.");
        }

        private void SkipWhiteSpace()
        {
            while (InRange)
            {
                switch (_buffer[_current])
                {
                    case JsonConst.SpaceByte:
                    case JsonConst.HorizontalTabByte:
                    case JsonConst.NewLineByte:
                    case JsonConst.CarriageReturnByte:
                        _ = NextWithEnsureCapacity();
                        continue;
                    case JsonConst.ForwardSlashByte:
                        if (!NextWithEnsureCapacity())
                        {
                            throw new JsonException("Reached end. " +
                                                    "Can not find correct comment format " +
                                                    "(neither single line comment token '//' " +
                                                    "nor multi-line comment token '/*').");
                        }
                        ReadComment();
                        continue;
                    default: return;
                }
            }
        }

        private void ReadComment()
        {
            switch (_buffer[_current])
            {
                case JsonConst.ForwardSlashByte:
                    while (NextWithEnsureCapacity())
                    {
                        byte current = _buffer[_current];
                        if (current is not JsonConst.CarriageReturnByte and not JsonConst.NewLineByte)
                        {
                            continue;
                        }

                        _ = NextWithEnsureCapacity();
                        return;
                    }
                    //we don't throw if we reach EoJ, we consider comment ended there!
                    //so we get out. If any other token was expected, further parsing will throw
                    //proper error.
                    break;
                case JsonConst.AsteriskByte:
                    while (NextWithEnsureCapacity())
                    {
                        if (_buffer[_current] != JsonConst.AsteriskByte)
                        {
                            continue;
                        }

                        if (NextWithEnsureCapacity() &&
                            _buffer[_current] == JsonConst.ForwardSlashByte)
                        {
                            _ = NextWithEnsureCapacity();
                            return;
                        }
                        _current--;
                    }
                    //we need to throw error even if we reached EoJ
                    //coz the comment was not properly terminated!
                    throw new JsonException("Reached end. Can not find end token of multi line comment(*/).");
                default:
                    throw new JsonException("Can not find correct comment format. " +
                        "Found single forward-slash '/' when expected " +
                        "either single line comment token '//' or multi-line comment token '/*'. " +
                        $"0-Based Position = {Position}.");
            }
        }

        private bool NextWithEnsureCapacity(int count = 1)
        {
            _current += count;
            return InRange;
        }

        /// <summary>
        /// Asynchronous clean up by releasing resources.
        /// </summary>
        public void Dispose()
        {
            if (_stream != null && _disposeStream)
            {
                _stream.Dispose();
            }

            _stream = null;
            _buffer = [];
        }
    }
}