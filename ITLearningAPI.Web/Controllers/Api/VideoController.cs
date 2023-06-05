using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly IVideoRepository _videoRepository;

    public VideoController(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string title, [FromForm] long courseId)
    {
        var content = await FileToByteArray(file);
        var video = new Video
        {
            CourseId = courseId,
            Title = title,
            ContentType = file.ContentType,
            Content = content
        };
        var itemId = await _videoRepository.InsertVideo(video);
        return Ok(new 
        {
            ItemId = itemId
        });
    }
    
    [HttpGet]
    public async Task<IResult> GetById([FromQuery] long videoId)
    {
        var video = await _videoRepository.GetVideoContentById(videoId);
        var memoryStream = new MemoryStream(video.Content);
        return Results.File(memoryStream, video.ContentType);
    }

    private static async Task<byte[]> FileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}