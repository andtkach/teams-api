namespace Teams.Services.Core.Extensions.Common
{
    using System;
    using System.Linq;

    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            dynamic displayAttribute = null;
            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            return displayAttribute?.Description ?? "Description Not Found";
        }
    }
}
