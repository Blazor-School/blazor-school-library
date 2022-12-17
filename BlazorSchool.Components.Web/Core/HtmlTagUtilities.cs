using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorSchool.Components.Web.Core;
internal class HtmlTagUtilities
{
    public static string ToHtmlTag(string componentName) => Regex.Replace(componentName, "(.{1})([A-Z])", "$1-$2");
}
