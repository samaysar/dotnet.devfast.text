using System.Text;
using System.Text.Json;

namespace DevFast.Net.Text.Json.NamingPolicy
{
    /// <summary>
    /// Class implements <see cref="JsonNamingPolicy" /> to provide long Snake-Case names
    /// (e.g.: AbcDef to abc_def, MyTKiBd to my_t_ki_bd, ABC to a_b_c etc) for JSON data.
    /// </summary>
    public sealed class JsonLongSnakeCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// Returns the naming policy for snake-casing.
        /// </summary>
        public static JsonNamingPolicy LongSnakeCase { get; } = new JsonLongSnakeCaseNamingPolicy();

        private JsonLongSnakeCaseNamingPolicy() { }

        /// <summary>
        /// Converts provided <paramref name="name"/> to snake-case
        /// (e.g.: AbcDef to abc_def, MyTKiBd to my_t_ki_bd, ABC to a_b_c etc).
        /// </summary>
        /// <param name="name">String value to convert to snake-case.</param>
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;

            var sb = new StringBuilder(name.Length * 2);
            sb.Append(char.ToLowerInvariant(name[0]));

            for (int i = 1; i < name.Length; i++)
            {
                var c = name[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}