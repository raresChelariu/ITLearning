using ITLearning.Domain;

namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;

public interface IVideoDetailsRepository
{
    Task<Video> GetVideoDetails(long videoId);
    Task<long> VideoDetailsInsert(Video video);
    Task<VideoItemDetails> GetVideoItemDetailsById(long videoId);
}