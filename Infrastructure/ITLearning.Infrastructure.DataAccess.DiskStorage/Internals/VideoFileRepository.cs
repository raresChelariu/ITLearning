using ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;

namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Internals;

public class VideoFileRepository : IVideoFileRepository
{
    private readonly IVideoStorageConfiguration _videoStorageConfiguration;

    public VideoFileRepository(IVideoStorageConfiguration videoStorageConfiguration)
    {
        _videoStorageConfiguration = videoStorageConfiguration;
    }

    public string GenerateVideoFilename()
    {
        return Path.GetRandomFileName();
    }

    public async Task SaveVideoFile(string filename, byte[] contents)
    {
        var root = _videoStorageConfiguration.VideoStorageDiskPath;
        var filePath = Path.Combine(root, filename);
        await File.WriteAllBytesAsync(filePath, contents);
    }

    public async Task<byte[]> ReadVideoFile(string filename)
    {
        var root = _videoStorageConfiguration.VideoStorageDiskPath;
        var filePath = Path.Combine(root, filename);
        return await File.ReadAllBytesAsync(filePath);
    }
}