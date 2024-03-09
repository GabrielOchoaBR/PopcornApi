using System.Net.Mime;
using System.Text.Json;
using PopcornApi.Model.WebApi;

namespace PopcornApi.Middlewares
{
    public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, ex.StackTrace);

                await ExceptionResponse(context, ex);
            }
        }

        private async Task ExceptionResponse(HttpContext context, Exception ex)
        {
            var statusCode = GetStatusCode(ex);

            var response = new ExceptionResponse(GetTitle(ex),
                                                 ex.Message,
                                                 statusCode,
                                                 GetErrors(ex));

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static IDictionary<string, string[]>? GetErrors(Exception ex) => ex switch
        {
            Application.Exceptions.ValidationException validationException => validationException.ErrorsDictionary,
            _ => null
        };

        private static string GetTitle(Exception ex) => ex switch
        {
            Application.Exceptions.ApplicationException appException => appException.Title,
            _ => "Server Error [Unknown]"
        };

        private static int GetStatusCode(Exception ex) => ex switch
        {
            Application.Exceptions.ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
