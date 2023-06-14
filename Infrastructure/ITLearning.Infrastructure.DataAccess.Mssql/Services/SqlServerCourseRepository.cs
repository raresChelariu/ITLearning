using System.Data;
using Dapper;
using ITLearning.Domain;
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
                ParameterName = "@Description",
                Value = course.Description,
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
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(Insert), ex);
            return -1;
        }
    }

    public async Task<IEnumerable<Course>> GetByAuthorId(long authorId)
    {
        const string query = "SELECT [ID], [Name], [AuthorId], [CreatedDateTime], [Description] FROM [dbo].[Courses] WHERE [AuthorID] = @AuthorID";
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
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetByAuthorId), ex);
            return null;
        }
    }

    public async Task<IEnumerable<Course>> GetAll()
    {
        const string query = "SELECT [ID], [Name], [AuthorID], [CreatedDateTime], [Description] FROM [dbo].[Courses]";
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
        const string query = "SELECT [ID], [Name], [AuthorId], [CreatedDateTime], [Description] FROM [dbo].[Courses] WHERE [AuthorID] = @AuthorID AND [ID]=@CourseId";
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
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetByAuthorIdAndCourseId), ex);
            return null;
        }
    }

    public async Task<Course> GetById(long courseId)
    {
        const string query = "SELECT [ID], [Name], [AuthorId], [CreatedDateTime], [Description] FROM [dbo].[Courses] WHERE [ID] = @CourseId";
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
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetById), ex);
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
            parameters.Add("EndOfCourse", null, DbType.Boolean, ParameterDirection.Output);
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
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetNextItemId), ex);
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
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(UpdateUserCourseProgress), ex);
        }
    }

    public async Task<long> GetUsersLastItemId(long userId, long courseId)
    {
        const string query = "SELECT ItemID FROM UserCourseProgress WHERE UserID = @UserID AND CourseID = @CourseID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                UserID = userId,
                CourseID = courseId
            });
            
            var result = await connection.ExecuteScalarAsync<long>(query, parameters);
            if (result == default)
            {
                return -1;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetUsersLastItemId), ex);
            return -1;
        }
    }

    public async Task ResetUserProgress(long userId, long courseId)
    {
        const string query = "DELETE FROM UserCourseProgress where UserID = @UserID AND CourseID = @CourseID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                UserID = userId,
                CourseID = courseId
            });
            await connection.ExecuteAsync(query, parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(ResetUserProgress), ex);
        }
    }

    public async Task<IEnumerable<Course>> GetByStudentId(long studentId)
    {
        const string query = "CoursesGetByStudentId";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                UserID = studentId
            });
            var result = await connection.QueryAsync<Course>(query, parameters, null, null, CommandType.StoredProcedure);
            if (result is null)
            {
                _logger.LogWarning("No courses found for student with {@UserId}", studentId);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetByStudentId), ex);
            return null;
        }
    }

    public async Task<IEnumerable<Course>> GetSqlCoursesByUserId(long userId)
    {
        const string query = "CoursesGetByUserIdWithDatabaseDetails";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                UserID = userId
            });
            var result = await connection.QueryAsync<Course>(query, parameters, null, null, CommandType.StoredProcedure);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetSqlCoursesByUserId), ex);
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
            CreatedDateTime = reader.GetFromColumn<DateTime>("CreatedDateTime"),
            Description = reader.GetFromColumn<string>("Description")
        };
    }
}