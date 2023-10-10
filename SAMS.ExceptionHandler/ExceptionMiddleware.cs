using SAMS.Data;
using SAMS.DataAccess;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.WebToken;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SAMS.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, unitOfWork, jwtHelper);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            var errorCode = Guid.NewGuid().ToString();
            var httpStatusCode = (int)HttpStatusCode.InternalServerError;
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                httpStatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                httpStatusCode = (int)HttpStatusCode.Forbidden;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpStatusCode;
            context.Response.Headers.Add("Application-Error-Code", errorCode);
            context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

            ErrorLog log = new ErrorLog();
            log.MachineName = context.Request.Host.Value;
            log.UserIp = context.Connection.RemoteIpAddress.ToString();
            log.LogLevel = LogLevel.Critical;
            log.Message = string.Format("{0} -*- {1}", exception.Source, exception.Message).Substring(0, 4000);
            log.StackTrace = exception.StackTrace.Substring(0, 4000);
            log.ErrorCode = errorCode;
            log.Url = context.Request.Path;

            var logRepo = unitOfWork.GetRepository<ErrorLog>();
            await logRepo.Add(log);
            await unitOfWork.SaveChangesAsync();

            await context.Response.WriteAsync(exception.Message);
            return;
        }
    }

}