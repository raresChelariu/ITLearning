using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

public static class MsSqlDiEx
{
    public static IServiceCollection AddDataAccessMssql(this IServiceCollection services)
    {
        services.AddSingleton<IVideoRepository, SqlServerVideoRepository>();
        services.AddSingleton<IUserRepository, SqlServerUserRepository>();
        services.AddSingleton<ICourseRepository, SqlServerCourseRepository>();
        services.AddSingleton<ICourseScriptRepository, SqlServerCourseScriptRepository>();
        services.AddSingleton<IQuizRepository, SqlServerQuizRepository>();
        services.AddSingleton<ICourseWikiRepository, SqlServerCourseWikiRepository>();
        services.AddSingleton<ICourseItemRepository, SqlServerCourseItemRepository>();
        return services;
    } 
}