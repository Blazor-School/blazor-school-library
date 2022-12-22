using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorSchool.Components.Web.UI;
public class BlazorCapturePdfButton : TargetTokenize, IThemable
{
    [CascadingParameter]
    private BlazorCaptureElement? CascadedBlazorCaptureElement { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnPopupBlocked { get; set; } = EventCallback.Empty;

    [Parameter]
    public EventCallback OnClick { get; set; } = EventCallback.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    private Lazy<IJSObjectReference> _blazorCaptureElementModule = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_blazorCaptureElementModule.IsValueCreated)
        {
            _blazorCaptureElementModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorSchool.Components.Web/BlazorCaptureElement.min.js"));
        }

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "onclick", "type");
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        builder.AddAttribute(1, "type", "button");
        builder.AddMultipleAttributes(2, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorCapturePdfButton)));
        builder.AddAttribute(3, "onclick", CapturePdfAsync);
        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }

    [JSInvokable]
    public void RaisePopupBlockedError() => OnPopupBlocked.InvokeAsync();

    private async Task CapturePdfAsync()
    {
        await OnClick.InvokeAsync();
        if (_blazorCaptureElementModule.IsValueCreated)
        {
            string? token = TargetToken;

            if (CascadedBlazorCaptureElement is not null)
            {
                token = CascadedBlazorCaptureElement.Token;
            }

            var wrappedInstance = DotNetObjectReference.Create(this);
            await _blazorCaptureElementModule.Value.InvokeVoidAsync("capturePdf", token, wrappedInstance);
        }
    }
}
