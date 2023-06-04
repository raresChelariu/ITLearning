using System.Data;
using Dapper;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerCourseRepository : ICourseRepository
{
    private readonly ILogger<SqlServerUserRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerCourseRepository(ILogger<SqlServerUserRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
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
                ParameterName = "@AuthorID",
                Value = course.AuthorId
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Name",
                Value = course.Name,
                Size = 128,
                SqlDbType = SqlDbType.VarChar
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
            return courseId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAll), ex);
            return -1;
        }
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

    public async Task<Course> GetById(long courseId)
    {
        const string query = "SELECT [ID], [Name], [AuthorId], [CreatedDateTime] FROM [dbo].[Courses] WHERE [ID] = @CourseId";
        try
        {
            await using var connection = _databaseConnector.GetSqlConnection();
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
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

    public async Task<NextItemIdResult> GetNextItemId(long courseId, long itemId)
    {
        const string query = "CourseGetNextItem";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                CourseID = courseId,
                ItemID = itemId
            });
            parameters.Add("EndOfCourse", null, DbType.Binary, ParameterDirection.Output);
            parameters.Add("NextID", null, DbType.Int64, ParameterDirection.Output);
            await connection.ExecuteAsync(query, parameters, null, null, CommandType.StoredProcedure);
            var isEndOfCourse = parameters.Get<bool>("EndOfCourse");
            var nextId = parameters.Get<long>("NextID");
            return new NextItemIdResult
            {
                IsEndOfCourse = isEndOfCourse,
                NextItemId = nextId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetAll), ex);
            return null;
        }
    }

    public async Task UpdateUserCourseProgress(long userId, long courseId, long itemId)
    {
        const string query = "UserCourseProgressUpdate";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                UserID = userId,
                CourseID = courseId,
                ItemID = itemId
            });
            await connection.ExecuteAsync(query, parameters, null, null, CommandType.StoredProcedure);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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