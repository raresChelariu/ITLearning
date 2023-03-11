using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

internal class MssqlVideoRepository : IVideoRepository
{
    private readonly IDatabaseConnector _databaseConnector;

    public MssqlVideoRepository(IDatabaseConnector databaseConnector)
    {
        _databaseConnector = databaseConnector;
    }
}