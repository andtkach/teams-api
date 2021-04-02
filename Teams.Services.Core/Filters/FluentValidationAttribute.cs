namespace Teams.Services.Core.Filters
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FluentValidationLengthAttribute : ValidationAttribute
    {
        public FluentValidationLengthAttribute(Type type)
        {
            this.MinimumLength = 20;
        }

        public int MinimumLength { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}
