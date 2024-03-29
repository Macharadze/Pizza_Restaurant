using RestaurantManagement.Infrastructure.Middlewares;

namespace RestaurantManagement.Infrastructure.Extensions
{
    public static class RequestResponseExtension
    {
        public static IApplicationBuilder UseRequestResponseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseMiddleware>();
            return app;
        }
    }
}
