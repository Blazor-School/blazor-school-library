using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.Core.Tokenize;
public abstract class TokenizeComponent : ComponentBase, IDisposable
{
    [Parameter]
    public string Token { get; set; } = GenerateToken();

    [Inject]
    private TokenizeResolver TokenizeResolver { get; set; } = default!;

    protected string TokenAttributeKey = "data-blazor-token";
    internal event EventHandler OnComponentUpdated = (sender, args) => { };

    protected void RegisterTokenize() => TokenizeResolver.AddTokenizeComponent(Token, this);

    protected override void OnInitialized()
    {
        RegisterTokenize();
        NotifyComponentUpdated();
    }

    private static string GenerateToken() => Guid.NewGuid().ToString("N").ToUpper();
    public void UnregisterTokenize() => TokenizeResolver.RemoveTokenizeComponent(Token);
    public void NotifyComponentUpdated() => OnComponentUpdated?.Invoke(this, EventArgs.Empty);
    public virtual void Dispose() => UnregisterTokenize();
}