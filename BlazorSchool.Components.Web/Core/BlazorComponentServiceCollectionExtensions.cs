using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSchool.Components.Web.Core;
public static class BlazorComponentServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorComponent(this IServiceCollection services)
    {
        _ = services.AddScoped<TokenizeResolver>();
        _ = services.AddHttpClient("__internal_blazor_library_http");

        return services;
    }
}