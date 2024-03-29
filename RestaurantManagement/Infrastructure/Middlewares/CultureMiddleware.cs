using System.Globalization;

namespace RestaurantManagement.Infrastructure.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext context)
        {
            
            
            string cultureName = context.Request.Headers["Accept-Language"].ToString();
            CultureInfo culture = new CultureInfo("ka-GE");
            if (!string.IsNullOrEmpty(cultureName))
            {
                try
                {
                    culture = new CultureInfo(cultureName);
                }catch(Exception ex)
                {

                }
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }

    }
}
