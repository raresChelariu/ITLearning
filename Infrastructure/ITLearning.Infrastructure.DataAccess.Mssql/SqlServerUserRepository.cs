using System.Data;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

public class SqlServerUserRepository : IUserRepository
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
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
            command.Parameters.AddWithValue("@Role", (short) user.Role);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@Id"
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

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        const string query = "SELECT [ID], [Username], [PasswordHash], [PasswordSalt], [Email], [Role] FROM [dbo].[Users] WHERE [User] = @Username";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            command.Parameters.AddWithValue("@Username", username);

            var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var user = CreateUserFromReader(reader);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetUserByUsernameAsync), ex);
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
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetUserByUsernameAsync), ex);
            return null;
        }
    }

    private static User CreateUserFromReader(SqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetFromColumn<long>("Id"),
            Username = reader.GetFromColumn<string>("Username"),
            PasswordSalt = reader.GetFromColumn<byte[]>("PasswordHash"),
            PasswordHash = reader.GetFromColumn<byte[]>("PasswordSalt"),
            Email = reader.GetFromColumn<string>("Email"),
            Role = (UserRole) reader.GetFromColumn<short>("Role")
        };
    }
}