using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSchool.Components.Web.Tests.TestCore;
public class AutoRenderFragmentComponent : AutoRenderComponent
{
    private readonly RenderFragment _renderFragment;

    public AutoRenderFragmentComponent(RenderFragment renderFragment)
    {
        _renderFragment = renderFragment;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
        => _renderFragment(builder);
}