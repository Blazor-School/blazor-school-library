namespace BlazorSchool.Components.Web.Theme.Data;
public class ComponentCssProvider
{
    internal Dictionary<string, string> InnerDict { get; set; } = new();

    public string? this[string componentName]
    {
        get
        {
            InnerDict.TryGetValue(componentName, out string? value);

            return value;
        }
    }
}
