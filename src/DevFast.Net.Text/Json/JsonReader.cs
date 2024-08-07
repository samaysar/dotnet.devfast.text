﻿using DevFast.Net.Extensions.Etc;
using DevFast.Net.Text.Json.Utf8;

namespace DevFast.Net.Text.Json;

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
    /// <see cref="JsonConst.JsonReaderMinBuffer"/> will be ignored.</param>
    /// <param name="disposeStream"><see langword="true"/> to dispose <paramref name="stream"/> when either
    /// current instance of <see cref="IJsonArrayReader"/> itself disposing or
    /// when <paramref name="stream"/> is completely read; <see langword="false"/> otherwise.</param>
    public static async ValueTask<IJsonArrayReader> CreateUtf8ArrayReaderAsync(Stream stream,
        Token token = default,
        int size = JsonConst.JsonReaderMinBuffer,
        bool disposeStream = false)
    {
        stream = stream.ThrowArgumentExceptionForNull($"Inside {nameof(CreateUtf8ArrayReaderAsync)}, {nameof(stream)}")
            .ThrowArgumentExceptionOnPredicateFail(static x => x.CanRead, $"Inside {nameof(CreateUtf8ArrayReaderAsync)}, {nameof(stream)}", nameof(Stream.CanRead));

        byte[] bom = Encoding.UTF8.GetPreamble();
        if (stream is MemoryStream ms)
        {
            ms.TryGetBuffer(out ArraySegment<byte> segment)
                .ThrowInvalidOperationExceptionIfFalse($"Inside {nameof(CreateUtf8ArrayReaderAsync)}, {nameof(MemoryStream)} must exposed it's buffer.");
            int newOffSet = 0;
            byte[] segmentArray = segment.Array!;
            newOffSet = segment.Count >= bom.Length &&
                        bom.All(b => b.Equals(segmentArray[segment.Offset + newOffSet++])) ? bom.Length : 0;
            return new MemJsonArrayReader(ms,
                new ArraySegment<byte>(segment.Array!, segment.Offset + newOffSet, segment.Count - newOffSet),
                disposeStream);
        }
        else
        {
            byte[] buffer = new byte[Math.Max(JsonConst.JsonReaderMinBuffer, size)];
#if NET7_0_OR_GREATER
            int end = await stream.ReadAtLeastAsync(buffer,
                buffer.Length / 2,
                false,
                token).ConfigureAwait(false);
#else
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
            var end = await stream.ReadAsync(buffer, token).ConfigureAwait(false);
#else
            int end = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
#endif
            if (end < bom.Length)
            {
                int newReads;
                do
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    newReads = await stream.ReadAsync(buffer.AsMemory(end, buffer.Length - end), token).ConfigureAwait(false);
#else
                    newReads = await stream.ReadAsync(buffer, end, buffer.Length - end, token).ConfigureAwait(false);
#endif
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