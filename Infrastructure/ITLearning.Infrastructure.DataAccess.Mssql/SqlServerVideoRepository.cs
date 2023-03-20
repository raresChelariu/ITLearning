using System.Data;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
namespace ITLearning.Infrastructure.DataAccess.Mssql;

internal class SqlServerVideoRepository : IVideoRepository
{
    private readonly ILogger<SqlServerVideoRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerVideoRepository(ILogger<SqlServerVideoRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = TypeGuard.ThrowIfNull(logger);
        _databaseConnector = TypeGuard.ThrowIfNull(databaseConnector);
    }

    public async Task<long> InsertVideoAsync(Video video)
    {
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            var command = new SqlCommand("VideosInsert", connection);
            command.Parameters.AddWithValue("@Name", video.Name);
            command.Parameters.AddWithValue("@Content", video.Content);
            var rowIdParameter = new SqlParameter("RowId", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(rowIdParameter);

            var videoId = -1L;
            await command.ExecuteNonQueryAsync();
            if (command.Parameters["@RowId"].Value != DBNull.Value)
            {
                videoId = (long)command.Parameters["RowId"].Value;
            }

            video.Id = videoId;

            return videoId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(InsertVideoAsync), ex);
            return -1;
        }
    }

    public async Task<IEnumerable<VideoEntry>> GetAllVideosAsync()
    {
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT RowId, Filename FROM Videos");
            var reader = await command.ExecuteReaderAsync();
            var videoEntries = new List<VideoEntry>();
            while (await reader.ReadAsync())
            {
                videoEntries.Add(CreateVideoEntryFromReader(reader));
            }
            return videoEntries;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAllVideosAsync), ex);
            return null;
        }
    }

    private static VideoEntry CreateVideoEntryFromReader(SqlDataReader reader)
    {
        var videoEntry = new VideoEntry
        {
            Id = reader.GetFromColumn<long>("RowId"),
            Name = reader.GetFromColumn<string>("Filename")
        };
        return videoEntry;
    }
}