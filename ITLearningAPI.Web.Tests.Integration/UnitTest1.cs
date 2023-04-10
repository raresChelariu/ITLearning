using Microsoft.Data.SqlClient;
using Xunit.Abstractions;

namespace ITLearningAPI.Infrastructure.Common.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test1()
    {
        const string connectionString = "Server=localhost;Database=ITLearning;Integrated Security=true;TrustServerCertificate=True";
        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync();
        _testOutputHelper.WriteLine("hey");
    }
}