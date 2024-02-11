using DevFast.Net.Text.Json.NamingPolicy;

namespace DevFast.Net.Text.Tests.Json.NamingPolicy
{
    [TestFixture]
    public class JsonLongSnakeCaseNamingPolicyTests
    {
        [Test]
        public void Static_Instance_Always_Available()
        {
            That(JsonLongSnakeCaseNamingPolicy.LongSnakeCase, Is.Not.Null);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("Abc", "abc")]
        [TestCase("AbC", "ab_c")]
        [TestCase("ABC", "a_b_c")]
        [TestCase("AbcDE", "abc_d_e")]
        [TestCase("Abc_DE", "abc__d_e")]
        public void Conversion_Is_Exact(string name, string expected)
        {
            That(expected, Is.EqualTo(JsonLongSnakeCaseNamingPolicy.LongSnakeCase.ConvertName(name)));
        }
    }
}