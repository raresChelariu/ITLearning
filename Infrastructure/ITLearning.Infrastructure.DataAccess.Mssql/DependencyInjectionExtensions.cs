﻿using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataAccessMssql(this IServiceCollection services)
    {
        services.AddSingleton<IVideoRepository, SqlServerVideoRepository>();
        services.AddSingleton<IUserRepository, SqlServerUserRepository>();
        return services;
    } 
}