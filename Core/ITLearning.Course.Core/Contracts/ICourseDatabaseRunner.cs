using System.Data;

namespace ITLearning.Course.Core.Contracts;

public interface ICourseDatabaseRunner
{
    Task<DataTable> GetQueryResult(SqlRunCommand command);
}