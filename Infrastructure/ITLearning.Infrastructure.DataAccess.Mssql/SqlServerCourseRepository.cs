using System.Data;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

internal class SqlServerCourseRepository : ICourseRepository
{
    private readonly ILogger<SqlServerUserRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerCourseRepository(ILogger<SqlServerUserRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = TypeGuard.ThrowIfNull(logger);
        _databaseConnector = TypeGuard.ThrowIfNull(databaseConnector);
    }

    public async Task<long> Insert(Course course)
    {
        const string query = "CoursesInsert";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Name",
                Value = course.Name,
                SqlDbType = SqlDbType.VarChar,
                Size = 128
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@AuthorID",
                Value = course.AuthorId
            });
            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@Id",
                SqlDbType = SqlDbType.BigInt
            });
            await command.ExecuteNonQueryAsync();
            
            var courseId = -1L;
            if (command.Parameters["@ID"].Value != DBNull.Value)
            {
                courseId = (long) command.Parameters["@ID"].Value;
            }
            course.Id = courseId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAll), ex);
            return -1;
        }
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Course>> GetByAuthorId(long authorId)
    {
        const string query = "SELECT [ID], [Name], [AuthorId], [CreatedDateTime] FROM [dbo].[Courses] WHERE [AuthorID] = @AuthorID";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AuthorId", authorId);
            var reader = await command.ExecuteReaderAsync();
            var courseList = new List<Course>();
            while (await reader.ReadAsync())
            {
                var course = CreateCourseFromReader(reader);
                courseList.Add(course);
            }
            return courseList;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAll), ex);
            return null;
        }
    }

    public async Task<IEnumerable<Course>> GetAll()
    {
        const string query = "SELECT [ID], [Name], [AuthorID], [CreatedDateTime] FROM [dbo].[Courses]";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
            var reader = await command.ExecuteReaderAsync();
            var courseList = new List<Course>();
            while (await reader.ReadAsync())
            {
                var course = CreateCourseFromReader(reader);
                courseList.Add(course);
            }
            return courseList;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAll), ex);
            return null;
        }
    }

    public async Task<Course> GetByAuthorIdAndCourseId(long authorId, long courseId)
    {
        const string query = "SELECT [ID], [Name], [AuthorId], [CreatedDateTime] FROM [dbo].[Courses] WHERE [AuthorID] = @AuthorID AND [ID]=@CourseId";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AuthorId", authorId);
            command.Parameters.AddWithValue("@CourseId", courseId);
            var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }

            var course = CreateCourseFromReader(reader);
            return course;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAll), ex);
            return null;
        }
    }

    private static Course CreateCourseFromReader(SqlDataReader reader)
    {
        return new Course
        {
            Id = reader.GetFromColumn<long>("ID"),
            AuthorId = reader.GetFromColumn<long>("AuthorID"),
            Name = reader.GetFromColumn<string>("Name"),
            CreatedDateTime = reader.GetFromColumn<DateTime>("CreatedDateTime")
        };
    }
}