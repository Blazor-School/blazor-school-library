using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.UI;
public class BlazorCollapseToggleButton : TargetTokenize, IThemable, IDisposable
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
    public string CollapseShowingClass { get; set; } = "blazor-collapse-toggle-show";

    [Parameter]
    public string CollapseHidingClass { get; set; } = "blazor-collapse-toggle-hide";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private BlazorCollapse? _currentBlazorCollapse;
    private bool _subscribedBlazorCollapse = false;

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
        var attachedAttributes = AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCollapseToggleButton));
        attachedAttributes = AttributeUtilities.AttachCssClass(attachedAttributes, GetCollapseVisibility() ? CollapseShowingClass : CollapseHidingClass);

        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(1, attachedAttributes);
        builder.AddAttribute(2, "onclick", ToggleClickedAsync);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }

    private bool GetCollapseVisibility()
    {
        bool fallbackValue = false;

        if (CascadedBlazorCollapse is null)
        {
            try
            {
                _currentBlazorCollapse = TokenizeResolver.Resolve<BlazorCollapse>(TargetToken);

                if (_subscribedBlazorCollapse is false)
                {
                    _currentBlazorCollapse.OnComponentUpdated += OnBlazorCollapseUpdate;
                    _subscribedBlazorCollapse = true;
                }

                return _currentBlazorCollapse?.CurrentVisibility ?? fallbackValue;
            }
            // When the BlazorCollapse hasn't initiated yet, we ignore
            catch (InvalidOperationException)
            {
                return fallbackValue;
            }
        }
        else
        {
            return CascadedBlazorCollapse?.CurrentVisibility ?? fallbackValue;
        }
    }

    // The toggle button does not allow to toggle the parent Blazor Collapse but can be inside Blazor Collapse.
    private async Task ToggleClickedAsync()
    {
        if (!string.IsNullOrEmpty(TargetToken))
        {
            _currentBlazorCollapse ??= TokenizeResolver.Resolve<BlazorCollapse>(TargetToken);

            if (_subscribedBlazorCollapse is false)
            {
                _currentBlazorCollapse.OnComponentUpdated += OnBlazorCollapseUpdate;
                _subscribedBlazorCollapse = true;
            }

            _currentBlazorCollapse?.ToggleVisibility();
        }
        else
        {
            CascadedBlazorCollapse?.ToggleVisibility();
        }

        await OnClick.InvokeAsync();
    }

    private void OnBlazorCollapseUpdate(object? sender, EventArgs args) => StateHasChanged();

    public void Dispose()
    {
        if (_subscribedBlazorCollapse && _currentBlazorCollapse is not null)
        {
            _currentBlazorCollapse.OnComponentUpdated -= OnBlazorCollapseUpdate;
        }
    }
}
