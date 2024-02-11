using System.Text.Json;

namespace DevFast.Net.Text.Json.NamingPolicy
{
    /// <summary>
    /// Class implements <see cref="JsonNamingPolicy" /> to provide Identical (i.e. no change) names
    /// (e.g.: AbcDef to AbcDef, MyTKiBd to MyTKiBd, ABC to ABC etc) for JSON data.
    /// </summary>
    public sealed class JsonIdemCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// Returns the naming policy which does not change the name.
        /// </summary>
        public static JsonNamingPolicy IdemCase { get; } = new JsonIdemCaseNamingPolicy();

        private JsonIdemCaseNamingPolicy() { }

        /// <summary>
        /// Does NOT convert provided <paramref name="name"/> and returns the
        /// value as it is (e.g.: AbcDef to AbcDef, MyTKiBd to MyTKiBd, ABC to ABC etc).
        /// </summary>
        /// <param name="name">String value to convert.</param>
        public override string ConvertName(string name) => name;
    }
}
