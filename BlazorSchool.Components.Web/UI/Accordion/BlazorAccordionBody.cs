using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorAccordionBody : TokenizeComponent, IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool InitialVisibility { get; set; }

    [Parameter]
    public string ShowClass { get; set; } = "blazor-accordion-body-show";

    [Parameter]
    public string HideClass { get; set; } = "blazor-accordion-body-hide";

    protected override void OnParametersSet()
    {
        if (string.IsNullOrEmpty(Token))
        {
            throw new InvalidOperationException($"You need to specify a {nameof(Token)} for {nameof(BlazorAccordionBody)}.");
        }
    }

    // Do not register/un-register the token for this component because the token is pass through child component
    protected override void OnInitialized() {}
    public override void Dispose() {}

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorAccordionBody)));
        RenderFragment blazorCollapse = BuildBlazorCollapse;
        builder.AddContent(1, blazorCollapse);
        builder.CloseElement();
    }

    private void BuildBlazorCollapse(RenderTreeBuilder builder)
    {
        builder.OpenComponent<BlazorCollapse>(0);
        builder.AddAttribute(1, nameof(BlazorCollapse.AdditionalAttributes), AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorAccordionBody)));
        builder.AddAttribute(2, nameof(BlazorCollapse.Token), Token);
        builder.AddAttribute(3, nameof(BlazorCollapse.HideClass), HideClass);
        builder.AddAttribute(4, nameof(BlazorCollapse.ShowClass), ShowClass);
        builder.AddAttribute(5, nameof(BlazorCollapse.InitialVisibility), InitialVisibility);
        builder.AddAttribute(6, nameof(BlazorCollapse.ChildContent), ChildContent);
        builder.CloseComponent();
    }
}
