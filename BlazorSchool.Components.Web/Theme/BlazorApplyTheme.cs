using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.Theme;
public class BlazorApplyTheme : TokenizeComponent
{
    [Parameter]
    public string ThemeConfigPath { get; set; } = "";

    [Parameter]
    public string InitialTheme { get; set; } = "";

    [Parameter]
    public RenderFragment<Dictionary<string, string>>? ChildContent { get; set; }

    [Parameter]
    public string ExtendedConfigPath { get; set; } = "";

    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    internal ThemePack CurrentThemePack { get; set; } = new();
    internal ThemeDefinition CurrentTheme { get; set; } = new();
    internal ThemePack ThemePackBase { get; set; } = new();
    internal ThemePack ExtendedThemeConfig { get; set; } = new();

    // Use SetParametersAsync to keep high performance
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        _ = parameters.TryGetValue(nameof(ThemeConfigPath), out string? newThemeConfigPath);
        if (string.IsNullOrEmpty(newThemeConfigPath))
        {
            throw new InvalidOperationException($"The {nameof(ThemeConfigPath)} must not be empty.");
        }

        _ = parameters.TryGetValue(nameof(InitialTheme), out string? newInitialTheme);
        if (string.IsNullOrEmpty(newInitialTheme))
        {
            throw new InvalidOperationException($"The {nameof(InitialTheme)} must not be empty.");
        }

        var httpClient = HttpClientFactory.CreateClient("__internal_blazor_library_http");
        httpClient.BaseAddress = new(NavigationManager.BaseUri);
        foreach (var parameter in parameters)
        {
            switch (parameter.Name)
            {
                case nameof(ThemeConfigPath):

                    if (ThemeConfigPath != newThemeConfigPath)
                    {
                        ThemeConfigPath = newThemeConfigPath;
                        string themeConfigJson = await httpClient.GetStringAsync(ThemeConfigPath) ?? throw new InvalidOperationException($"Could not get the theme JSON at {ThemeConfigPath}.");
                        ThemePackBase = ThemePack.FromJson(themeConfigJson);
                        MergeTheme();
                    }

                    break;

                case nameof(ExtendedConfigPath):

                    if (parameter.Value is string newExtendedConfigPath && ExtendedConfigPath != newExtendedConfigPath)
                    {
                        ExtendedConfigPath = newExtendedConfigPath;
                        string extendedThemeConfig = await httpClient.GetStringAsync(ExtendedConfigPath) ?? throw new InvalidOperationException($"Could not get the theme JSON at {ExtendedConfigPath}.");
                        ExtendedThemeConfig = ThemePack.FromJson(extendedThemeConfig);
                        MergeTheme();
                    }

                    break;
                case nameof(InitialTheme):

                    if (InitialTheme != newInitialTheme)
                    {
                        InitialTheme = newInitialTheme;
                        ChangeTheme(InitialTheme, false);

                        if (CurrentTheme is null)
                        {
                            throw new InvalidOperationException($"The initial theme {InitialTheme} not found.");
                        }
                    }

                    break;

                case nameof(ChildContent):

                    if (parameter.Value is RenderFragment<Dictionary<string, string>> newChildContent && ChildContent != newChildContent)
                    {
                        ChildContent = newChildContent;
                    }

                    break;
            }
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<BlazorApplyTheme>>(0);
        builder.AddAttribute(1, "IsFixed", true);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent?.Invoke(CurrentTheme.Components));
        builder.CloseComponent();
    }

    private void MergeTheme()
    {
        CurrentThemePack = ThemePack.FromThemeConfig(ThemePackBase);

        CurrentThemePack.Author = string.Join(" ", ThemePackBase.Author, ExtendedThemeConfig.Author);

        // improve this in the future, more strictly when merging theme, no duplication imports
        var baseThemeNames = ThemePackBase.Themes.Select(t => t.Name).ToList();
        var addingThemes = ExtendedThemeConfig.Themes.Where(t => !baseThemeNames.Any(name => t.Name == name)).ToList();
        var updatingThemes = ExtendedThemeConfig.Themes.Where(t => baseThemeNames.Any(name => t.Name == name)).ToList();
        ThemePackBase.Themes.AddRange(addingThemes);

        //foreach (var theme in updatingThemes)
        //{
        //    var baseTheme = ThemeConfigBase.Themes.First(t => t.Name == theme.Name);
        //    baseTheme.Imports.AddRange(theme.Imports);
        //}
    }

    internal void ChangeTheme(string name, bool withRender)
    {
        if (CurrentThemePack is null || !CurrentThemePack.Themes.Any())
        {
            throw new InvalidOperationException($"No theme provided.");
        }

        CurrentTheme = CurrentThemePack.Themes.FirstOrDefault(t => t.Name == name) ?? new();

        if (CurrentTheme is null)
        {
            throw new InvalidOperationException($"Theme name {name} not found.");
        }

        if (withRender)
        {
            StateHasChanged();
        }
    }
}
