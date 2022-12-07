namespace HandFootExcluded.Common;

public interface IDependencyExtension
{
    IServiceCollection Initialize(IServiceCollection services);
}

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddExtension<TExtension>(this IServiceCollection serviceCollection) where TExtension : IDependencyExtension
    {
        var extenstion = Activator.CreateInstance<TExtension>();
        return extenstion.Initialize(serviceCollection);
    }
}
