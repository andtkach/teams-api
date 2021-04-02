namespace Teams.Services.Core.Extensions.Common
{
    using System.Collections.Generic;
    using FluentValidation.Validators;

    public class PopulatedCollectionValidator : PropertyValidator
    {
        public PopulatedCollectionValidator()
            : base("{PropertyName} must be NULL or contain between 1 and 10 elements.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var propValue = (List<object>)context.PropertyValue;
            if (propValue == null)
            {
                return true;
            }
            else if (propValue.Count == 0 || propValue.Count > 10)
            {
                return false;
            }

            return true;
        }
    }
}