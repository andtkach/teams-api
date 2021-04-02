namespace Teams.Services.Core.Filters
{
    using System.Collections.Generic;
    using Teams.Services.Core.Models.Errors;

    public class ModelValidationInformation : ServiceErrorInformation
    {
        public List<ModelValidationError> ValidationErrors { get; set; } = new List<ModelValidationError>();
    }
}
