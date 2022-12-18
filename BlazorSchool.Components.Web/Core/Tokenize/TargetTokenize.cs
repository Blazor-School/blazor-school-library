using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.Core.Tokenize;
public abstract class TargetTokenize : ComponentBase
{
    [Parameter]
    public string TargetToken { get; set; } = "";

    [Inject]
    internal TokenizeResolver TokenizeResolver { get; set; } = default!;
}
