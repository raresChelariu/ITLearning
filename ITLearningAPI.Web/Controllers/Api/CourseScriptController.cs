using System.Text;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CourseScriptController : ControllerBase
{
    private readonly ICourseScriptRepository _scriptRepository;
    
    public CourseScriptController(ICourseScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository ?? throw new ArgumentNullException(nameof(scriptRepository));
    }

    [Authorize(Policy = "AdminOrTeacher")]
    [HttpPost]
    public async Task<IActionResult> CreateScript([FromForm] IFormFile fileScript, [FromForm] long courseId)
    {
        var scriptText = await ReadAsStringAsync(fileScript);
        var courseScript = new CourseScript
        {
            CourseId = courseId,
            ScriptText = scriptText
        };
        var scriptId = await _scriptRepository.CreateScript(courseScript);
        return Created("/api/coursescript", new
        {
            scriptId
        });
    }
    
    private static async Task<string> ReadAsStringAsync(IFormFile file)
    {
        var result = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(await reader.ReadLineAsync()); 
        }
        return result.ToString();
    }
}