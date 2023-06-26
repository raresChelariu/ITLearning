namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;

internal interface IVideoFileRepository
{
    public string GenerateVideoFilename();
    public Task SaveVideoFile(string filename, byte[] contents);
    public Task<byte[]> ReadVideoFile(string filename);
}