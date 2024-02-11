using DevFast.Net.Text.Json.NamingPolicy;

namespace DevFast.Net.Text.Tests.Json
{
    [TestFixture]
    public class JsonConstTests
    {
        [Test]
        public void Instances_Are_Consistent()
        {
            Multiple(() =>
            {
                That(ReferenceEquals(JsonConst.CamelCase, JsonNamingPolicy.CamelCase), Is.True);
                That(ReferenceEquals(JsonConst.LongSnakeCase, JsonLongSnakeCaseNamingPolicy.LongSnakeCase), Is.True);
                That(ReferenceEquals(JsonConst.SnakeCase, JsonSnakeCaseNamingPolicy.SnakeCase), Is.True);
                That(ReferenceEquals(JsonConst.IdemCase, JsonIdemCaseNamingPolicy.IdemCase), Is.True);
            });
        }

        [Test]
        public void Values_Are_Consistent()
        {
            Multiple(() =>
            {
                That(JsonConst.RawUtf8JsonPartReaderMinBuffer, Is.EqualTo(512));
                That(JsonConst.ArrayBeginByte, Is.EqualTo(0x5B));
                That(JsonConst.ArrayEndByte, Is.EqualTo(0x5D));
                That(JsonConst.ObjectBeginByte, Is.EqualTo(0x7B));
                That(JsonConst.ObjectEndByte, Is.EqualTo(0x7D));
                That(JsonConst.StringQuoteByte, Is.EqualTo(0x22));
                That(JsonConst.ValueSeparatorByte, Is.EqualTo(0x2C));
                That(JsonConst.NameSeparatorByte, Is.EqualTo(0x3A));
                That(JsonConst.FirstOfTrueByte, Is.EqualTo(0x74));
                That(JsonConst.FirstOfFalseByte, Is.EqualTo(0x66));
                That(JsonConst.FirstOfNullByte, Is.EqualTo(0x6E));
                That(JsonConst.LastOfBackspaceInStringByte, Is.EqualTo(0x62));
                That(JsonConst.LastOfCarriageReturnInStringByte, Is.EqualTo(0x72));
                That(JsonConst.SecondOfHexDigitInStringByte, Is.EqualTo(0x75));
                That(JsonConst.MinusSignByte, Is.EqualTo(0x2D));
                That(JsonConst.PlusSignByte, Is.EqualTo(0x2B));
                That(JsonConst.ExponentUpperByte, Is.EqualTo(0x45));
                That(JsonConst.ExponentLowerByte, Is.EqualTo(0x65));
                That(JsonConst.DecimalPointByte, Is.EqualTo(0x2E));
                That(JsonConst.Number1Byte, Is.EqualTo(0x31));
                That(JsonConst.Number2Byte, Is.EqualTo(0x32));
                That(JsonConst.Number3Byte, Is.EqualTo(0x33));
                That(JsonConst.Number4Byte, Is.EqualTo(0x34));
                That(JsonConst.Number5Byte, Is.EqualTo(0x35));
                That(JsonConst.Number6Byte, Is.EqualTo(0x36));
                That(JsonConst.Number7Byte, Is.EqualTo(0x37));
                That(JsonConst.Number8Byte, Is.EqualTo(0x38));
                That(JsonConst.Number9Byte, Is.EqualTo(0x39));
                That(JsonConst.Number0Byte, Is.EqualTo(0x30));
                That(JsonConst.ForwardSlashByte, Is.EqualTo(0x2F));
                That(JsonConst.ReverseSlashByte, Is.EqualTo(0x5C));
                That(JsonConst.AsteriskByte, Is.EqualTo(0x2A));
                That(JsonConst.SpaceByte, Is.EqualTo(0x20));
                That(JsonConst.HorizontalTabByte, Is.EqualTo(0x09));
                That(JsonConst.NewLineByte, Is.EqualTo(0x0A));
                That(JsonConst.CarriageReturnByte, Is.EqualTo(0x0D));
            });
        }
    }
}