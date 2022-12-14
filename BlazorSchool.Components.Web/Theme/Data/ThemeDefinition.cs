using BlazorSchool.Components.Web.Theme.Data;
using System.Text.Json;

namespace BlazorSchool.Components.Web.Theme.Data;
internal class ThemeDefinition
{
    public string Name { get; set; } = "";
    public List<ImportDefinition> Imports { get; set; } = new();
    public Dictionary<string, string> Components { get; set; } = new();

    internal static ThemeDefinition FromJson(JsonElement jsonElement)
    {
        jsonElement.TryGetProperty(nameof(Name), out var nameElement);

        if (nameElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            throw new InvalidOperationException($"The theme config must have a {nameof(Name)} property");
        }

        string name = nameElement.Deserialize<string>()!;
        var imports = new List<ImportDefinition>();
        var components = new Dictionary<string, string>();
        jsonElement.TryGetProperty(nameof(Imports), out var importElements);

        if (importElements.ValueKind is not JsonValueKind.Undefined and not JsonValueKind.Null)
        {
            foreach (var importsElement in importElements.EnumerateArray())
            {
                imports.Add(ImportDefinition.FromJson(importsElement));
            }
        }

        jsonElement.TryGetProperty(nameof(Components), out var componentElements);

        if (componentElements.ValueKind is not JsonValueKind.Undefined and not JsonValueKind.Null)
        {
            components = componentElements.Deserialize<Dictionary<string, string>>() ?? new();
        }

        return new()
        {
            Name = name,
            Imports = imports,
            Components = components
        };
    }
}
