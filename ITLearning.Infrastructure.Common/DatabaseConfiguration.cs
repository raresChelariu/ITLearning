using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ITLearning.Infrastructure.Common;

internal class DatabaseConfiguration : IDatabaseConfiguration
{
    public const string ConfigurationKey = "Database";

    public string ConnectionString { get; set; }

    public SqlConnection GetSqlConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    public static DatabaseConfiguration GetFromConfiguration(IConfiguration configuration)
    {
        return configuration.GetSection(ConfigurationKey).Get<DatabaseConfiguration>();
    }
}