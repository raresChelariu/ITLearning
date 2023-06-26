using ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Internals;

internal class VideoStorageConfiguration : IVideoStorageConfiguration
{
    public string VideoStorageDiskPath { get; set; }

    public static VideoStorageConfiguration GetFromConfiguration(IConfiguration configuration)
    {
        return new VideoStorageConfiguration
        {
            VideoStorageDiskPath = configuration[IVideoStorageConfiguration.ConfigurationKey]
        };
    }
}