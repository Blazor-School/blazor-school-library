using BlazorSchool.Components.Web.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.Window;
public class BlazorWindowCloseAction : ComponentBase
{
    [CascadingParameter]
    private BlazorWindow? CascadedBlazorWindow { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback OnCloseClick { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void OnParametersSet()
    {
        if (CascadedBlazorWindow is null)
        {
            throw new InvalidOperationException($"{nameof(BlazorWindowTitle)} requires a {nameof(BlazorWindow)} component.");
        }

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "onclick",OnCloseClick);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}
