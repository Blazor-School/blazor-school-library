using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorSchool.Components.Web.Tests.TestCore;

internal static class ArrayRangeExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(this ArrayRange<T> source) =>
        // This is very allocatey, hence it only existing in test code.
        // If we need a way to enumerate ArrayRange in product code, we should
        // consider adding an AsSpan() method or a struct enumerator.
        new ArraySegment<T>(source.Array, 0, source.Count);
}