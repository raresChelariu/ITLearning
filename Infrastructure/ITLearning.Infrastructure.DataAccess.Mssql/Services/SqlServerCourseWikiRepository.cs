using System.Data;
using Dapper;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerCourseWikiRepository : ICourseWikiRepository
{
    private readonly ILogger<SqlServerCourseWikiRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;
    
    public SqlServerCourseWikiRepository(ILogger<SqlServerCourseWikiRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }
    
    public async Task<long> CreateWiki(CourseWiki wiki)
    {
        const string query = "CourseWikiInsert";

        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                CourseID = wiki.CourseId,
                WikiText = wiki.CourseWikiText,
                WikiTitle = wiki.Title
            });
            var result = await connection.QueryFirstOrDefaultAsync<long>(query, parameters, null, null, CommandType.StoredProcedure);
            if (result == default)
            {
                _logger.LogError("Failed to get {@ItemId} on {@Operation}", result, nameof(CreateWiki));
                return -1;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Db failure for {@Operation}! {@Exception}", nameof(CreateWiki), ex);
            return -1;
        }
    }

    public async Task<CourseWiki> GetWikiByItemId(long itemId)
    {
        const string query = "SELECT ItemID, CourseWikiText FROM CourseWikis WHERE ItemID = @ItemId";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                ItemId = itemId 
            });
            var result = await connection.QueryFirstOrDefaultAsync<CourseWiki>(query, parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Db failure for {@Operation}! {@Exception}", nameof(GetWikiByItemId), ex);
            return null;
        }
    }
}