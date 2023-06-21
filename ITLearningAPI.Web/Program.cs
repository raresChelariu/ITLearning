using System.Diagnostics;
using ITLearningAPI.Web;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Middleware;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilogLogging();

builder.Services.AddHttpClient("Internal", httpClient =>
{
    var internalUrl = builder.Configuration["InternalUrl"] ?? throw new ArgumentException(nameof(builder.Configuration));
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
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = AuthorizationSchemas.MixedSchema;
        options.DefaultScheme = AuthorizationSchemas.MixedSchema;
        options.DefaultChallengeScheme = AuthorizationSchemas.MixedSchema;
    })
    .AddMixedJwtCookieAuthentication();

builder.Services.AddAuthorization(options =>
{
    options.AddItLearningPolicies();
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(ep => ep.MapControllers());
app.UseMiddleware<RequestLoggingMiddleware>();
app.Run();
