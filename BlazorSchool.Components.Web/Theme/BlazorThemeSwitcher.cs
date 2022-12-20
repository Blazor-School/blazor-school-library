using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.Theme;
public class BlazorThemeSwitcher : TargetTokenize, IDisposable
{
    [Parameter]
    public RenderFragment<BlazorThemePack>? ChildContent { get; set; }

    [CascadingParameter]
    private BlazorApplyTheme? CascadedBlazorApplyTheme { get; set; }

    private BlazorThemePack CurrentThemePack => GetCurrentThemePack();
    private BlazorApplyTheme? _currentBlazorApplyTheme;
    private bool _subscribedBlazorApplyTheme = false;

    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, ChildContent?.Invoke(CurrentThemePack));

    protected override void OnParametersSet()
    {
        if (CascadedBlazorApplyTheme is null && string.IsNullOrEmpty(TargetToken))
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
        _currentBlazorApplyTheme = CascadedBlazorApplyTheme;

        if (CascadedBlazorApplyTheme is null)
        {
            try
            {
                _currentBlazorApplyTheme = TokenizeResolver.Resolve<BlazorApplyTheme>(TargetToken);
                _currentBlazorApplyTheme.OnComponentUpdated += OnBlazorApplyThemeUpdate;
                _subscribedBlazorApplyTheme = true;
            }
            // When the BlazorApplyTheme hasn't initiated yet, we ignore
            catch (InvalidOperationException) { }
        }

        return new()
        {
            BlazorApplyTheme = _currentBlazorApplyTheme,
            Author = _currentBlazorApplyTheme.CurrentThemePack.Author,
            Name = _currentBlazorApplyTheme.CurrentThemePack.Name,
            Themes = _currentBlazorApplyTheme.CurrentThemePack.Themes.Select(t => t.Name).ToList()
        };
    }

    private void OnBlazorApplyThemeUpdate(object? sender, EventArgs args) => StateHasChanged();

    public void Dispose()
    {
        if (_subscribedBlazorApplyTheme && _currentBlazorApplyTheme is not null)
        {
            _currentBlazorApplyTheme.OnComponentUpdated -= OnBlazorApplyThemeUpdate;
        }
    }
}
