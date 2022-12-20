using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorAccordionTitle : TargetTokenize, IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; } = EventCallback.Empty;

    protected override void OnParametersSet()
    {
        if (string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"{nameof(BlazorAccordionTitle)} requires a {nameof(TargetToken)}.");
        }

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick");
    }

    // Do not register/un-register the token for this component because the token is pass through child component
    protected override void OnInitialized() { }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorAccordionTitle)));
        RenderFragment blazorCollapseToggleButton = BuildBlazorCollapseButton;
        builder.AddContent(2, blazorCollapseToggleButton);
        builder.CloseElement();
    }

    private void BuildBlazorCollapseButton(RenderTreeBuilder builder)
    {
        builder.OpenComponent<BlazorCollapseToggleButton>(0);
        builder.AddAttribute(1, nameof(BlazorCollapseToggleButton.AdditionalAttributes), AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorAccordionTitle)));
        builder.AddAttribute(2, nameof(BlazorCollapseToggleButton.TargetToken), TargetToken);
        builder.AddAttribute(3, nameof(BlazorCollapseToggleButton.ChildContent), ChildContent);
        builder.CloseComponent();
    }
}
