namespace BlazorSchool.Components.Web.Tests.TestCore;
public class TestServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, Func<object>> _factories
        = new();

    public object? GetService(Type serviceType)
        => _factories.TryGetValue(serviceType, out var factory)
            ? factory()
            : null;

    internal void AddService<T>(T value)
        => _factories.Add(typeof(T), () => value);
}