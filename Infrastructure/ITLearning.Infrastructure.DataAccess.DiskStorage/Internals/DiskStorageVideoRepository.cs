using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;

namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Internals;

internal class DiskStorageVideoRepository : IVideoRepository
{
    private readonly IVideoFileRepository _videoFileRepository;
    private readonly IVideoDetailsRepository _videoDetailsRepository;

    public DiskStorageVideoRepository(IVideoFileRepository videoFileRepository, IVideoDetailsRepository videoDetailsRepository)
    {
        _videoFileRepository = videoFileRepository;
        _videoDetailsRepository = videoDetailsRepository;
    }
    
    public async Task<long> InsertVideo(Video video)
    {
        video.Filename = Path.GetRandomFileName();

        var videoId = await _videoDetailsRepository.VideoDetailsInsert(video);
        
        if (videoId != -1)
        {
            await _videoFileRepository.SaveVideoFile(video.Filename, video.Content);    
        }

        return videoId;
    }
    
    public async Task<Video> GetVideoContentById(long videoId)
    {
        var video = await _videoDetailsRepository.GetVideoDetails(videoId);
        if (video is not null)
        {
            video.Content = await _videoFileRepository.ReadVideoFile(video.Filename);
        }
        return video;
    }

    public async Task<VideoItemDetails> GetVideoItemDetailsById(long videoId)
    {
        return await _videoDetailsRepository.GetVideoItemDetailsById(videoId);
    }

    
}