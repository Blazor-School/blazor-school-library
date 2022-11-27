using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.Window;
public class BlazorWindowCloseAction : ComponentBase
{
    [CascadingParameter]
    private BlazorWindow? CascadedBlazorWindow { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback? OnClick { get; set; }

    [Parameter]
    public string TargetToken { get; set; } = "";

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject]
    private TokenizeResolver TokenizeResolver { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (CascadedBlazorWindow is null && string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"{nameof(BlazorWindowTitle)} requires a {nameof(BlazorWindow)} component or a {nameof(TargetToken)}.");
        }

        if (CascadedBlazorWindow is not null && !string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"Use {nameof(BlazorWindow)} component or a {nameof(TargetToken)}. Do not use both.");
        }

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "onclick", Clicked);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }

    private async Task Clicked()
    {
        if (CascadedBlazorWindow is not null)
        {
            CascadedBlazorWindow?.CloseWindow();
        }
        else
        {
            var windowComponent = TokenizeResolver.Resolve<BlazorWindow>(TargetToken);
            windowComponent.CloseWindow();
        }

        await (OnClick?.InvokeAsync() ?? Task.CompletedTask);
    }
}
