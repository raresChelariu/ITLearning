using System.Text;
using ITLearning.Course.Core.Contracts;
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
    private readonly ISqlDatabaseBuilder _sqlDatabaseBuilder;

    public CourseScriptController(ICourseScriptRepository scriptRepository, ISqlDatabaseBuilder sqlDatabaseBuilder)
    {
        _scriptRepository = scriptRepository ?? throw new ArgumentNullException(nameof(scriptRepository));
        _sqlDatabaseBuilder = sqlDatabaseBuilder ?? throw new ArgumentNullException(nameof(sqlDatabaseBuilder));
    }

    [Authorize(Policy = "AdminOrTeacher")]
    [HttpPost]
    public async Task<IActionResult> CreateScript([FromForm] IFormFile fileScript, [FromForm] long courseId)
    {
        var user = HttpContext.GetUser();
        var scriptText = await ReadAsStringAsync(fileScript);
        var courseScript = new CourseScript
        {
            CourseId = courseId,
            ScriptText = scriptText
        };
        var scriptId = await _scriptRepository.CreateScript(courseScript);
        var errorList = await _sqlDatabaseBuilder.CreateDatabaseWithScripts(new DatabaseBuildCommand
        {
            UserId = user.Id, 
            CourseId = courseId,
            SeedingScripts = new List<CourseScript>
            {
                courseScript
            }
        });
        if (errorList is not null && errorList.Count > 0)
        {
            return BadRequest(errorList);
        }
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