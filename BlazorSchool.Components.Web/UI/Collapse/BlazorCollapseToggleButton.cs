using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorCollapseToggleButton : TargetTokenize, IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; } = EventCallback.Empty;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnParametersSet()
    {
        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");

        if (string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"{nameof(BlazorCollapseToggleButton)} must have a {nameof(TargetToken)}.");
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCollapseToggleButton)));
        builder.AddAttribute(2, "onclick", ToggleClickedAsync);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }

    // The toggle button does not allow to toggle the parent Blazor Collapse but can be inside Blazor Collapse.
    private async Task ToggleClickedAsync()
    {
        var blazorCollapse = TokenizeResolver.Resolve<BlazorCollapse>(TargetToken);
        blazorCollapse.ToggleVisibility();
        await OnClick.InvokeAsync();
    }
}
