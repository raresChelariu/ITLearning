using Microsoft.Data.SqlClient;

namespace ITLearning.Infrastructure.Common;

public interface IDatabaseConfiguration
{
    public string ConnectionString { get; }

    public SqlConnection GetSqlConnection();
}