namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;

public interface IVideoStorageConfiguration
{
    public const string ConfigurationKey = "VideoStorageDiskPath";
    
    public string VideoStorageDiskPath { get; }
}