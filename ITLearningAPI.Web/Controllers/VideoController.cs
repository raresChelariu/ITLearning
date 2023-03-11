using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload(List<IFormFile> files)
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

        return Ok(new { count = files.Count(), size });
    }


}