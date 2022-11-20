using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSchool.Components.Web.UI.Button;
{
    [Parameter]

    [Parameter]

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    
protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        {
            builder.OpenElement(0, "button");
            builder.CloseElement();
        }
    }
}