using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSchool.Components.Web.Core;
public static class BlazorComponentServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorComponent(this IServiceCollection services)
    {
        _ = services.AddScoped<TokenizeResolver>();
        //services.AddHttpClient("_internal_blazor_library_http", (sp, _httpClient) =>
        //{
        //    var navigationManager = sp.GetRequiredService<NavigationManager>();
        //    _httpClient.BaseAddress = new(navigationManager.BaseUri);
        //});
        services.AddHttpClient("__internal_blazor_library_http");

        return services;
    }
}