using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.ExceptionServices;

namespace BlazorSchool.Components.Web.Tests.TestCore;
internal class GenericHostComponent : IComponent
{
    private RenderHandle _renderHandle;

    public void Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;
    public RenderFragment InnerContent { get; set; } = (b) => { };

    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        TriggerRender();
        return Task.CompletedTask;
    }

    // We do it this way so that we don't have to be doing renderer.Invoke on each and every test.
    public void TriggerRender()
    {
        var t = _renderHandle.Dispatcher.InvokeAsync(() => _renderHandle.Render(BuildRenderTree));
        // This should always be run synchronously
        Assert.True(t.IsCompleted);
        if (t.IsFaulted)
        {
            var exception = t.Exception.Flatten().InnerException;

            while (exception is AggregateException e)
            {
                exception = e.InnerException;
            }

            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }

    protected void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, InnerContent);
}
