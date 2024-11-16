using AppX.Client.Domain.Exceptions;

namespace AppX.Client.API.Middlewares
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundEx)
            {
                logger.LogWarning(notFoundEx.Message);

                context.Response.StatusCode = 404;

                await context.Response.WriteAsync(notFoundEx.Message);
            }
            catch (ForbiddenException forbiddenEx)
            {
                logger.LogWarning("Access forbidden");

                context.Response.StatusCode = 403;

                await context.Response.WriteAsync("Access forbidden");
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;

                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
