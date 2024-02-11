using System.Dynamic;
using Dot.Net.DevFast.Extensions;

namespace DevFast.Net.Text.Tests.Json.Utf8
{
    internal class JsonArrayReaderTests
    {
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public async Task Works_Well_When_Stream_Is_Empty(bool withPreamble, bool disposeInner)
        {
            await using Stream m = TestHelper.GetReadableStreamWith("", withPreamble);
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
            await using Stream m = TestHelper.GetReadableStreamWith("[]", withPreamble);
            using (IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: disposeInner))
            {
                That(r.Capacity, Is.EqualTo(JsonConst.RawUtf8JsonPartReaderMinBuffer));
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
            await using Stream m = TestHelper.GetReadableStreamWith("[]");
            using (IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true))
            {
                That(r.EnumerateJsonArray(true).Count(), Is.EqualTo(0));
            }
            _ = Throws<ObjectDisposedException>(() => m.Write(new byte[] { 1 }));
        }

        [Test]
        public async Task EnumerateJsonArray_Returns_Data_When_Stream_Contains_At_Least_One_Element()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("[9]");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            List<RawJson> dataPoints = r.EnumerateJsonArray(true).ToList();
            Multiple(() =>
            {
                That(dataPoints, Has.Count.EqualTo(1));
                That(dataPoints[0].Type, Is.EqualTo(JsonType.Num));
                That(dataPoints[0].Value, Has.Length.EqualTo(1));
                That(dataPoints[0].Value[0], Is.EqualTo(JsonConst.Number9Byte));
            });
        }

        [Test]
        public async Task EnumerateJsonArray_Throws_Error_For_Undefined_Types()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("[");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException? err = Throws<JsonException>(() => r.EnumerateJsonArray(true).ToList())!;
            That(err, Is.Not.Null);
            That(err.Message, Is.EqualTo("Expected a valid JSON element or end of JSON array. 0-Based Position = 1."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ReadIsBeginArrayWithVerify_Throws_Error_When_Current_Location_Is_Not_Begin_Array_Character(bool keepRange)
        {
            await using Stream m = TestHelper.GetReadableStreamWith(keepRange ? "[]" : "[");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            r.ReadIsBeginArrayWithVerify();
            JsonException? err = Throws<JsonException>(() => r.ReadIsBeginArrayWithVerify())!;
            That(err, Is.Not.Null);
            That(err.Message, Is.EqualTo(keepRange ?
                "Invalid byte value for JSON begin-array. Expected = 91, Found = ], 0-Based Position = 1." :
                "Reached end, unable to find JSON begin-array. 0-Based Position = 1."));
        }

        [Test]
        public async Task ReadIsEndArray_Throws_Error_When_EoJ_Is_Required_But_Data_Is_Present()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("][");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException? err = Throws<JsonException>(() => r.ReadIsEndArray(true))!;
            That(err, Is.Not.Null);
            That(err.Message, Is.EqualTo("Expected End Of JSON after encountering ']'. Found = [, 0-Based Position = 1."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ReadRaw_Can_Read_Elements_WithOrWithout_Value_Separator(bool addValueSeparator)
        {
            await using Stream m = TestHelper.GetReadableStreamWith(addValueSeparator ? "9,8" : "9 8");
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
            await using Stream m = TestHelper.GetReadableStreamWith("u");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException? err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value for start of JSON element. Found = u, 0-Based Position = 0."));
            });
        }

        [Test]
        public async Task ReadRaw_Properly_Reads_Complex_Raw_Json()
        {
            await using Stream m = TestHelper.GetReadableStreamWith();
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
            await using Stream m = TestHelper.GetReadableStreamWith("{1]");
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
            await using Stream m = TestHelper.GetReadableStreamWith("\"\\\\\\/\\\"\\b\\f\\n\\t\\r\\u0000\"");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson str = r.ReadRaw(false);
            That(str.Type, Is.EqualTo(JsonType.Str));
        }

        [Test]
        public async Task ReadRaw_Throw_Error_When_String_Contains_Invalid_Escape_Character()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("\"\\1\"");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Bad JSON escape. Expected = \\\\ or \\/ or \\\" or \\b or \\f or \\n or \\t or \\r or \\uXXXX, Found = \\1, 0-Based Position = 2."));
            });

            await using Stream m1 = TestHelper.GetReadableStreamWith("\"\\");
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m1, CancellationToken.None, disposeStream: true);
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
            await using Stream m = TestHelper.GetReadableStreamWith("\"\\u0\"");
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
            await using Stream m = TestHelper.GetReadableStreamWith("\"0");
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
            await using Stream m = TestHelper.GetReadableStreamWith("tru");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end while parsing 'true' literal. 0-Based Position = 3."));
            });

            await using Stream m1 = TestHelper.GetReadableStreamWith("fals");
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m1, CancellationToken.None, disposeStream: true);
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
            await using Stream m = TestHelper.GetReadableStreamWith("nul");
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
            await using Stream m = TestHelper.GetReadableStreamWith("{\"a\":1");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find '}'. 0-Based Position = 6."));
            });

            await using Stream m1 = TestHelper.GetReadableStreamWith("[1");
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m1, CancellationToken.None, disposeStream: true);
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
            await using Stream m = TestHelper.GetReadableStreamWith("{\"a\":1 \"b\":2}");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Invalid byte value for 'Object property'. Expected = ',' or '}' (but not ',}'), Found = \", 0-Based Position = 7."));
            });

            await using Stream m1 = TestHelper.GetReadableStreamWith("[1 1]");
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m1, CancellationToken.None, disposeStream: true);
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
            await using Stream m = TestHelper.GetReadableStreamWith("{\"a\":1,");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find '}'. 0-Based Position = 7."));
            });

            await using Stream m1 = TestHelper.GetReadableStreamWith("{ ");
            using IJsonArrayReader r1 = await JsonReader.CreateUtf8ArrayReaderAsync(m1, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => r1.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, expected to find '}'. 0-Based Position = 2."));
            });

            await using Stream m11 = TestHelper.GetReadableStreamWith("{");
            using IJsonArrayReader r11 = await JsonReader.CreateUtf8ArrayReaderAsync(m11, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => r11.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find valid JSON end-object ('}}'). 0-Based Position = 1."));
            });

            await using Stream m2 = TestHelper.GetReadableStreamWith("[1,");
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m2, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find ']'. 0-Based Position = 3."));
            });

            await using Stream m3 = TestHelper.GetReadableStreamWith("[");
            using IJsonArrayReader rr1 = await JsonReader.CreateUtf8ArrayReaderAsync(m3, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr1.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, unable to find valid JSON end-array (']'). 0-Based Position = 1."));
            });

            await using Stream m33 = TestHelper.GetReadableStreamWith("[ ");
            using IJsonArrayReader rr3 = await JsonReader.CreateUtf8ArrayReaderAsync(m33, CancellationToken.None, disposeStream: true);
            err = Throws<JsonException>(() => rr3.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Reached end, expected to find ']'. 0-Based Position = 2."));
            });
        }

        [Test]
        public async Task ReadRaw_Throws_Error_When_Object_Does_Not_Have_Name_Separator()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("{\"a\" 1}");
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
            await using Stream m = TestHelper.GetReadableStreamWith("{\"a\":1,\"b\":2}");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d = r.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Obj));

            await using Stream m1 = TestHelper.GetReadableStreamWith("[1,1]");
            using IJsonArrayReader rr = await JsonReader.CreateUtf8ArrayReaderAsync(m1, CancellationToken.None, disposeStream: true);
            d = rr.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Arr));
        }

        [Test]
        public async Task ReadRaw_Throws_Error_For_Mal_Formed_Comment()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("{/");
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
            await using Stream m = TestHelper.GetReadableStreamWith("//");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d = r.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Undefined));
        }

        [Test]
        public async Task ReadRaw_Passes_For_MultiLine_Comment_Having_More_Asterisks_As_Needed()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("/***/");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            RawJson d = r.ReadRaw(false);
            That(d.Type, Is.EqualTo(JsonType.Undefined));
        }

        [Test]
        public async Task ReadRaw_Throws_Error_If_MultiLine_Comment_Not_Properly_Closed_At_End_Of_Data()
        {
            await using Stream m = TestHelper.GetReadableStreamWith("/*");
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
            await using Stream m = TestHelper.GetReadableStreamWith("/my");
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(m, CancellationToken.None, disposeStream: true);
            JsonException err = Throws<JsonException>(() => r.ReadRaw(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Can not find correct comment format. Found single forward-slash '/' when expected either single line comment token '//' or multi-line comment token '/*'. 0-Based Position = 1."));
            });
        }

        [Test]
        public async Task HugeArray_Parsing_Without_A_Hitch()
        {
            await using FileStream f = new(Guid.NewGuid().ToString() + ".json",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.None,
                4096,
                FileOptions.SequentialScan | FileOptions.Asynchronous | FileOptions.DeleteOnClose);
            await using StreamWriter w = new(f, new UTF8Encoding(false), 4096, true);
            w.Write("[");
            Enumerable.Range(0, 1000).ForEach(_ =>
            {
                w.Write(TestHelper.ComplexJson);
                w.Write(",");
            });
            w.Write(TestHelper.ComplexJson);
            w.Write("]");
            w.Flush();
            _ = f.Seek(0, SeekOrigin.Begin);
            using IJsonArrayReader r = await JsonReader.CreateUtf8ArrayReaderAsync(f, CancellationToken.None, disposeStream: false);
            That(r.EnumerateJsonArray(true).Count(), Is.EqualTo(1001));
        }
    }
}
