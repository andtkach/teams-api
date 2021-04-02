namespace Teams.Services.Core.Common.Enums
{
    using System.ComponentModel;

    public enum ServiceErrorType
    {
        [Description("A valid subscription key is required.")]
        SubscriptionKeyRequired = 100,
        [Description("A general error has occurred.")]
        GeneralError = 500,
        [Description("A model validation error has occurred.")]
        ModelValidationError = 501,
    }
}
