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

    [CascadingParameter]
    public BlazorCollapse? CascadedBlazorCollapse { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; } = EventCallback.Empty;

    [Parameter]
    public string CollapseShowingClass { get; set; } = "show";

    [Parameter]
    public string CollapseHidingClass { get; set; } = "hide";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnParametersSet()
    {
        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");

        if (string.IsNullOrEmpty(TargetToken) && CascadedBlazorCollapse is null)
        {
            throw new InvalidOperationException($"{nameof(BlazorCollapseToggleButton)} must have a {nameof(TargetToken)} or must be put inside a {nameof(BlazorCollapse)} component.");
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
        if (!string.IsNullOrEmpty(TargetToken))
        {
            var blazorCollapse = TokenizeResolver.Resolve<BlazorCollapse>(TargetToken);
            blazorCollapse.ToggleVisibility();
        }
        else
        {
            CascadedBlazorCollapse?.ToggleVisibility();
        }

        await OnClick.InvokeAsync();
    }
}
