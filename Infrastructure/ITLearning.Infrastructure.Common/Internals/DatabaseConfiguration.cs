﻿using ITLearning.TypeGuards;
using Microsoft.Extensions.Configuration;

namespace ITLearning.Infrastructure.DataAccess.Common.Internals;

internal class DatabaseConfiguration : IDatabaseConfiguration
{
    private const string ConfigurationKey = "Database";

    public string ConnectionString { get; set; }
    public string ConnectionStringMasterDatabase { get; set; }
    
    public static DatabaseConfiguration GetFromConfiguration(IConfiguration configuration)
    {
        var databaseConfiguration = configuration.GetSection(ConfigurationKey).Get<DatabaseConfiguration>();
        _ = TypeGuard.ThrowIfStringIsNullOrWhitespace(databaseConfiguration.ConnectionString);
        return databaseConfiguration;
    }
}