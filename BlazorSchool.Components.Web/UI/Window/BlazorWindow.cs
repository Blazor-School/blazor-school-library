using BlazorSchool.Components.Web.Core;
using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorSchool.Components.Web.UI.Window;
public class BlazorWindow : TokenizeComponent, IAsyncDisposable
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsWindowVisible { get; set; } = true;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public Lazy<IJSObjectReference> BlazorWindowModule = new();
    private string _cssClass = "";

    protected override void OnParametersSet()
    {
        // Need to rethink this
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

        AttributeUtilities.ThrowsIfContains(AdditionalAttributes, "id");

        if (AdditionalAttributes is not null && AdditionalAttributes.TryGetValue("class", out object? cssClass))
        {
            _cssClass = cssClass?.ToString() ?? "";
        }
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
            BlazorWindowModule = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorSchool.Components.Web/BlazorWindow.min.js"));
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (IsWindowVisible)
        {
            builder.OpenElement(0, "blazor-window");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
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

    public async ValueTask DisposeAsync()
    {
        if (BlazorWindowModule.IsValueCreated)
        {
            await BlazorWindowModule.Value.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}