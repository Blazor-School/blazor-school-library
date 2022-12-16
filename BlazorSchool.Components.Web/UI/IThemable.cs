using BlazorSchool.Components.Web.Theme;
using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.UI;

public interface IThemable
{
    [CascadingParameter]
    public BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }
}
