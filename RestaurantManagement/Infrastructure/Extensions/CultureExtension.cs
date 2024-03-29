using RestaurantManagement.Infrastructure.Middlewares;

namespace RestaurantManagement.Infrastructure.Extensions
{
    public static class CultureExtension
    {
        public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CultureMiddleware>();
            return app;
        }
    }
}
