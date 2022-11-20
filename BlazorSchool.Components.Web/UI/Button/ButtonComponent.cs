using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSchool.Components.Web.UI.Button;
public class ButtonComponent : ComponentBase
{
    [Parameter]
    public bool IsButtonVisible { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public EventCallback OnButtonClick { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (IsButtonVisible)
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            builder.AddAttribute(3, "onclick", OnButtonClick);
            builder.AddContent(4, ChildContent);
            builder.CloseElement();
        }
    }
}
