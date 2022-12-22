using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorWindowOpenButton : TargetTokenize, IThemable
{
    [CascadingParameter]
    private BlazorWindow? CascadedBlazorWindow { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback? OnClick { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    protected override void OnParametersSet()
    {
        if (CascadedBlazorWindow is null && string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"{nameof(BlazorWindowTitle)} requires a {nameof(BlazorWindow)} component or a {nameof(TargetToken)}.");
        }

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorWindowOpenButton)));
        builder.AddAttribute(2, "onclick", Clicked);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }

    private async Task Clicked()
    {
        if (CascadedBlazorWindow is not null)
        {
            CascadedBlazorWindow?.OpenWindow();
        }
        else
        {
            var windowComponent = TokenizeResolver.Resolve<BlazorWindow>(TargetToken);
            windowComponent.OpenWindow();
        }

        await (OnClick?.InvokeAsync() ?? Task.CompletedTask);
    }
}
