using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.UI.Accordion;
public class BlazorAccordion : ComponentBase, IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
