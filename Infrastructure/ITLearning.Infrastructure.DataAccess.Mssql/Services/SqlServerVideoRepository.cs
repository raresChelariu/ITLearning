using System.Data;
using Dapper;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerVideoRepository : IVideoRepository
{
    private readonly ILogger<SqlServerVideoRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerVideoRepository(ILogger<SqlServerVideoRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }

    public async Task<long> InsertVideo(Video video)
    {
        const string query = "VideoInsert";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            var command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            await connection.OpenAsync();
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "CourseID",
                DbType = DbType.Int64,
                Value = video.CourseId
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "ItemTitle",
                DbType = DbType.String,
                Value = video.Title
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "ContentType",
                DbType = DbType.String,
                Value = video.ContentType
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "Content",
                DbType = DbType.Binary,
                Value = video.Content
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "ItemID",
                DbType = DbType.Int64,
                Direction = ParameterDirection.Output
            });
            await command.ExecuteNonQueryAsync();
            var itemId = (long) command.Parameters["ItemID"].Value;
            if (itemId == default)
            {
                _logger.LogError("No ItemId fetched for operation {@Operation} which received {@Video}",
                    nameof(InsertVideo), video);
            }
            return itemId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(InsertVideo), ex);
            return -1;
        }
    }

    public async Task<VideoItemDetails> GetVideoItemDetailsById(long videoId)
    {
        const string query = "SELECT V.ItemID, V.ContentType, IT.ItemTitle Title FROM dbo.Videos V JOIN ItemTitles IT ON V.ItemID = IT.ItemID WHERE V.ItemID = @ItemID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                ItemID = videoId
            });
            var result = await connection.QueryFirstOrDefaultAsync<VideoItemDetails>(query, parameters);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(InsertVideo), ex);
            return null;
        }
    } 
    
    public async Task<Video> GetVideoContentById(long videoId)
    {
        const string query = "SELECT DATALENGTH(FileContent) ContentSize, FileContent Content, ContentType FROM dbo.Videos WHERE ItemID = @ItemID";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            var command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "ItemID",
                SqlDbType = SqlDbType.BigInt,
                Value = videoId
            });
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            if (!reader.Read())
            {
                return null;
            }

            var content = (byte[]) reader["Content"];
            // var contentSize = (long) reader["ContentSize"];
            // var content = new byte[contentSize];
            // _ = reader.GetBytes(1, 0L, content, 0, (int) contentSize);
            return new Video
            {
                Content = content, 
                ContentType = (string) reader["ContentType"] 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetVideoContentById), ex);
            return null;
        }
    }
}