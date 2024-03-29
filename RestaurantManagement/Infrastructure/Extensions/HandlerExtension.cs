using RestaurantManagement.Infrastructure.Middlewares;

namespace RestaurantManagement.Infrastructure.Extensions
{
    public static class HandlerExtension
    {
        public static IApplicationBuilder UseHandlerMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();
            return builder;
        }
    }
}
