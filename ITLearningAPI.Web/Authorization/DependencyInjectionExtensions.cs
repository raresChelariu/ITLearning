using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ITLearningAPI.Web.Authorization;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var authorizationSettings = AuthorizationSettings.GetFromConfiguration(configuration);
        services.AddSingleton<IAuthorizationSettings>(authorizationSettings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                              {
                                  options.TokenValidationParameters = new TokenValidationParameters
                                  {
                                      ValidateIssuer = true,
                                      ValidateAudience = true,
                                      ValidateLifetime = true,
                                      ValidateIssuerSigningKey = true,
                                      ValidIssuer = authorizationSettings.Issuer,
                                      ValidAudience = authorizationSettings.Audience,
                                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationSettings.Secret))
                                  };        
                              });

        return services;
    }

}