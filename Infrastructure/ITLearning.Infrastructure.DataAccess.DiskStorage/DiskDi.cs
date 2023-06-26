using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;
using ITLearning.Infrastructure.DataAccess.DiskStorage.Internals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Infrastructure.DataAccess.DiskStorage;

public static class DiskDi
{
    public static IServiceCollection AddDiskRepositories
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var videoConfiguration = VideoStorageConfiguration.GetFromConfiguration(configuration);
        services.AddSingleton<IVideoRepository, DiskStorageVideoRepository>();
        services.AddSingleton<IVideoStorageConfiguration>(_ => videoConfiguration);
        services.AddSingleton<IVideoFileRepository, VideoFileRepository>();
        services.AddSingleton<IVideoDetailsRepository, VideoDetailsRepository>();
        return services;
    }
}