using ITLearningAPI.Web;
using ITLearningAPI.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Internal", httpClient =>
{
    var internalUrl = builder.Configuration["InternalUrl"];
    httpClient.BaseAddress = new Uri(internalUrl);
});

builder.Services
    .AddControllers()
    .AddControllersAsServices();

builder.Services.AddRouting();

builder.Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddItLearningServices(builder.Configuration);

builder.Services
    .AddAuthorizationDependencies(builder.Configuration)
    .AddAuthentication()
    .AddMixedJwtCookieAuthentication();

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(ep => ep.MapControllers());

app.Run();
