using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.Collapse;
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

    protected override void OnParametersSet() => AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCollapseToggleButton)));
        builder.AddAttribute(2, "onclick", ToggleClickedAsync);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }

    private async Task ToggleClickedAsync()
    {
        var blazorCollapse = TokenizeResolver.Resolve<BlazorCollapse>(TargetToken);
        blazorCollapse.ToggleVisibility();
        await OnClick.InvokeAsync();
    }
}
