using System.Net;
using System.Text.Json;

namespace Cinema.Presentation.Middleware
{
    public class UnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                var response = new
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized access. Please log in."
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                var response = new
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Message = "Access denied. You do not have the required permissions."
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
