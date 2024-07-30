using RestItla.Application.Extras.ResultObject;
using RestItla.Domain.Enum;

namespace RestItla.WebApi.Middleware
{
    internal class ApiErrorExceptionMiddleware
        : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(
                    ErrorType.ConnectionError.Because(e.Message)
                );
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(
                    ErrorType.Unknown.Because(e.Message)
                );

                throw;
            }
        }
    }
}