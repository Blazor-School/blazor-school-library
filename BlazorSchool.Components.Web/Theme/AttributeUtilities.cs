using System.Globalization;

namespace BlazorSchool.Components.Web.Theme;
internal static class AttributeUtilities
{
    public static string? CombineClassNames(IReadOnlyDictionary<string, object>? additionalAttributes, string? classNames)
    {
        if (additionalAttributes is null || !additionalAttributes.TryGetValue("class", out object? @class))
        {
            return classNames;
        }

        string? classAttributeValue = Convert.ToString(@class, CultureInfo.InvariantCulture);

        return string.IsNullOrEmpty(classAttributeValue)
            ? classNames
            : string.IsNullOrEmpty(classNames) ? classAttributeValue : $"{classAttributeValue} {classNames}";
    }

    public static void ThrowsIfContains(IReadOnlyDictionary<string, object>? additionalAttributes, params string[] attributeList)
    {
        if (additionalAttributes is not null && attributeList.Any(a => additionalAttributes.ContainsKey(a)))
        {
            throw new InvalidOperationException($"Do not specify the following attributes {string.Join(", ", attributeList)}.");
        }
    }
}