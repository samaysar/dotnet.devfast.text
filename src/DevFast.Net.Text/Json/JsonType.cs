namespace DevFast.Net.Text.Json
{
    /// <summary>
    /// Various JSON types as defined in https://datatracker.ietf.org/doc/html/rfc7159
    /// (also mentioned at https://www.json.org/json-en.html).
    /// </summary>
    public enum JsonType
    {
        /// <summary>
        /// Absence of any other JSON types. Normally represents end of JSON data or absence of value.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// JSON object containing everything including &amp; in-between '{' and '}'.
        /// </summary>
        Obj = 1,
        /// <summary>
        /// Array/collection of other JSON types, containing everything including &amp; in-between '[' and ']'.
        /// </summary>
        Arr = 2,
        /// <summary>
        /// JSON numerical values defined as 'integer fraction exponent'. See https://www.json.org/json-en.html.
        /// </summary>
        Num = 3,
        /// <summary>
        /// JSON string value containing everything including and in-between '"' and '"'.
        /// </summary>
        Str = 4,
        /// <summary>JSON 'true' or 'false' literal.</summary>
        Bool = 5,
        /// <summary>JSON 'null' literal (NOT same as <see cref="Undefined"/>).</summary>
        Null = 6
    }
}