using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SecureAPI.HealthChecks;

public class RandomHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellation = default)
    {
        var responseTime = 100; // dummy for actual check

        if (responseTime < 100)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Is heatlthy"));
        }
        return Task.FromResult(HealthCheckResult.Degraded("Is not so heatlthy"));

        // etc
    }
}
