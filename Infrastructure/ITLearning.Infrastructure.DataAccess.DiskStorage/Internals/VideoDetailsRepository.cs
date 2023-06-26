using System.Data;
using Dapper;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.DiskStorage.DTOs;
using ITLearning.Infrastructure.DataAccess.DiskStorage.Interfaces;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.DiskStorage.Internals;

public class VideoDetailsRepository : IVideoDetailsRepository
{
    private readonly ILogger<VideoDetailsRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public VideoDetailsRepository(ILogger<VideoDetailsRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = logger;
        _databaseConnector = databaseConnector;
    }

    public async Task<Video> GetVideoDetails(long videoId)
    {
        const string query = "SELECT [ItemID], [ContentType], [Filename] FROM VideoDetails WHERE [ItemID] = @ItemID";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new {
                ItemID = videoId    
            });
            var result = await connection.QueryFirstOrDefaultAsync<VideoDetailDto>(
                query, parameters
            );
            return new Video
            {
                ItemId = result.ItemId,
                ContentType = result.ContentType,
                Filename = result.Filename
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get video with {@VideoId}", videoId);
            return null;
        }
    }

    public async Task<long> VideoDetailsInsert(Video video)
    {
        const string query = "VideoDetailsInsert";
        const string parameterNameItemId = "ItemID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                CourseID = video.CourseId,
                ItemTitle = video.Title,
                video.ContentType,
                video.Filename
            });
            parameters.Add(parameterNameItemId, dbType: DbType.Int64, direction: ParameterDirection.Output);
            _ = await connection.ExecuteAsync(query, parameters, null, null, CommandType.StoredProcedure);
            var itemId = parameters.Get<long>(parameterNameItemId);
            return itemId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert in VideoDetails the {@Video}", video);
            return -1;
        }
    }
    
    public async Task<VideoItemDetails> GetVideoItemDetailsById(long videoId)
    {
        const string query = "SELECT V.ItemID, V.ContentType, IT.ItemTitle Title FROM dbo.VideoDetails V JOIN ItemTitles IT ON V.ItemID = IT.ItemID WHERE V.ItemID = @ItemID";
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
            _logger.LogError(ex,"Db failure for {@Operation}! {@Exception}", nameof(GetVideoItemDetailsById), ex);
            return null;
        }
    }
}