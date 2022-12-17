using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorSchool.Components.Web.UI.Window;
public class BlazorWindow : TokenizeComponent, IThemable
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool? InitialVisibility { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    public Lazy<IJSObjectReference> BlazorWindowModule = new();
    private bool _visibilityState = true;

    protected override void OnInitialized()
    {
        _visibilityState = InitialVisibility ?? true;
        RegisterTokenize();
    }

    protected override void OnParametersSet()
    {
        // The user can only pass BlazorWindowTitle or Content and this check still passes. Need to think for another way to do this or remove it at all.
        if (ChildContent is null)
        {
            throw new InvalidOperationException($"You need to specify both '{nameof(BlazorWindowTitle)}' and '{nameof(BlazorWindowContent)}'.");
        }

        if (AdditionalAttributes is not null && AdditionalAttributes.TryGetValue("style", out object? style))
        {
            string styleAttribute = style?.ToString() ?? "";

            if (styleAttribute.Contains("position"))
            {
                throw new InvalidOperationException("Do not specify position for this component.");
            }
        }

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, TokenAttributeKey);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadModules();
        }
    }

    public async Task LoadModules()
    {
        if (!BlazorWindowModule.IsValueCreated)
        {
            BlazorWindowModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorSchool.Components.Web/BlazorWindow.min.js"));
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (_visibilityState)
        {
            builder.OpenElement(0, HtmlTagUtilities.ToHtmlTag(nameof(BlazorWindow)));
            builder.AddMultipleAttributes(1, AttributeUtilities.Normalized(AdditionalAttributes, CascadedBlazorApplyTheme, nameof(BlazorWindow)));
            builder.AddAttribute(1, "style", "position: absolute;");
            builder.AddAttribute(2, TokenAttributeKey, Token);
            builder.OpenComponent<CascadingValue<BlazorWindow>>(3);
            builder.AddAttribute(4, "IsFixed", true);
            builder.AddAttribute(5, "Value", this);
            builder.AddAttribute(6, "ChildContent", ChildContent);
            builder.CloseComponent();
            builder.CloseElement();
        }
    }

    public void CloseWindow()
    {
        _visibilityState = false;
        StateHasChanged();
    }

    public void OpenWindow()
    {
        _visibilityState = true;
        StateHasChanged();
    }

    public override void Dispose()
    {
        UnregisterTokenize();
        if (BlazorWindowModule.IsValueCreated)
        {
            _ = InvokeAsync(async () => await BlazorWindowModule.Value.DisposeAsync());
        }
    }
}