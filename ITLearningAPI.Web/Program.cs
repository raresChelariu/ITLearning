using System.Security.Claims;
using ITLearningAPI.Web;
using ITLearningAPI.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Internal", httpClient =>
{
    var internalUrl = builder.Configuration["InternalUrl"] ?? throw new ArgumentNullException(nameof(builder.Configuration));
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
    options.AddPolicy("Teacher", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Teacher");
    });
    options.AddPolicy("Administrator", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Administrator");
    });
    options.AddPolicy("Student", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Student");
    });
    options.AddPolicy("AdminOrTeacher", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Administrator", "Teacher");
    });
    options.AddPolicy("AdminOrStudent", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Administrator", "Student");
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(ep => ep.MapControllers());

app.Run();
