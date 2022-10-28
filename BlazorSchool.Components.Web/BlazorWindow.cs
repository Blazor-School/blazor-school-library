using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web;
public class BlazorWindow : ComponentBase
{
    [Parameter]
    public RenderFragment? BlazorWindowTitle { get; set; }

    [Parameter]
    public RenderFragment? BlazorWindowContent { get; set; }

    protected override void OnParametersSet()
    {
        if(BlazorWindowTitle is null)
        {
            throw new InvalidOperationException($"You need to specify both '{nameof(BlazorWindowTitle)}'.");
        }
        
        if(BlazorWindowContent is null)
        {
            throw new InvalidOperationException($"You need to specify both '{nameof(BlazorWindowContent)}'.");
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddContent(1, (__builder =>
        {
            __builder.OpenElement(2, "div");
            __builder.AddContent(3, BlazorWindowTitle);
            __builder.CloseElement();
        }));
        builder.AddContent(4, (__builder =>
        {
            __builder.OpenElement(5, "div");
            __builder.AddContent(6, BlazorWindowContent);
            __builder.CloseElement();
        }));
        builder.CloseElement();
    }
}
