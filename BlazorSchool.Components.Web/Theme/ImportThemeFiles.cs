using BlazorSchool.Components.Web.Theme.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorSchool.Components.Web.Theme;
internal class ImportThemeFiles : ComponentBase
{
    [Parameter]
    public List<ImportDefinition> Imports { get; set; } = new();

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Imports.Any())
        {
            builder.OpenComponent<HeadContent>(0);
            RenderFragment childContentDelegate = RenderImportTags;
            builder.AddAttribute(1, "ChildContent", childContentDelegate);
            builder.CloseComponent();
        }
    }

    private void RenderImportTags(RenderTreeBuilder builder)
    {
        var filteredImportList = Imports.GroupBy(i => i.Href).Select(groupedImports => groupedImports.First()).ToList();

        foreach (var import in filteredImportList)
        {
            builder.OpenRegion(import.GetHashCode());

            builder.OpenElement(0, import.Rel == "text/javascript" ? "script" : "link");
            builder.AddAttribute(1, "href", import.Href);
            builder.AddAttribute(2, "rel", import.Rel);
            builder.AddMultipleAttributes(3, import.AdditionalAttributes);
            builder.CloseElement();

            builder.CloseRegion();
        }
    }
}
