﻿using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class LogoutController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public LogoutController(IHttpClientFactory httpClientFactory, IStaticAssetResponseService staticAssetResponseService)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [HttpGet]
    public async Task GetLogoutPage()
    {
        var httpClient = _httpClientFactory.CreateClient("Internal");
        await httpClient.PostAsync("/api/user/logout", new StringContent(string.Empty));
        HttpContext.Response.Cookies.Delete("AuthToken");
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Logout.html");
    }
}