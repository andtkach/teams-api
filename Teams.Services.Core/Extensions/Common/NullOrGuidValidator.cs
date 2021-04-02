namespace Teams.Services.Core.Extensions.Common
{
    using System;
    using FluentValidation.Validators;

    public class NullOrGuidValidator : PropertyValidator
    {
        public NullOrGuidValidator()
            : base("{PropertyName} is not NULL or a valid GUID.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var propValue = (Guid?)context.PropertyValue;
            if (propValue != null && propValue.Equals(Guid.Empty))
            {
                return false;
            }

            return true;
        }
    }
}