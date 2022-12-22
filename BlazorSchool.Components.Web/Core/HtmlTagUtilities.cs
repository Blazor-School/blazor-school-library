using System.Text.RegularExpressions;

namespace BlazorSchool.Components.Web.Core;
internal class HtmlTagUtilities
{
    public static string ToHtmlTag(string componentName) => Regex.Replace(componentName, "(.{1})([A-Z])", "$1-$2");
}
