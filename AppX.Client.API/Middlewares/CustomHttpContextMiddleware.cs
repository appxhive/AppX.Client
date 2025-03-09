using System.Diagnostics;

namespace AppX.Client.API.Middlewares
{
    public class CustomHttpContextMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string QueryString = context.Request.QueryString.Value;

            await next.Invoke(context);
        }
    }
}
