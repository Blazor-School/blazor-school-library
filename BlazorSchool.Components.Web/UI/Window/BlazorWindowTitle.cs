using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorSchool.Components.Web.UI.Window;
public class BlazorWindowTitle : TokenizeComponent
{
    [CascadingParameter]
    private BlazorWindow? CascadedBlazorWindow { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnParametersSet()
    {
        if (CascadedBlazorWindow is null)
        {
            throw new InvalidOperationException($"{nameof(BlazorWindowTitle)} requires a {nameof(BlazorWindow)} component.");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (CascadedBlazorWindow is null)
        {
            return;
        }

        if (firstRender)
        {
            await CascadedBlazorWindow.LoadModules();
            await CascadedBlazorWindow.BlazorWindowModule.Value.InvokeVoidAsync("registerWindowTitleEvent", Token, CascadedBlazorWindow.Token);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, TokenAttributeKey, Token);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}