using Serilog;

namespace RestaurantManagement.Infrastructure.Middlewares
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestResponseMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequestAndResponse(context.Request, context.Response);
            await _next(context);
        }

        private async Task LogRequestAndResponse(HttpRequest request, HttpResponse response)
        {
            var toLog = $"{Environment.NewLine} Request Info {Environment.NewLine}" +
            $"IP = {request.HttpContext.Connection.RemoteIpAddress}{Environment.NewLine}" +
            $"Address = {request.Scheme}{Environment.NewLine}" +
            $"IsSescured = {request.IsHttps}{Environment.NewLine}" +
            $"Body = {request.Body}{Environment.NewLine}" +
            $"QueryString = {request.QueryString}{Environment.NewLine}" +
            $"Time = {DateTime.Now}{Environment.NewLine}" +
            $"{Environment.NewLine} Response Info {Environment.NewLine}" +
            $"StatusCode = {response.StatusCode}{Environment.NewLine}"+
            $"Body = {response.Body}{Environment.NewLine}";



            Log.Information(toLog);
            await Task.CompletedTask;
        }
    }
}
