using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Reflection;

namespace BlazorSchool.Components.Web.Tests.TestCore;
public class CapturedBatch
{
    public IDictionary<int, List<RenderTreeDiff>> DiffsByComponentId { get; }
        = new Dictionary<int, List<RenderTreeDiff>>();

    public IList<RenderTreeDiff> DiffsInOrder { get; }
        = new List<RenderTreeDiff>();

    public IList<int>? DisposedComponentIDs { get; set; }
    public RenderTreeFrame[]? ReferenceFrames { get; set; }

    public IEnumerable<RenderTreeFrame> GetComponentFrames<T>() where T : IComponent
        => ReferenceFrames.Where(f => f.FrameType == RenderTreeFrameType.Component && f.Component is T);

    public IEnumerable<RenderTreeDiff> GetComponentDiffs<T>() where T : IComponent
        => GetComponentFrames<T>().SelectMany(f => DiffsByComponentId[f.ComponentId]);

    internal void AddDiff(RenderTreeDiff diff)
    {
        int componentId = diff.ComponentId;
        if (!DiffsByComponentId.ContainsKey(componentId))
        {
            DiffsByComponentId.Add(componentId, new List<RenderTreeDiff>());
        }

        // Clone the diff, because its underlying storage will get reused in subsequent batches
        //var cloneBuilder = new ArrayBuilder<RenderTreeEdit>();
        //cloneBuilder.Append(diff.Edits.ToArray(), 0, diff.Edits.Count);
        //var diffClone = new RenderTreeDiff(
        //    diff.ComponentId,
        //    cloneBuilder.ToSegment(0, diff.Edits.Count));
        var diffClone = new RenderTreeDiff();
        typeof(RenderTreeDiff).GetField(nameof(diffClone.ComponentId), BindingFlags.Instance | BindingFlags.NonPublic).SetValue(diffClone, diff.ComponentId);
        typeof(RenderTreeDiff).GetField(nameof(diffClone.Edits), BindingFlags.Instance | BindingFlags.NonPublic).SetValue(diffClone, diff.Edits.ToArray());
        DiffsByComponentId[componentId].Add(diffClone);
        DiffsInOrder.Add(diffClone);
    }
}