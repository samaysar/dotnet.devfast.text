namespace DevFast.Net.Text.Json.NamingPolicy;

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
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        StringBuilder sb = new(name.Length * 2);
        _ = sb.Append(char.ToLowerInvariant(name[0]));

        for (int i = 1; i < name.Length; i++)
        {
            char c = name[i];
            if (char.IsUpper(c))
            {
                _ = sb.Append('_');
                _ = sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                _ = sb.Append(c);
            }
        }

        return sb.ToString();
    }
}