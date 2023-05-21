using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ITLearning.Utils.Contracts;
using ITLearningAPI.Web.Authorization;

namespace ITLearningAPI.Web.Tests;

public class JwtServiceTests
{
    private const string SomeSecret = "someSecretHere12345";
    private readonly DateTime _someDateTime = new (2022, 03, 04);
    private const string SomeName = "MichaelJackson";

    [Fact]
    public void CreateJwtToken_ShouldCreateToken_WithGivenClaims()
    {
        var authSettings = GetAuthSettings(SomeSecret, 10);
        var timeProvider = GetDateTimeProvider(_someDateTime);
        var sut = new JwtService(authSettings, timeProvider);

        var expectedTicks = _someDateTime.AddMinutes(authSettings.TokenTimeoutMinutes).Ticks;

        var input = new List<Claim>
        {
            new(ClaimTypes.Name, SomeName)
        };
        var result = sut.CreateJwtToken(input);
        result.Should().NotBeNull();

        var token = new JwtSecurityToken(result);
        var tokenClaims = token.Claims.ToDictionary(x => x.Type, x => x);
        tokenClaims[ClaimTypes.Name].Value.Should().Be(SomeName);
        tokenClaims[ClaimTypes.Expiration].Value.Should().Be(expectedTicks.ToString());
    }

    private static IDateTimeProvider GetDateTimeProvider(DateTime dateTime)
    {
        var timeProvider = new Mock<IDateTimeProvider>();
        timeProvider.Setup(x => x.UtcNow).Returns(dateTime);
        return timeProvider.Object;
    }

    private static IAuthorizationSettings GetAuthSettings(string secret, int tokenTimeoutMinutes)
    {
        var mockAuthSettings = new Mock<IAuthorizationSettings>();
        mockAuthSettings.SetupGet(x => x.Secret).Returns(secret);
        mockAuthSettings.SetupGet(x => x.TokenTimeoutMinutes).Returns(tokenTimeoutMinutes);
        return mockAuthSettings.Object;
    }
}