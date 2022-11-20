using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSchool.Components.Web.UI.Button;
public class ButtonComponent : TokenizeComponent, IAsyncDisposable
{
    public Lazy<IJSObjectReference> ButtonComponentModule = new();
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnOpenClick { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public bool IsWindowOpen { get; set; }
    
protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (IsWindowOpen)
        {
            builder.OpenElement(0, "button");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "onclick", OnOpenClick);
            builder.AddContent(3, ChildContent);
            builder.CloseElement();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (ButtonComponentModule.IsValueCreated)
        {
            await ButtonComponentModule.Value.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);

        }
    }
}