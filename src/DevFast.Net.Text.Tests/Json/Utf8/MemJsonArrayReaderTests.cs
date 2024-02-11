using System.Dynamic;

namespace DevFast.Net.Text.Tests.Json.Utf8
{
    [TestFixture]
    public class MemJsonArrayReaderTests
    {
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public async Task Works_Well_When_Stream_Is_Empty(bool withPreamble, bool disposeInner)
        {
            MemoryStream m = new();
            UTF8Encoding e = new(withPreamble);
            await m.WriteAsync(e.GetPreamble());
            _ = m.Seek(0, SeekOrigin.Begin);
            using (IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: disposeInner))
            {
                Multiple(() =>
                {
                    That(r.ReadIsBeginArray(), Is.False);
                    That(r.ReadIsEndArray(false), Is.False);
                    RawJson current = r.ReadRaw(default);
                    That(current.Value, Is.Empty);
                    That(current.Type, Is.EqualTo(JsonType.Undefined));
                    That(r.EoJ, Is.True);
                    That(r.Current, Is.Null);
                    That(r.Position, Is.EqualTo(withPreamble ? 3 : 0));
                });
            }
            if (disposeInner)
            {
                _ = Throws<ObjectDisposedException>(() => m.Write(new byte[] { 1 }));
            }
            else
            {
                That(m.Length, Is.EqualTo(withPreamble ? 3 : 0));
            }
        }

        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public async Task Works_Well_When_Stream_Contains_Empty_Array(bool withPreamble, bool disposeInner)
        {
            MemoryStream m = new();
            UTF8Encoding e = new(withPreamble);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[] { JsonConst.ArrayBeginByte, JsonConst.ArrayEndByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using (IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: disposeInner))
            {
                That(r.Capacity, Is.EqualTo(m.Capacity));
                That(r.ReadIsBeginArray(), Is.True);
                That(r.ReadIsBeginArray(), Is.False);
                RawJson current = r.ReadRaw(default);
                Multiple(() =>
                {
                    That(current.Value, Is.Empty);
                    That(current.Type, Is.EqualTo(JsonType.Undefined));
                    That(r.EoJ, Is.False);
                    That(r.Current, Is.EqualTo(JsonConst.ArrayEndByte));
                    That(r.ReadIsEndArray(true), Is.True);
                    That(r.Current, Is.Null);
                    That(r.ReadIsEndArray(false), Is.False);
                    That(r.EoJ, Is.True);
                    That(r.Position, Is.EqualTo(withPreamble ? 5 : 2));
                });
            }
            if (disposeInner)
            {
                _ = Throws<ObjectDisposedException>(() => m.Write(new byte[] { 1 }));
            }
            else
            {
                That(m.Length, Is.EqualTo(withPreamble ? 5 : 2));
            }
        }

