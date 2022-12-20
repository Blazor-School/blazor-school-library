using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorCollapse : TokenizeComponent, IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public string ShowClass { get; set; } = "blazor-collapse-show";

    [Parameter]
    public string HideClass { get; set; } = "blazor-collapse-hide";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool InitialVisibility { get; set; }

    internal bool CurrentVisibility { get; set; } = false;

    protected override void OnParametersSet() => AttributeUtilities.ThrowsIfContains(AdditionalAttributes, TokenAttributeKey);

    protected override void OnInitialized()
    {
        RegisterTokenize();
        CurrentVisibility = InitialVisibility;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var attachedAttributes = AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCollapse));
        attachedAttributes = AttributeUtilities.AttachCssClass(attachedAttributes, CurrentVisibility ? ShowClass : HideClass);

        builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorCollapse)));
        builder.AddAttribute(1, TokenAttributeKey, Token);
        builder.AddMultipleAttributes(2, attachedAttributes);

        builder.OpenComponent<CascadingValue<BlazorCollapse>>(3);
        builder.AddAttribute(4, "IsFixed", true);
        builder.AddAttribute(5, "Value", this);
        builder.AddAttribute(6, "ChildContent", ChildContent);
        builder.CloseComponent();

        builder.CloseElement();
    }

    internal void ToggleVisibility()
    {
        CurrentVisibility = !CurrentVisibility;
        StateHasChanged();
    }
}
