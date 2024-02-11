using DevFast.Net.Text.Json.NamingPolicy;

namespace DevFast.Net.Text.Tests.Json.NamingPolicy
{
    [TestFixture]
    public class JsonIdemCaseNamingPolicyTests
    {
        [Test]
        public void Static_Instance_Always_Available()
        {
            That(JsonIdemCaseNamingPolicy.IdemCase, Is.Not.Null);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("Abc", "Abc")]
        [TestCase("AbC", "AbC")]
        public void Conversion_Is_Exact(string name, string expected)
        {
            That(expected, Is.EqualTo(JsonIdemCaseNamingPolicy.IdemCase.ConvertName(name)));
        }
    }
}