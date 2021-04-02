namespace Teams.Services.Core.Extensions.Common
{
    using System;
    using FluentValidation.Validators;

    public class GuidValidator : PropertyValidator
    {
        public GuidValidator()
            : base("{PropertyName} is not a valid GUID.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var propValue = (Guid?)context.PropertyValue;
            if (propValue == null || propValue.Equals(Guid.Empty))
            {
                return false;
            }

            return true;
        }
    }
}