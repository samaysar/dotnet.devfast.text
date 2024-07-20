namespace DevFast.Net.Text.Tests.Json
{
    [TestFixture]
    public class JsonReaderTests
    {
        [Test]
        public void CreateUtf8ArrayReaderAsync_Throws_Error_If_Stream_Is_Not_Readable()
        {
            Stream s = Substitute.For<Stream>();
            _ = s.CanRead.Returns(false);
            ArgumentException err = ThrowsAsync<ArgumentException>(async () => await JsonReader.CreateUtf8ArrayReaderAsync(s).ConfigureAwait(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Inside CreateUtf8ArrayReaderAsync, stream does not satisfy CanRead."));
            });
        }

        [Test]
        public void CreateUtf8ArrayReaderAsync_Throws_Error_If_MemoryStream_Does_Not_Provide_Buffer()
        {
            MemoryStream m = new(Array.Empty<byte>(), 0, 0, false, false);
            InvalidOperationException err = ThrowsAsync<InvalidOperationException>(async () => await JsonReader.CreateUtf8ArrayReaderAsync(m).ConfigureAwait(false))!;
            Multiple(() =>
            {
                That(err, Is.Not.Null);
                That(err.Message, Is.EqualTo("Inside CreateUtf8ArrayReaderAsync, MemoryStream must exposed it's buffer."));
            });
        }
    }
}
