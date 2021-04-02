namespace Teams.Services.Core.Models.Errors
{
    using System;
    using Teams.Services.Core.Common.Enums;

    public class ServiceErrorInformation
    {
        public ServiceErrorType Type { get; set; }
        
        public string Description { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}
