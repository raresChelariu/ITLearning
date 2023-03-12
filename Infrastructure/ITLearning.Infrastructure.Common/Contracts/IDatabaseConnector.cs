using Microsoft.Data.SqlClient;

namespace ITLearning.Infrastructure.DataAccess.Common.Contracts;

public interface IDatabaseConnector
{
    SqlConnection GetSqlConnection();
}