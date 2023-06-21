using System.Data;
using Dapper;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModelMapping;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerCourseItemRepository : ICourseItemRepository
{
    private readonly ILogger<SqlServerCourseItemRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;
    
    public SqlServerCourseItemRepository(
        ILogger<SqlServerCourseItemRepository> logger,
        IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }
    
    public async Task<ItemDetail> GetItemDetailById(long itemId)
    {
        const string query = "SELECT ItemID, ItemTitle, ItemTypeID FROM dbo.ItemTitles WHERE ItemID = @ItemID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                ItemID = itemId
            });
            var result = await connection.QueryFirstOrDefaultAsync<ItemDetailsDbDto>(query, parameters);
            return result.ToItemDetails();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Db failure for {@Operation}! {@Exception}", nameof(GetItemDetailById), ex);
            return null;
        }
    }
    
    public async Task<IEnumerable<ItemDetail>> GetItemDetailsByCourseId(long courseId)
    {
        const string query = "CourseGetTitlesByCourseID";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            var parameters = new DynamicParameters(new
            {
                CourseID = courseId
            });
            var result = await connection.QueryAsync<ItemDetailsDbDto>(query, parameters, null, null, CommandType.StoredProcedure);
            return result?.Select(x => x.ToItemDetails());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Db failure for {@Operation}! {@Exception}", nameof(GetItemDetailsByCourseId), ex);
            return null;
        }
    }
}