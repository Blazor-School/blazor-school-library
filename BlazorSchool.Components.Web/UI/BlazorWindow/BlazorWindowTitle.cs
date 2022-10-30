using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorSchool.Components.Web.UI;
public class BlazorWindowTitle : ComponentBase
{
    [CascadingParameter]
    private BlazorWindow? CascadedBlazorWindow { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string _windowId { get; init; } = Guid.NewGuid().ToString();

    protected override void OnParametersSet()
    {
        if (CascadedBlazorWindow is null)
        {
            throw new InvalidOperationException($"{nameof(BlazorWindowTitle)} requires a {nameof(BlazorWindow)} component.");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await CascadedBlazorWindow.LoadModules();
            await CascadedBlazorWindow.BlazorWindowModule.Value.InvokeVoidAsync("registerWindowTitleEvent",_windowId, CascadedBlazorWindow.WindowId);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "id", _windowId);
        builder.AddAttribute(3, "draggable", "true");
        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }
}