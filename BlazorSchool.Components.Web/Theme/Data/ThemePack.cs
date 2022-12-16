using System.Text.Json;

namespace BlazorSchool.Components.Web.Theme.Data;
internal class ThemePack
{
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public List<ThemeDefinition> Themes { get; set; } = new();

    internal static ThemePack FromThemeConfig(ThemePack themeConfig) => new()
    {
        Name = themeConfig.Name,
        Author = themeConfig.Author,
        Themes = themeConfig.Themes.ToList()
    };

    internal static ThemePack FromJson(string json)
    {
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
        _ = jsonElement.TryGetProperty(nameof(Name), out var nameElement);

        if (nameElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            throw new InvalidOperationException($"The theme config must have a {nameof(Name)} property");
        }

        _ = jsonElement.TryGetProperty(nameof(Author), out var authorElement);

        if (authorElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            throw new InvalidOperationException($"The theme config must have a {nameof(Author)} property");
        }

        string name = nameElement.Deserialize<string>()!;
        string author = authorElement.Deserialize<string>()!;
        var themes = new List<ThemeDefinition>();

        _ = jsonElement.TryGetProperty(nameof(Themes), out var themeElements);

        if (themeElements.ValueKind is not JsonValueKind.Undefined and not JsonValueKind.Null)
        {
            foreach (var themeElement in themeElements.EnumerateArray())
            {
                themes.Add(ThemeDefinition.FromJson(themeElement));
            }
        }

        return new()
        {
            Name = name,
            Author = author,
            Themes = themes
        };
    }
}
