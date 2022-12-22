using BlazorSchool.Components.Web.Theme;
using System.Text.RegularExpressions;

namespace BlazorSchool.Components.Web.Core;
internal static partial class AttributeUtilities
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

    public static IReadOnlyDictionary<string, object> AttachCssClass(IReadOnlyDictionary<string, object>? originalReadOnlyDictionary, string cssClasses)
    {
        if (originalReadOnlyDictionary is null)
        {
            return new Dictionary<string, object>()
            {
                { "class", cssClasses }
            };
        }

        _ = originalReadOnlyDictionary.TryGetValue("class", out object? originalCssClassesObj);
        var result = new Dictionary<string, object>(originalReadOnlyDictionary);

        if (originalCssClassesObj is not null and string originalCssClasses)
        {
            var removeSpacesRegex = RemoveSpacesRegex();
            originalCssClasses = removeSpacesRegex.Replace(originalCssClasses, " ").Trim();
            cssClasses = removeSpacesRegex.Replace(cssClasses, " ").Trim();
            string joinedCssClasses = string.Join(" ", originalCssClasses, cssClasses);
            var splitedCssClasses = joinedCssClasses.Split(" ").Distinct();
            string finalCssClasses = string.Join(" ", splitedCssClasses);
            result["class"] = finalCssClasses;
        }

        return result;
    }

    public static void ThrowsIfContains(IReadOnlyDictionary<string, object>? additionalAttributes, params string[] attributeList)
    {
        if (additionalAttributes is not null && attributeList.Any(additionalAttributes.ContainsKey))
        {
            throw new InvalidOperationException($"Do not specify the following attributes {string.Join(", ", attributeList)}.");
        }
    }

    [GeneratedRegex("[ ]{2,}")]
    private static partial Regex RemoveSpacesRegex();
}