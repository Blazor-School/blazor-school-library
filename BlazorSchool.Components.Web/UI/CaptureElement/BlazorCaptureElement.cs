using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI.CaptureElement;
public class BlazorCaptureElement : TokenizeComponent
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void OnParametersSet() => AttributeUtilities.ThrowsIfContains(AdditionalAttributes, TokenAttributeKey);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "blazor-capture");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, TokenAttributeKey, Token);
        builder.OpenComponent<CascadingValue<BlazorCaptureElement>>(3);
        builder.AddAttribute(4, "IsFixed", true);
        builder.AddAttribute(5, "Value", this);
        builder.AddAttribute(6, "ChildContent", ChildContent);
        builder.CloseComponent();
        builder.CloseElement();
    }
}