using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;
    private readonly ICourseRepository _courseRepository;
    
    public CourseController(IStaticAssetResponseService staticAssetResponseService
        , ICourseRepository courseRepository)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    [HttpGet]
    [Route("create")]
    public async Task GetCourseCreate()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CourseCreate.html");
    }
    
    [HttpGet]
    [Route("{id:long}")]
    public async Task GetCoursePage(long id)
    {
        var course = _courseRepository.GetById(id);
        if (course == null)
        {
            await _staticAssetResponseService.RespondWithStaticAsset(Response, "AccessDenied.html");
            return;
        }
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CourseView.html");
    }

}