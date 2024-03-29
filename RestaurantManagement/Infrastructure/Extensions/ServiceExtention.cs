using RestaurantManagement.Application.Interfaces.IAddress;
using RestaurantManagement.Application.Interfaces.IImage;
using RestaurantManagement.Application.Interfaces.IOrder;
using RestaurantManagement.Application.Interfaces.IPizza;
using RestaurantManagement.Application.Interfaces.IUser;
using RestaurantManagement.Infrastrucutre.Repositories.AddressRepository;
using RestaurantManagement.Infrastrucutre.Repositories;
using RestaurantManagement.Infrastrucutre.Repositories.OrderRepository;
using RestaurantManagement.Infrastrucutre.Repositories.PizzaReposity;
using RestaurantManagement.Infrastrucutre.Repositories.UserRepository;
using RestaurantManagement.Infrastrucutre.Repositories.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestaurantManagement.Api.Infrastructure.Middlewares;

namespace RestaurantManagement.Api.Infrastructure.Extensions
{
    public static class ServiceExtention
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IImageRepository,ImageRepository>();

        }
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:DefaultConnection"], healthQuery: "select 1", name: "SQL Server", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Database" })
               .AddCheck<RemoteHealthCheck>("Remote endpoints Health Check",failureStatus: HealthStatus.Unhealthy)
                .AddUrlGroup(new Uri("https://localhost:7243/api/Address"), name: "base URL", failureStatus: HealthStatus.Unhealthy);

            //services.AddHealthChecksUI();
            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("feedback api", "/api/health"); //map health check api    

            })
                .AddInMemoryStorage();
        }

    }
}
