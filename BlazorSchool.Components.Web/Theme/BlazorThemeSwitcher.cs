using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.Theme;
public class BlazorThemeSwitcher : ComponentBase
{
    [Parameter]
    public RenderFragment<BlazorThemePack>? ChildContent { get; set; }

    [Parameter]
    public string TargetToken { get; set; } = "";

    [CascadingParameter]
    private BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    [Inject]
    private TokenizeResolver TokenizeResolver { get; set; } = default!;

    private BlazorThemePack CurrentThemePack => GetCurrentThemePack();

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent?.Invoke(CurrentThemePack));
    }

    protected override void OnParametersSet()
    {
        if(CascadedBlazorApplyTheme is null && string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"You either use {nameof(BlazorThemeSwitcher)} inside {nameof(BlazorApplyTheme)} or privide the {nameof(TargetToken)}.");
        }

        if (CascadedBlazorApplyTheme is not null && !string.IsNullOrEmpty(TargetToken))
        {
            throw new InvalidOperationException($"You cannot specify {nameof(TargetToken)} when using the component {nameof(BlazorThemeSwitcher)} inside {nameof(BlazorApplyTheme)}.");
        }
    }

    private BlazorThemePack GetCurrentThemePack()
    {
        var blazorApplyTheme = CascadedBlazorApplyTheme;

        if (CascadedBlazorApplyTheme is null)
        {
            blazorApplyTheme = TokenizeResolver.Resolve<BlazorApplyTheme>(TargetToken);
        }

        return new()
        {
            BlazorApplyTheme = blazorApplyTheme,
            Author = blazorApplyTheme.CurrentThemePack.Author,
            Name = blazorApplyTheme.CurrentThemePack.Name,
            Themes = blazorApplyTheme.CurrentThemePack.Themes.Select(t => t.Name).ToList()
        };
    }
}
