using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.Core.Tokenize;

// Should we make the generic? We can register when the target tokenize component update in the base class.
public abstract class TargetTokenize : ComponentBase
{
    [Parameter]
    public string TargetToken { get; set; } = "";

    [Inject]
    internal TokenizeResolver TokenizeResolver { get; set; } = default!;
}
