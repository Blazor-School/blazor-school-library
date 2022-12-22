using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorCaptureElement : TokenizeComponent, IThemable
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    protected override void OnParametersSet() => AttributeUtilities.ThrowsIfContains(AdditionalAttributes, TokenAttributeKey);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorCaptureElement)));
        builder.AddMultipleAttributes(1, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCaptureElement)));
        builder.AddAttribute(2, TokenAttributeKey, Token);
        builder.OpenComponent<CascadingValue<BlazorCaptureElement>>(3);
        builder.AddAttribute(4, "IsFixed", true);
        builder.AddAttribute(5, "Value", this);
        builder.AddAttribute(6, "ChildContent", ChildContent);
        builder.CloseComponent();
        builder.CloseElement();
    }
}