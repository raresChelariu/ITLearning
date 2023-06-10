using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IStaticAssetResponseService _saResponseService;
    
    public RegisterController(IStaticAssetResponseService saResponseService)
    {
        _saResponseService = saResponseService ?? throw new ArgumentNullException(nameof(saResponseService));
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task ServeLoginPage()
    {
        await _saResponseService.RespondWithStaticAsset(Response, "Register.html");
    }
}