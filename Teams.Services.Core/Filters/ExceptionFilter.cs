namespace Teams.Services.Core.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using Teams.Services.Core.Common.Enums;

    public class ServiceExceptionInformation
    {
        public List<ServiceExceptionError> Errors { get; set; } = new List<ServiceExceptionError>();
    }

    public class ServiceException : Exception
    {
        public ServiceException()
        { }

        public ServiceException(string message)
            : base(message)
        { }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class ServiceExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ServiceExceptionInformation exception = new ServiceExceptionInformation();

            HttpStatusCode status = HttpStatusCode.InternalServerError;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(ServiceException))
            {
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                status = HttpStatusCode.NotFound;
            }
            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            exception.Errors.Add(new ServiceExceptionError
            {
                Type = ServiceErrorType.GeneralError,
                Description = $"{status} {context.Exception.Message}",
                Timestamp = DateTime.UtcNow,
            });

            //new TelemetryClient().TrackException(new Exception(context.Exception.Message));
            response.WriteAsync(JsonConvert.SerializeObject(exception));
        }
    }
}
