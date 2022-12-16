using BlazorSchool.Components.Web.Theme;

namespace BlazorSchool.Components.Web.Core;
internal static class AttributeUtilities
{
    public static IReadOnlyDictionary<string, object>? Normalized(IReadOnlyDictionary<string, object>? originalReadOnlyDictionary, BlazorApplyTheme? blazorApplyTheme, string callingComponent) => originalReadOnlyDictionary switch
    {
        _ when blazorApplyTheme is null => originalReadOnlyDictionary,
        _ when originalReadOnlyDictionary is null || !originalReadOnlyDictionary.Any() => new Dictionary<string, object>()
            {
                { "class", blazorApplyTheme[callingComponent] }
            },
        _ when originalReadOnlyDictionary.ContainsKey("class") => originalReadOnlyDictionary,
        _ => new Dictionary<string, object>(originalReadOnlyDictionary)
            {
                { "class", blazorApplyTheme[callingComponent] }
            },
    };

    public static void ThrowsIfContains(IReadOnlyDictionary<string, object>? additionalAttributes, params string[] attributeList)
    {
        if (additionalAttributes is not null && attributeList.Any(additionalAttributes.ContainsKey))
        {
            throw new InvalidOperationException($"Do not specify the following attributes {string.Join(", ", attributeList)}.");
        }
    }
}