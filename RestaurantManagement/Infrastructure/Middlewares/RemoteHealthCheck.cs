using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RestaurantManagement.Api.Infrastructure.Middlewares
{
    public class RemoteHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory httpClientFactory;
        private RemoteHealthCheck(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using( var http = httpClientFactory.CreateClient() )
            {
                var response = await http.GetAsync("https://api.ipify.org");
                if(response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy($"Remote endpoints is healthy");
                }

                return HealthCheckResult.Unhealthy("unhealthy");
            }
        }
    }
}
