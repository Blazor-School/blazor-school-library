using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorSchool.Components.Web.Tests.TestCore;
public readonly struct CustomRenderTreeDiff
{
    /// <summary>
    /// Gets the ID of the component.
    /// </summary>
    public readonly int ComponentId;

    /// <summary>
    /// Gets the changes to the render tree since a previous state.
    /// </summary>
    public readonly ArrayBuilderSegment<RenderTreeEdit> Edits;

    internal CustomRenderTreeDiff(
        int componentId,
        ArrayBuilderSegment<RenderTreeEdit> entries)
    {
        ComponentId = componentId;
        Edits = entries;
    }
}