using Microsoft.Data.SqlClient;

namespace ITLearning.Infrastructure.DataAccess.Common.Contracts;

public interface IDatabaseConnector
{
    SqlConnection GetSqlConnection();
    SqlConnection GetSqlConnectionMasterDatabase();
    SqlConnection GetSqlConnectionCustomDatabase(string customDatabase);
}