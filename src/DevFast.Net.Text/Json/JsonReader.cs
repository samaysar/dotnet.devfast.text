using System.Text;
using DevFast.Net.Text.Json.Utf8;

namespace DevFast.Net.Text.Json
{
    /// <summary>
    /// Static class to create JSON reader instance.
    /// </summary>
    public static class JsonReader
    {
        /// <summary>
        /// Convenient method, to create well initialized instance of type <see cref="IJsonArrayReader"/>.
        /// </summary>
        /// <param name="stream">A readable stream containing JSON array data.</param>
        /// <param name="token">Cancellation token to observe.</param>
        /// <param name="size">Initial size of underlying byte buffer. Any value less than
        /// <see cref="JsonConst.RawUtf8JsonPartReaderMinBuffer"/> will be ignored.</param>
        /// <param name="disposeStream"><see langword="true"/> to dispose <paramref name="stream"/> when either
        /// current instance of <see cref="IJsonArrayReader"/> itself disposing or
        /// when <paramref name="stream"/> is completely read; <see langword="false"/> otherwise.</param>
        public static async ValueTask<IJsonArrayReader> CreateUtf8ArrayReaderAsync(Stream stream,
            CancellationToken token = default,
            int size = JsonConst.RawUtf8JsonPartReaderMinBuffer,
            bool disposeStream = false)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException($"{nameof(stream)} should support Read operation!");
            }

            byte[] bom = Encoding.UTF8.GetPreamble();
            if (stream is MemoryStream ms)
            {
                if (!ms.TryGetBuffer(out ArraySegment<byte> segment))
                {
                    throw new ArgumentException("Stream buffer is not exposed!");
                }
                int newOffSet = 0;
                newOffSet = segment.Count >= bom.Length &&
                            bom.All(b => b.Equals(segment[segment.Offset + newOffSet++])) ? bom.Length : 0;
                return new MemJsonArrayReader(ms,
                    new ArraySegment<byte>(segment.Array!, segment.Offset + newOffSet, segment.Count - newOffSet),
                    disposeStream);
            }
            else
            {
                byte[] buffer = new byte[Math.Max(JsonConst.RawUtf8JsonPartReaderMinBuffer, size)];
#if NET7_0_OR_GREATER
                int end = await stream.ReadAtLeastAsync(buffer,
                    buffer.Length / 2,
                    false,
                    token).ConfigureAwait(false);
#else
                var end = await stream.ReadAsync(buffer, token).ConfigureAwait(false);
                if (end < bom.Length)
                {
                    int newReads;
                    do
                    {
                        newReads = await stream.ReadAsync(buffer.AsMemory(end, buffer.Length - end), token).ConfigureAwait(false);
                        end += newReads;
                    } while (newReads != 0 && end < bom.Length);
                }
#endif
                int begin = 0;
                begin = end >= bom.Length && bom.All(b => b.Equals(buffer[begin++])) ? bom.Length : 0;
                return new JsonArrayReader(stream, buffer, begin, end, disposeStream);
            }
        }
    }
}