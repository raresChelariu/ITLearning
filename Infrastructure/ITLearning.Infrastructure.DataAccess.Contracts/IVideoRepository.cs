using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface IVideoRepository
{
    Task<long> InsertVideo(Video video);
    Task<Video> GetVideoContentById(long videoId);
    Task<VideoItemDetails> GetVideoItemDetailsById(long videoId);
}