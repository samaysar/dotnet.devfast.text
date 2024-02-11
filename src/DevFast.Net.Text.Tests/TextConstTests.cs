namespace DevFast.Net.Text.Tests
{
    [TestFixture]
    public class TextConstTests
    {
        [Test]
        public void Values_Are_Consistent()
        {
            Multiple(() =>
            {
                That(TextConst.Utf8NoBom.GetPreamble(), Is.Empty);
                That(TextConst.Utf8NoBom.EncodingName, Is.EqualTo("Unicode (UTF-8)"));
            });
        }
    }
}