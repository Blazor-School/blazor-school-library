using BlazorSchool.Components.Web.UI.Window;
using Microsoft.AspNetCore.Components;

namespace BlazorSchool.Components.Web.Tests.UI.Window;

public class BlazorWindowTest
{
    [Fact]
    public async void BlazorWindow_ThrowsWhenNoContent()
    {
        // Arrange
        var rootComponent = new GenericHostComponent()
        {
            InnerContent = (builder) =>
            {
                builder.OpenComponent<BlazorWindow>(0);
                builder.CloseComponent();
            }
        };

        // Act: Render component
        var testRenderer = TestRenderer.CreateGenericTestRenderer();
        int componentId = testRenderer.AssignRootComponentId(rootComponent);

        // Assert: An exception is thrown and with defined message
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await testRenderer.RenderRootComponentAsync(componentId));
        Assert.Equal($"You need to specify both '{nameof(BlazorWindowTitle)}' and '{nameof(BlazorWindowContent)}'.", ex.Message);
    }

    [Fact]
    public async void BlazorWindow_ThrowsWhenPositionStylePassed()
    {
        // Arrange
        RenderFragment titleComponent = builder =>
        {
            builder.OpenComponent<BlazorWindowTitle>(1);
            builder.AddContent(2, "Title");
            builder.CloseComponent();
        };

        var rootComponent = new GenericHostComponent()
        {
            InnerContent = (builder) =>
            {
                builder.OpenComponent<BlazorWindow>(0);
                builder.AddAttribute(1, "ChildContent", titleComponent);
                builder.AddAttribute(2, "style", "position:related");
                builder.CloseComponent();
            }
        };

        // Act: Render component
        var testRenderer = TestRenderer.CreateGenericTestRenderer();
        int componentId = testRenderer.AssignRootComponentId(rootComponent);

        // Assert: An exception is thrown and with defined message
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await testRenderer.RenderRootComponentAsync(componentId));
        Assert.Equal("Do not specify position for this component.", ex.Message);
    }

    [Fact]
    public void BlazorWindow_HasImplementedIAsyncDisposable()
    {
        bool hasImplementedIAsyncDisposable = typeof(IAsyncDisposable).IsAssignableFrom(typeof(BlazorWindow));
        Assert.True(hasImplementedIAsyncDisposable);
    }
}