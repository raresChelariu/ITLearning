using ITLearningAPI.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddItLearningServices(builder.Configuration);

builder.Services
       .AddAuthentication()
       .AddJwtBearer(options =>
       {
           var secret = builder.Configuration["authorization:Secret"] ?? throw new ArgumentNullException(nameof(options));
           options.SaveToken = true;
           options.RequireHttpsMetadata = false;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidAudience = builder.Configuration["authorization:Audience"],
               ValidIssuer = builder.Configuration["authorization:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
           };
       });

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
