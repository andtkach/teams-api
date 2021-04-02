namespace Teams.Services.Core.Filters
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Teams.Services.Core.Common.Enums;
    using Teams.Services.Core.Extensions.Common;

    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ModelValidationInformation error = new ModelValidationInformation()
                {
                    Type = ServiceErrorType.ModelValidationError,
                    Description = ServiceErrorType.ModelValidationError.Description(),
                    Timestamp = DateTime.UtcNow,
                };

                foreach (var item in context.ModelState.Values)
                {
                    foreach (var message in item.Errors)
                    {
                        error.ValidationErrors.Add(new ModelValidationError
                        {
                            Description = (!string.IsNullOrEmpty(message.ErrorMessage)) ? message.ErrorMessage : $"{message.Exception.Message}"
                        });
                    }
                }

                context.Result = new BadRequestObjectResult(error);
            }
        }
    }
}