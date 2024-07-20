#### [DevFast.Net.Text](index.md 'index')

## DevFast.Net.Text.Json.NamingPolicy Namespace

Under this [namespace](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/namespace 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/namespace') naming policies for JSON data are implemented.

| Classes | |
| :--- | :--- |
| [JsonIdemCaseNamingPolicy](DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy.md 'DevFast.Net.Text.Json.NamingPolicy.JsonIdemCaseNamingPolicy') | Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide Identical (i.e. no change) names (e.g.: AbcDef to AbcDef, MyTKiBd to MyTKiBd, ABC to ABC etc) for JSON data. |
| [JsonLongSnakeCaseNamingPolicy](DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy.md 'DevFast.Net.Text.Json.NamingPolicy.JsonLongSnakeCaseNamingPolicy') | Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide long Snake-Case names (e.g.: AbcDef to abc_def, MyTKiBd to my_t_ki_bd, ABC to a_b_c etc) for JSON data. |
| [JsonSnakeCaseNamingPolicy](DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy.md 'DevFast.Net.Text.Json.NamingPolicy.JsonSnakeCaseNamingPolicy') | Class implements [System.Text.Json.JsonNamingPolicy](https://docs.microsoft.com/en-us/dotnet/api/System.Text.Json.JsonNamingPolicy 'System.Text.Json.JsonNamingPolicy') to provide Snake-Case names (e.g.: AbcDef to abc_def, MyTKiBd to my_tki_bd, ABC to abc etc) for JSON data. |
