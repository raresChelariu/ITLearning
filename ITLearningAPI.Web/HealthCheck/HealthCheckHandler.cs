using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ITLearningAPI.Web.HealthCheck;

public class HealthCheckHandler : IHealthCheck
{
    public const string TAG = "MainHealthCheck";
    
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        return Task.FromResult(HealthCheckResult.Healthy(
            description: "Hello from health", 
            data: new Dictionary<string, object>
        {
            {
                "CurrentDirectory", 
                Directory.GetCurrentDirectory()
            }
        }));
    }
}