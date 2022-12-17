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

    protected override void OnParametersSet() => AttributeUtilities.ThrowsIfContains(AdditionalAttributes, TokenAttributeKey);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(1, HtmlTagUtilities.ToHtmlTag(nameof(BlazorCollapse)));
        builder.AddAttribute(2, TokenAttributeKey, Token);
        builder.AddMultipleAttributes(3, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCollapse)));
        builder.CloseElement();
    }
}
