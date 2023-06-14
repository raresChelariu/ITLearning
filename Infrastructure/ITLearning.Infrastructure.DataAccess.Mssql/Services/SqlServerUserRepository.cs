using System.Data;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerUserRepository : IUserRepository
{
    private readonly IDatabaseConnector _databaseConnector;
    private readonly ILogger<SqlServerUserRepository> _logger;

    public SqlServerUserRepository(ILogger<SqlServerUserRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = TypeGuard.ThrowIfNull(logger);
        _databaseConnector = TypeGuard.ThrowIfNull(databaseConnector);
    }

    public async Task<long> InsertUserAsync(User user)
    {
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await using var command = new SqlCommand("UsersInsert", connection);
            command.CommandType = CommandType.StoredProcedure;
            await connection.OpenAsync();
            command.Parameters.Add(new SqlParameter 
            {
                ParameterName = "@Username",
                Value = user.Username,
                Size = 256
            });
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
            command.Parameters.AddWithValue("@Role", (short) user.Role);
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Email",
                Value = user.Email,
                Size = 256
            });
            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@Id",
                SqlDbType = SqlDbType.BigInt
            });
            await command.ExecuteNonQueryAsync();
            var userId = -1L;
            if (command.Parameters["@Id"].Value != DBNull.Value)
            {
                userId = (long) command.Parameters["@Id"].Value;
            }
            user.Id = userId;
            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(InsertUserAsync), ex);
            return -1;
        }
    }

    public async Task<User> GetUserByUserIdentifierAsync(string username)
    {
        const string query = "SELECT [ID], [Username], [PasswordHash], [PasswordSalt], [Email], [Role] " + 
                             "FROM [dbo].[Users] WHERE [Username] = @UserIdentifier OR [Email] = @UserIdentifier";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@UserIdentifier", username);

            var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var user = CreateUserFromReader(reader);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetUserByUserIdentifierAsync), ex);
            return null;
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        const string query = "SELECT [ID], [Username], [PasswordHash], [PasswordSalt], [Email], [Role] FROM [dbo].[Users] WHERE Email = @Email";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@Email", email);

            var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var user = CreateUserFromReader(reader);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetUserByUserIdentifierAsync), ex);
            return null;
        }
    }

    private static User CreateUserFromReader(SqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetFromColumn<long>("Id"),
            Username = reader.GetFromColumn<string>("Username"),
            PasswordHash = reader.GetFromColumn<byte[]>("PasswordHash"),
            PasswordSalt = reader.GetFromColumn<byte[]>("PasswordSalt"),
            Email = reader.GetFromColumn<string>("Email"),
            Role = (UserRole) reader.GetFromColumn<short>("Role")
        };
    }
}