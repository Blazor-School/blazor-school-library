using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.Core.Tokenize;
public abstract class TokenizeComponent : ComponentBase
{
    [Parameter]
    public string? Token { get; set; } = GenerateToken();

    protected string TokenAttributeKey = "data-blazor-token";

    private static string GenerateToken() => Guid.NewGuid().ToString("N").ToUpper();
}