namespace BlazorSchool.Components.Web.Theme.Data;
public class BlazorThemePack
{
    internal BlazorApplyTheme BlazorApplyTheme { get; set; } = new();

    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public List<string> Themes { get; set; } = new();

    public void ChangeTheme(string name) => BlazorApplyTheme.ChangeTheme(name, true);
}
