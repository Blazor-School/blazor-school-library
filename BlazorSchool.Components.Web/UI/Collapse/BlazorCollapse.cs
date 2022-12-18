using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.Collapse;
public class BlazorCollapse : TokenizeComponent, IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public string ShowClass { get; set; } = "show";

    [Parameter]
    public string HideClass { get; set; } = "hide";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool InitialVisibility { get; set; }

    private bool _currentVisibility;

    protected override void OnParametersSet() => AttributeUtilities.ThrowsIfContains(AdditionalAttributes, TokenAttributeKey);

    protected override void OnInitialized()
    {
        RegisterTokenize();
        _currentVisibility = InitialVisibility;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var attachedAttributes = AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCollapse));
        attachedAttributes = AttributeUtilities.AttachCssClass(attachedAttributes, _currentVisibility ? ShowClass : HideClass);
        builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorCollapse)));
        builder.AddAttribute(1, TokenAttributeKey, Token);
        builder.AddMultipleAttributes(2, attachedAttributes);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }

    internal void ToggleVisibility()
    {
        _currentVisibility = !_currentVisibility;
        StateHasChanged();
    }
}
