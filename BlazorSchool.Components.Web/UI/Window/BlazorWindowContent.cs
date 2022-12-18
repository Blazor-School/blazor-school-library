using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.Window;
public class BlazorWindowContent : ComponentBase, IThemable
{
    [CascadingParameter]
    private BlazorWindow? CascadedBlazorWindow { get; set; }

    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void OnParametersSet()
    {
        if (CascadedBlazorWindow is null)
        {
            throw new InvalidOperationException($"{nameof(BlazorWindowTitle)} requires a {nameof(BlazorWindow)} component.");
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorWindowContent)));
        builder.AddMultipleAttributes(3, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorWindowContent)));
        builder.AddContent(0, ChildContent);
        builder.CloseElement();
    }
}