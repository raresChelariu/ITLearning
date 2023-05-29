using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ITLearningAPI.Web.Authorization;

public static class AuthorizationPolicies
{
    public const string Teacher = "Teacher";
    public const string AdminOrTeacher = "AdminOrTeacher";
    public const string AdminOrStudent = "AdminOrStudent";
    public const string User = "User";
    
    public static void AddItLearningPolicies(this AuthorizationOptions options)
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
        options.AddPolicy("User", policy =>
        {
            policy.RequireClaim(ClaimTypes.Role, "Administrator", "Student", "Teacher");
        });
    }
}