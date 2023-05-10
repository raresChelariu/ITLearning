using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace ITLearningAPI.Web.Authorization;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthJwtOrCookie(this IServiceCollection services, IConfiguration configuration)
    {
        var authorizationSettings = AuthorizationSettings.GetFromConfiguration(configuration);
        services.AddSingleton<IAuthorizationSettings>(authorizationSettings);

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = authorizationSettings.Audience,
                    ValidIssuer = authorizationSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationSettings.Secret))
                };
            })
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return JwtBearerDefaults.AuthenticationScheme;

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

        return services;
    }

}