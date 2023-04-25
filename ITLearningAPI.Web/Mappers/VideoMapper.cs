using ITLearning.Domain.Models;

namespace ITLearningAPI.Web.Mappers;

public class VideoMapper
{
    public static async Task<Video> ToVideoAsync(IFormFile file)
    {
        var fileName = file.FileName;
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        var video = new Video
        {
            Content = fileBytes,
            Name = fileName
        };
        return video;
    }
}