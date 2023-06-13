using ITLearning.Course.Core.Contracts;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts.SqlPlayground;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class SqlPlaygroundController : ControllerBase
{
    private readonly ICourseDatabaseRunner _databaseRunner;
    
    public SqlPlaygroundController(ICourseDatabaseRunner databaseRunner)
    {
        _databaseRunner = databaseRunner ?? throw new ArgumentNullException(nameof(databaseRunner));
    }

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpPost("run")]
    public async Task<IActionResult> RunQuery(RunQueryRequest request)
    {
        var user = HttpContext.GetUser();
        var result = await _databaseRunner.GetQueryResult(new SqlRunCommand
        {
            UserId = user.Id,
            CourseId = request.CourseId,
            Query = request.QueryText
        });
        var serializedResult = JsonConvert.SerializeObject(result);
        return Ok(new
        {
            result = serializedResult
        });
    }
}