        [Test]
        public async Task EnumerateJsonArray_Has_No_Data_When_Stream_Contains_Empty_Array()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[] { JsonConst.ArrayBeginByte, JsonConst.ArrayEndByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using (IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true))
            {
                That(r.EnumerateJsonArray(true).Count(), Is.EqualTo(0));
            }
            _ = Throws<ObjectDisposedException>(() => m.Write(new byte[] { 1 }));
        }

        [Test]
        public async Task EnumerateJsonArray_Returns_Data_When_Stream_Contains_At_Least_One_Element()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[] { JsonConst.ArrayBeginByte, JsonConst.Number9Byte, JsonConst.ArrayEndByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            List<RawJson> dataPoints = r.EnumerateJsonArray(true).ToList();
            That(dataPoints, Has.Count.EqualTo(1));
            That(dataPoints[0].Type, Is.EqualTo(JsonType.Num));
            That(dataPoints[0].Value, Has.Length.EqualTo(1));
            That(dataPoints[0].Value[0], Is.EqualTo(JsonConst.Number9Byte));
        }

        [Test]
        public async Task EnumerateJsonArray_Throws_Error_For_Undefined_Types()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[] { JsonConst.ArrayBeginByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.EnumerateJsonArray(true).ToList())!;
            That(err, Is.Not.Null);
            That(err.Message, Is.EqualTo("Expected a valid JSON element or end of JSON array. 0-Based Position = 1."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ReadIsBeginArrayWithVerify_Throws_Error_When_Current_Location_Is_Not_Begin_Array_Character(bool keepRange)
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(keepRange ? new[] { JsonConst.ArrayBeginByte, JsonConst.ArrayEndByte } : new[] { JsonConst.ArrayBeginByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            r.ReadIsBeginArrayWithVerify();
            JsonException err = Throws<JsonException>(() => r.ReadIsBeginArrayWithVerify())!;
            That(err, Is.Not.Null);
            That(err.Message, Is.EqualTo(keepRange ?
                "Invalid byte value for JSON begin-array. Expected = 91, Found = ], 0-Based Position = 1." :
                "Reached end, unable to find JSON begin-array. 0-Based Position = 1."));
        }

        [Test]
        public async Task ReadIsEndArray_Throws_Error_When_EoJ_Is_Required_But_Data_Is_Present()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[] { JsonConst.ArrayEndByte, JsonConst.ArrayBeginByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadIsEndArray(true))!;
            That(err, Is.Not.Null);
            That(err.Message, Is.EqualTo("Expected End Of JSON after encountering ']'. Found = [, 0-Based Position = 1."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ReadRaw_Can_Read_Elements_WithOrWithout_Value_Separator(bool addValueSeparator)
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(addValueSeparator ?
                new[] { JsonConst.Number9Byte, JsonConst.ValueSeparatorByte, JsonConst.Number8Byte } :
                new[] { JsonConst.Number9Byte, JsonConst.SpaceByte, JsonConst.Number8Byte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d1 = r.ReadRaw(false);
            RawJson d2 = r.ReadRaw(false);
            RawJson d3 = r.ReadRaw(false);
            Multiple(() =>
            {
                That(d1.Type, Is.EqualTo(JsonType.Num));
                That(d2.Type, Is.EqualTo(JsonType.Num));
                That(d3.Type, Is.EqualTo(JsonType.Undefined));
                That(d1.Value[0], Is.EqualTo(JsonConst.Number9Byte));
                That(d2.Value[0], Is.EqualTo(JsonConst.Number8Byte));
            });
        }

        [Test]
        public async Task ReadRaw_Throws_Error_If_Data_Has_Invalid_Format()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(true);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[] { JsonConst.SecondOfHexDigitInStringByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value for start of JSON element. Found = u, 0-Based Position = 3."));
            });
        }

        [Test]
        public async Task ReadRaw_Properly_Reads_Complex_Raw_Json()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(true);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(TestHelper.ComplexRawJson());
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson raw = r.ReadRaw(false);
            That(raw.Type, Is.EqualTo(JsonType.Obj));
            TestHelper.ValidateObjectOfComplexRawJson(JsonSerializer.Deserialize<ExpandoObject>(raw.Value, new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            })!);
        }

        [Test]
        public async Task ReadRaw_Throws_Error_For_Invalid_Object()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{1} => invalid object
            await m.WriteAsync(new[] { JsonConst.ObjectBeginByte, JsonConst.Number1Byte, JsonConst.ArrayEndByte });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value for start of Object Property Name. Expected = \", Found = 1, 0-Based Position = 1."));
            });
        }

        [Test]
        public async Task ReadRaw_Passes_When_String_Contains_WhiteSpace_Characters_Or_Hex()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                JsonConst.StringQuoteByte,
                JsonConst.ReverseSlashByte,
                JsonConst.ReverseSlashByte,
                JsonConst.ReverseSlashByte,
                JsonConst.ForwardSlashByte,
                JsonConst.ReverseSlashByte,
                JsonConst.StringQuoteByte,
                JsonConst.ReverseSlashByte,
                JsonConst.LastOfBackspaceInStringByte,
                JsonConst.ReverseSlashByte,
                JsonConst.FirstOfFalseByte,
                JsonConst.ReverseSlashByte,
                JsonConst.FirstOfNullByte,
                JsonConst.ReverseSlashByte,
                JsonConst.FirstOfTrueByte,
                JsonConst.ReverseSlashByte,
                JsonConst.LastOfCarriageReturnInStringByte,
                JsonConst.ReverseSlashByte,
                JsonConst.SecondOfHexDigitInStringByte,
                JsonConst.Number0Byte,
                JsonConst.Number0Byte,
                JsonConst.Number0Byte,
                JsonConst.Number0Byte,
                JsonConst.StringQuoteByte
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson str = r.ReadRaw(false);
            That(str.Type, Is.EqualTo(JsonType.Str));
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_String_Contains_Invalid_Escape_Character()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                JsonConst.StringQuoteByte,
                JsonConst.ReverseSlashByte,
                JsonConst.Number1Byte,
                JsonConst.StringQuoteByte
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Bad JSON escape. Expected = \\\\ or \\/ or \\\" or \\b or \\f or \\n or \\t or \\r or \\uXXXX, Found = \\1, 0-Based Position = 2."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                JsonConst.StringQuoteByte,
                JsonConst.ReverseSlashByte,
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find valid escape character. 0-Based Position = 2."));
            });
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_HexEscape_DoesNot_Followed_By_4_Characters()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                JsonConst.StringQuoteByte,
                JsonConst.ReverseSlashByte,
                JsonConst.SecondOfHexDigitInStringByte,
                JsonConst.Number0Byte,
                JsonConst.StringQuoteByte
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find 4 characters after Hex escape \\u. 0-Based Position = 5."));
            });
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_String_Is_Not_Terminated_Before_End()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                JsonConst.StringQuoteByte,
                JsonConst.Number0Byte
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find end-of-string quote '\"'. 0-Based Position = 2."));
            });
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_True_Or_False_Are_Not_Terminated_Before_End()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'t',
                (byte)'r',
                (byte)'u' //missing 'e'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end while parsing 'true' literal. 0-Based Position = 3."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'f',
                (byte)'a',
                (byte)'l',
                (byte)'s' //missing 'e'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end while parsing 'false' literal. 0-Based Position = 4."));
            });
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_Null_Is_Not_Terminated_Before_End()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'n',
                (byte)'u',
                (byte)'l' //missing 'l'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end while parsing 'null' literal. 0-Based Position = 3."));
            });
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_Array_Or_Object_Is_Not_Well_Terminated()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)'"',
                (byte)'a',
                (byte)'"',
                (byte)':',
                (byte)'1' //not closing '}'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find '}'. 0-Based Position = 6."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'[',
                (byte)'1' //not closing ']'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find ']'. 0-Based Position = 2."));
            });
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_Array_Or_Object_Is_Not_Well_Formatted()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)'"',
                (byte)'a',
                (byte)'"',
                (byte)':',
                (byte)'1',
                (byte)' ',//forgot ',' between 1 and next field's 
                (byte)'"',
                (byte)'b',
                (byte)'"',
                (byte)':',
                (byte)'2',
                (byte)'}'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value for 'Object property'. Expected = ',' or '}' (but not ',}'), Found = \", 0-Based Position = 7."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'[',
                (byte)'1',
                (byte)' ',//forgot ',' between 1 and next value
                (byte)'1',
                (byte)']'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value for 'array'. Expected = ',' or ']' (but not ',]'), Found = 1, 0-Based Position = 3."));
            });
        }

        [Test]
        public async Task ReadRaw_Throws_Error_When_Array_Or_Object_Is_Not_Well_Defined()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)'"',
                (byte)'a',
                (byte)'"',
                (byte)':',
                (byte)'1',
                (byte)','
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find '}'. 0-Based Position = 7."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r1 = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => r1.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, expected to find '}'. 0-Based Position = 1."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)' '
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r21 = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => r21.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, expected to find '}'. 0-Based Position = 2."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'[',
                (byte)'1',
                (byte)','
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find ']'. 0-Based Position = 3."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'['
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr1 = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr1.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, expected to find ']'. 0-Based Position = 1."));
            });

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'[',
                (byte)' '
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr2 = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr2.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, expected to find ']'. 0-Based Position = 2."));
            });
        }

        [Test]
        public async Task ReadRaw_Throws_Error_When_Object_Does_Not_Have_Name_Separator()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)'"',
                (byte)'a',
                (byte)'"',
                (byte)' ',// missed ':'
                (byte)'1',
                (byte)'}'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value while parsing 'Object property'. Expected = 58, Found = 1, 0-Based Position = 5."));
            });
        }

        [Test]
        public async Task ReadRaw_Passes_When_Array_Or_Object_Is_Well_Defined()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)'"',
                (byte)'a',
                (byte)'"',
                (byte)':',
                (byte)'1',
                (byte)',',
                (byte)'"',
                (byte)'b',
                (byte)'"',
                (byte)':',
                (byte)'2',
                (byte)'}'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d = r.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Obj));

            m = new();
            e = new(false);
            await m.WriteAsync(e.GetPreamble());
            //{\\a\n} => objectwith comments
            await m.WriteAsync(new[]
            {
                (byte)'[',
                (byte)'1',
                (byte)',',//forgot ',' between 1 and next value
                (byte)'1',
                (byte)']'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            d = rr.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Arr));
        }

        [Test]
        public async Task ReadRaw_Throws_Error_For_Mal_Formed_Comment()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'{',
                (byte)'/'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end. Can not find correct comment format (neither single line comment token '//' nor multi-line comment token '/*')."));
            });
        }

        [Test]
        public async Task ReadRaw_Passes_For_Empty_Comment()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'/',
                (byte)'/'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d = r.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Undefined));
        }

        [Test]
        public async Task ReadRaw_Passes_For_MultiLine_Comment_Having_More_Asterisks_As_Needed()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'/',
                (byte)'*',
                (byte)'*',
                (byte)'*',
                (byte)'/'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d = r.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Undefined));
        }

        [Test]
        public async Task ReadRaw_Throws_Error_If_MultiLine_Comment_Not_Properly_Closed_At_End_Of_Data()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'/',
                (byte)'*'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end. Can not find end token of multi line comment(*/)."));
            });
        }

        [Test]
        public async Task ReadRaw_Throws_Error_For_Invalid_Comment_Opening()
        {
            MemoryStream m = new();
            UTF8Encoding e = new(false);
            await m.WriteAsync(e.GetPreamble());
            await m.WriteAsync(new[]
            {
                (byte)'/',
                (byte)'m',
                (byte)'y'
            });
            _ = m.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Can not find correct comment format. Found single forward-slash '/' when expected either single line comment token '//' or multi-line comment token '/*'. 0-Based Position = 1."));
            });
        }
    }
}