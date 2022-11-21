using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.Tests.TestCore;
public static class IComponentExtensions
{
    public static void SetParameters(
        this IComponent component,
        Dictionary<string, object> parameters) => component.SetParametersAsync(ParameterView.FromDictionary(parameters));
}