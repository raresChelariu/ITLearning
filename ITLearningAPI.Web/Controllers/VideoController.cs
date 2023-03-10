using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using ITLearningAPI.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoController : ControllerBase
{
    private readonly IVideoRepository _videoRepository;

    public VideoController(IVideoRepository videoRepository)
    {
        _videoRepository = TypeGuard.ThrowIfNull(videoRepository);
    }

    [HttpPatch]
    public async Task<IActionResult> UploadVideos(List<IFormFile> files)
    {
        var size = files.Sum(f => f.Length);
        foreach (var formFile in files)
        {
            if (formFile.Length <= 0)
            {
                continue;
            }

            var filePath = Path.GetTempFileName();

            await using var stream = System.IO.File.Create(filePath);
            await formFile.CopyToAsync(stream);
        }

        return Ok(new { count = files.Count, size });
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var video = await Mapper.MapToVideoAsync(file);
        await _videoRepository.InsertVideoAsync(video);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allVideoEntries = await _videoRepository.GetAllVideosAsync();
        return new OkObjectResult(allVideoEntries);
    }
}