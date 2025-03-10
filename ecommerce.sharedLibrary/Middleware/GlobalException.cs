using ecommerce.sharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;


namespace ecommerce.sharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Sorry Internal server error occured.";
            int statuscode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";


            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many Request";
                    statuscode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, statuscode, message);

                }
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorised";
                    statuscode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, statuscode, message);

                }

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of service";
                    message = "Yor are not allowed";
                    statuscode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, statuscode, message);

                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out Of Time"; message = "RTequest Timeout "; statuscode = StatusCodes.Status408RequestTimeout;
                }
                await ModifyHeader(context, title, statuscode, message);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, int statuscode, string message)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statuscode,
                Title = title
            }), CancellationToken.None);
            return;
        }
    }
}

