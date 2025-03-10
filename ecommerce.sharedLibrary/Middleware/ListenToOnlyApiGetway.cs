using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.sharedLibrary.Middleware
{
    public class ListenToOnlyApiGetway(RequestDelegate next)
    {

        public async Task InovakAsync(HttpContext context)
        {
            var signHeader = context.Request.Headers["Api-Gateway"];
            if (signHeader.FirstOrDefault() != null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry service is unavailable");
                return;
            } 
            else {
                await next(context);
            }
        }
    }
}
