using BlazorSchool.Components.Web.Core.Tokenize;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSchool.Components.Web.Core;
public static class BlazorComponentServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorComponent(this IServiceCollection services)
    {
        _ = services.AddScoped<TokenizeResolver>();

        return services;
    }
}