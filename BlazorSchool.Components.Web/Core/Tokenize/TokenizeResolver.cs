namespace BlazorSchool.Components.Web.Core.Tokenize;
internal class TokenizeResolver
{
    private Dictionary<string, TokenizeComponent> _tokenizeComponentCollection { get; set; } = new();

    public T Resolve<T>(string token) where T : TokenizeComponent
    {
        _ = _tokenizeComponentCollection.TryGetValue(token, out var component);

        return component is not null and T parsedComponent
            ? parsedComponent
            : throw new InvalidOperationException($"Token {token} not found or casting error.");
    }

    public void AddTokenizeComponent(string token, TokenizeComponent tokenizeComponent)
    {
        if (_tokenizeComponentCollection.ContainsKey(token))
        {
            _tokenizeComponentCollection.Remove(token);
            throw new InvalidOperationException($"The token {token} is duplicated. Check if any token is duplicated and reload the page.");
        }

        _tokenizeComponentCollection.Add(token, tokenizeComponent);
    }

    public void RemoveTokenizeComponent(string token) => _ = _tokenizeComponentCollection.Remove(token);
}