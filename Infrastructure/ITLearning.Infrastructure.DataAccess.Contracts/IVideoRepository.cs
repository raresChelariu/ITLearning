using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface IVideoRepository
{
    Task<long> InsertVideoAsync(Video video);
    Task<IEnumerable<VideoEntry>> GetAllVideosAsync();
}