using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.CaptureElement;
public class BlazorCaptureElement : ComponentBase
{
    // The component can import external CSS files.
    // Use this link to research https://stackoverflow.com/questions/18191893/generate-pdf-from-html-in-div-using-javascript
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? CaptureToken { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        //rework blazor-window with different approach than using id attribute
        builder.OpenElement(0, "blazor-capture");
        builder.CloseElement();
    }
}