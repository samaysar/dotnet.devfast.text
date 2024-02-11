using System.Text.Json;

namespace DevFast.Net.Text.Json
{
    /// <summary>
    /// Interface dictating implementation of parsing individual items of a JSON Array
    /// with the possibility to parse individual elements (as defined in <see cref="JsonType"/>) in a JSON sequence.
    /// Parsing of such elements produces <see cref="RawJson"/> representing entire value-form
    /// (including structural characters, string quotes etc.) as <see cref="RawJson.Value"/>, of single element at a time,
    /// of a known <see cref="JsonType"/>.
    /// </summary>
    public interface IJsonArrayReader : IDisposable
    {
        /// <summary>
        /// <see langword="true"/> indicating that reader has reached end of JSON input,
        /// otherwise <see langword="false"/>.
        /// </summary>
        bool EoJ { get; }

        /// <summary>
        /// <see cref="byte"/> value of current position of reader. <see langword="null"/> when
        /// reader has reached <see cref="EoJ"/>.
        /// </summary>
        byte? Current { get; }

        /// <summary>
        /// Total number of <see cref="byte"/>s observed by the reader since the very beginning (0-based position).
        /// </summary>
        long Position { get; }

        /// <summary>
        /// Current capacity as total number of <see cref="byte"/>s.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Provides a convenient way to enumerate over elements of a JSON array (one at a time).
        /// For every iteration, such mechanism produces <see cref="RawJson"/>, where <see cref="RawJson.Value"/> represents
        /// entire value-form (including structural characters, string quotes etc.) of such an individual
        /// element &amp; <see cref="RawJson.Type"/> indicates underlying JSON element type.
        /// Any standard JSON serializer can be used to deserialize <see cref="RawJson.Value"/>
        /// to obtain an instance of corresponding .Net type.
        /// </summary>
        /// <param name="ensureEoj"><see langword="false"/> to ignore leftover JSON after <see cref="JsonConst.ArrayEndByte"/>.
        /// <see langword="true"/> to ensure that no data is present after <see cref="JsonConst.ArrayEndByte"/>. However, both
        /// single line and multiline comments are allowed after <see cref="JsonConst.ArrayEndByte"/> until <see cref="EoJ"/>.</param>
        /// <param name="token">Cancellation token to observe.</param>
        /// <exception cref="JsonException"></exception>
        IEnumerable<RawJson> EnumerateJsonArray(bool ensureEoj, CancellationToken token = default);

        /// <summary>
        /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it checks
        /// if value is <see cref="JsonConst.ArrayBeginByte"/>. If the value matches, then reader advances
        /// its current position to next <see cref="byte"/> in the sequence or to end of JSON. If the value does NOT match,
        /// reader position is maintained on the current byte and an error
        /// (of type <see cref="JsonException"/>) is thrown.
        /// </summary>
        /// <param name="token">Cancellation token to observe</param>
        /// <exception cref="JsonException"></exception>
        void ReadIsBeginArrayWithVerify(CancellationToken token = default);

        /// <summary>
        /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
        /// <see langword="true"/> if value is <see cref="JsonConst.ArrayBeginByte"/>. If the value matches,
        /// then reader advances its current position to next <see cref="byte"/> in the sequence or to end of JSON.
        /// Otherwise, it returns <see langword="false"/> when current byte is NOT <see cref="JsonConst.ArrayBeginByte"/> and
        /// reader position is maintained on the current byte.
        /// </summary>
        /// <param name="token">Cancellation token to observe</param>
        bool ReadIsBeginArray(CancellationToken token = default);

        /// <summary>
        /// Call makes reader skip all the irrelevant whitespaces (comments included). Once done, it returns
        /// <see langword="true"/> if value is <see cref="JsonConst.ArrayEndByte"/>. If the value matches,
        /// then reader advances its current position to next <see cref="byte"/> in the sequence or to end of JSON.
        /// Otherwise, it returns <see langword="false"/> when current byte is NOT <see cref="JsonConst.ArrayEndByte"/> and
        /// reader position is maintained on the current byte.
        /// </summary>
        /// <param name="ensureEoj"><see langword="false"/> to ignore any text (JSON or not) after
        /// observing <see cref="JsonConst.ArrayEndByte"/>.
        /// <see langword="true"/> to ensure that no data is present after <see cref="JsonConst.ArrayEndByte"/>. However, both
        /// single line and multiline comments are allowed before <see cref="EoJ"/>.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <exception cref="JsonException"></exception>
        bool ReadIsEndArray(bool ensureEoj, CancellationToken token = default);

        /// <summary>
        /// Reads the current JSON element as <see cref="RawJson"/>. If it reaches <see cref="EoJ"/> or
        /// encounters <see cref="JsonConst.ArrayEndByte"/>, it returns <see cref="JsonType.Undefined"/> as
        /// <see cref="RawJson.Type"/>.
        /// <para>
        /// One should prefer <see cref="EnumerateJsonArray"/> to parse well-structured JSON stream over this method.
        /// This method is to parse non-standard chain of JSON elements separated by ',' (or not).
        /// </para>
        /// </summary>
        /// <param name="withVerify"><see langword="true"/> to verify the presence of ',' or ']' (but not ',]')
        /// after successfully parsing the current JSON element; <see langword="false"/> otherwise.</param>
        /// <param name="token">Cancellation token to observe.</param>
        /// <exception cref="JsonException"></exception>
        RawJson ReadRaw(bool withVerify = true, CancellationToken token = default);
    }
}
