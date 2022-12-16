using BlazorSchool.Components.Web.Core.Tokenize;
using BlazorSchool.Components.Web.Theme.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.Theme;
public class BlazorApplyTheme : TokenizeComponent
{
    [Parameter]
    public string ThemePackPath { get; set; } = "";

    [Parameter]
    public string InitialTheme { get; set; } = "";

    [Parameter]
    public RenderFragment<ComponentCssProvider>? ChildContent { get; set; }

    [Parameter]
    public string? ExtendedThemePackPath { get; set; } = "";

    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public string? this[string key] => CurrentCssProvider[key];

    internal ThemePack CurrentThemePack { get; set; } = new();
    internal ThemeDefinition? CurrentTheme { get; set; } = new();
    internal ComponentCssProvider CurrentCssProvider { get; set; } = new();
    internal ThemePack ThemePackBase { get; set; } = new();
    internal ThemePack ExtendedThemePack { get; set; } = new();

    // Use SetParametersAsync to keep high performance
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var httpClient = HttpClientFactory.CreateClient("__internal_blazor_library_http");
        httpClient.BaseAddress = new(NavigationManager.BaseUri);
        var parameterDictionary = parameters.ToDictionary();

        TokenUpdate(parameterDictionary);
        await ThemePackPathUpdate(httpClient, parameterDictionary);
        await ExtendedThemePackPathUpdate(httpClient, parameterDictionary);
        MergeTheme();
        InitialThemeUpdate(parameterDictionary);
        ChildContentUpdate(parameterDictionary);

        await base.SetParametersAsync(ParameterView.Empty);
    }

    private void TokenUpdate(IReadOnlyDictionary<string, object> parameterDictionary)
    {
        _ = parameterDictionary.TryGetValue(nameof(Token), out object? newTokenObj);
        string? newToken = newTokenObj as string;

        if (Token != newToken && !string.IsNullOrEmpty(newToken))
        {
            Token = newToken;
            RegisterTokenize();
        }
    }

    // Don't allow Tokenizer initialized. Otherwise, the component will be registered twice.
    protected override void OnInitialized() => NotifyComponentUpdated();

    private void ChildContentUpdate(IReadOnlyDictionary<string, object> parameterDictionary)
    {
        _ = parameterDictionary.TryGetValue(nameof(ChildContent), out object? newChildContentObj);
        var newChildContent = newChildContentObj as RenderFragment<ComponentCssProvider>;
        if (ChildContent != newChildContent)
        {
            ChildContent = newChildContent;
        }
    }

    private void InitialThemeUpdate(IReadOnlyDictionary<string, object> parameterDictionary)
    {
        _ = parameterDictionary.TryGetValue(nameof(InitialTheme), out object? newInitialThemeObj);
        string? newInitialTheme = newInitialThemeObj as string;
        if (string.IsNullOrEmpty(newInitialTheme))
        {
            throw new InvalidOperationException($"The {nameof(InitialTheme)} must not be empty.");
        }

        if (InitialTheme != newInitialTheme)
        {
            InitialTheme = newInitialTheme;
            ChangeTheme(InitialTheme, false);

            if (CurrentTheme is null)
            {
                throw new InvalidOperationException($"The initial theme {InitialTheme} not found.");
            }
        }
    }

    private async Task ExtendedThemePackPathUpdate(HttpClient httpClient, IReadOnlyDictionary<string, object> parameterDictionary)
    {
        _ = parameterDictionary.TryGetValue(nameof(ExtendedThemePackPath), out object? newExtendedConfigPathObj);
        string? newExtendedConfigPath = newExtendedConfigPathObj as string;
        if (ExtendedThemePackPath != newExtendedConfigPath)
        {
            ExtendedThemePackPath = newExtendedConfigPath;

            if (!string.IsNullOrEmpty(ExtendedThemePackPath))
            {
                string extendedThemeConfig = await httpClient.GetStringAsync(ExtendedThemePackPath) ?? throw new InvalidOperationException($"Could not get the theme JSON at {ExtendedThemePackPath}.");
                ExtendedThemePack = ThemePack.FromJson(extendedThemeConfig);
            }
            else
            {
                ExtendedThemePack = new();
            }
        }
    }

    private async Task ThemePackPathUpdate(HttpClient httpClient, IReadOnlyDictionary<string, object> parameterDictionary)
    {
        _ = parameterDictionary.TryGetValue(nameof(ThemePackPath), out object? newThemePackPathObj);
        string? newThemePackPath = newThemePackPathObj as string;
        if (string.IsNullOrEmpty(newThemePackPath))
        {
            throw new InvalidOperationException($"The {nameof(ThemePackPath)} must not be empty.");
        }

        if (ThemePackPath != newThemePackPath)
        {
            ThemePackPath = newThemePackPath;
            string themeConfigJson = await httpClient.GetStringAsync(ThemePackPath) ?? throw new InvalidOperationException($"Could not get the theme JSON at {ThemePackPath}.");
            ThemePackBase = ThemePack.FromJson(themeConfigJson);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<BlazorApplyTheme>>(0);
        builder.AddAttribute(1, "IsFixed", true);
        builder.AddAttribute(2, "Value", this);
        RenderFragment childContentDelegate = RenderChildContent;
        builder.AddAttribute(3, "ChildContent", childContentDelegate);
        builder.CloseComponent();
    }

    private void RenderChildContent(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent?.Invoke(CurrentCssProvider));
        builder.OpenComponent<ImportThemeFiles>(1);
        builder.AddAttribute(2, nameof(ImportThemeFiles.Imports), CurrentTheme?.Imports);
        builder.CloseComponent();
    }

    private void MergeTheme()
    {
        CurrentThemePack = ThemePack.FromThemeConfig(ThemePackBase);
        CurrentThemePack.Name = string.Join(" ", ThemePackBase.Name, ExtendedThemePack.Name);
        CurrentThemePack.Author = string.Join(" ", ThemePackBase.Author, ExtendedThemePack.Author);

        foreach (var theme in ExtendedThemePack.Themes)
        {
            var updatingTheme = CurrentThemePack.Themes.FirstOrDefault(t => t.Name == theme.Name);

            if (updatingTheme is null)
            {
                CurrentThemePack.Themes.Add(theme);
            }
            else
            {
                // We don't care about import duplication. For now
                updatingTheme.Imports.AddRange(theme.Imports);

                foreach (var component in theme.Components)
                {
                    updatingTheme.Components[component.Key] = component.Value;
                }
            }
        }
    }

    internal void ChangeTheme(string name, bool withRender)
    {
        if (CurrentThemePack is null || !CurrentThemePack.Themes.Any())
        {
            throw new InvalidOperationException($"No theme provided.");
        }

        CurrentTheme = CurrentThemePack.Themes.FirstOrDefault(t => t.Name == name);

        if (CurrentTheme is null)
        {
            throw new InvalidOperationException($"Theme name \"{name}\" not found. Select one of the following themes: {string.Join(",", CurrentThemePack.Themes.Select(t => $"\"{t.Name}\""))}");
        }

        CurrentCssProvider.InnerDict = CurrentTheme.Components;

        if (withRender)
        {
            StateHasChanged();
        }
    }
}
