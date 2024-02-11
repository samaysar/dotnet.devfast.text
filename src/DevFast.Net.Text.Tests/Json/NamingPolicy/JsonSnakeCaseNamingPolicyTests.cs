using DevFast.Net.Text.Json.NamingPolicy;

namespace DevFast.Net.Text.Tests.Json.NamingPolicy
{
    [TestFixture]
    public class JsonSnakeCaseNamingPolicyTests
    {
        [Test]
        public void Static_Instance_Always_Available()
        {
            That(JsonSnakeCaseNamingPolicy.SnakeCase, Is.Not.Null);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("Abc", "abc")]
        [TestCase("AbC", "ab_c")]
        [TestCase("ABC", "abc")]
        [TestCase("AbcDE", "abc_de")]
        [TestCase("Abc_DE", "abc__de")]
        public void Conversion_Is_Exact(string name, string expected)
        {
            That(expected, Is.EqualTo(JsonSnakeCaseNamingPolicy.SnakeCase.ConvertName(name)));
        }
    }
}