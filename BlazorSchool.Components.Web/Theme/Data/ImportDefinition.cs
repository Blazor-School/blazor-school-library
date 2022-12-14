using System.Text.Json;

namespace BlazorSchool.Components.Web.Theme.Data;
internal class ImportDefinition
{
    public string Href { get; set; } = "";
    public string Rel { get; set; } = "";
    public Dictionary<string, string> AdditionalAttributes { get; set; } = new();

    internal static ImportDefinition FromJson(JsonElement jsonElement)
    {
        _ = jsonElement.TryGetProperty(nameof(Href).ToLower(), out var hrefElement);

        if (hrefElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            throw new InvalidOperationException($"The theme config must have a {nameof(Href).ToLower()} property");
        }

        _ = jsonElement.TryGetProperty(nameof(Rel).ToLower(), out var relElement);

        if (relElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            throw new InvalidOperationException($"The theme config must have a {nameof(Rel).ToLower()} property");
        }

        var otherAttributes = jsonElement.Deserialize<Dictionary<string, string>>() ?? new();
        _ = otherAttributes.Remove("href");
        _ = otherAttributes.Remove("rel");

        string href = hrefElement.Deserialize<string>()!;
        string rel = relElement.Deserialize<string>()!;

        return new()
        {
            Href = href,
            Rel = rel,
            AdditionalAttributes = otherAttributes
        };
    }
}
