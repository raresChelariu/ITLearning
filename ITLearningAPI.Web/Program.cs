using ITLearningAPI.Web;
using ITLearningAPI.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddItLearningServices(builder.Configuration);

builder.Services
       .AddAuthJwtOrCookie(builder.Configuration);

builder.Services.AddOutputCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOutputCache();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
