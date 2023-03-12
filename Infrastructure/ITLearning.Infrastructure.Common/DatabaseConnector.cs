using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.TypeGuards;
using Microsoft.Data.SqlClient;

namespace ITLearning.Infrastructure.DataAccess.Common;

internal class DatabaseConnector : IDatabaseConnector
{
    private readonly IDatabaseConfiguration _databaseConfiguration;

    public DatabaseConnector(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = TypeGuard.ThrowIfNull(databaseConfiguration);
    }

    public SqlConnection GetSqlConnection() => new(_databaseConfiguration.ConnectionString);
}