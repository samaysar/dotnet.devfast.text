namespace DevFast.Net.Text.Json.NamingPolicy;

/// <summary>
/// Class implements <see cref="JsonNamingPolicy" /> to provide Snake-Case names
/// (e.g.: AbcDef to abc_def, MyTKiBd to my_tki_bd, ABC to abc etc) for JSON data.
/// </summary>
public sealed class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary>
    /// Returns the naming policy for snake-casing.
    /// </summary>
    public static JsonNamingPolicy SnakeCase { get; } = new JsonSnakeCaseNamingPolicy();

    private JsonSnakeCaseNamingPolicy() { }

    /// <summary>
    /// Converts provided <paramref name="name"/> to snake-case
    /// (e.g.: AbcDef to abc_def, MyTKiBd to my_tki_bd, ABC to abc etc).
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

        bool prevUpper = char.IsUpper(name[0]);
        for (int i = 1; i < name.Length; i++)
        {
            char c = name[i];
            if (char.IsUpper(c))
            {
                if (!prevUpper)
                {
                    _ = sb.Append('_');
                }
                _ = sb.Append(char.ToLowerInvariant(c));
                prevUpper = true;
            }
            else
            {
                prevUpper = false;
                _ = sb.Append(c);
            }
        }

        return sb.ToString();
    }
}