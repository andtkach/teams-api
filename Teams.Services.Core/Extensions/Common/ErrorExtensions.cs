namespace Teams.Services.Core.Extensions.Common
{
    using System;
    using Teams.Services.Core.Common.Enums;
    using Teams.Services.Core.Models.Errors;

    public static class ErrorExtensions
    {
        public static ServiceErrorInformation AsErrorMessage(this ServiceErrorType type)
        {
            return new ServiceErrorInformation()
            {
                Description = type.Description(),
                Type = type,
                Timestamp = DateTime.UtcNow,
            };
        }

        public static ServiceErrorInformation AsServiceError(this string value)
        {
            return new ServiceErrorInformation()
            {
                Description = value,
                Timestamp = DateTime.UtcNow,
                Type = ServiceErrorType.GeneralError
            };
        }
    }
}
