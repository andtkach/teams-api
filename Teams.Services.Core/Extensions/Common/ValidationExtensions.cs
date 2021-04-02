namespace Teams.Services.Core.Extensions.Common
{
    using FluentValidation;

    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> NullOrPopulatedCollection<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PopulatedCollectionValidator());
        }

        public static IRuleBuilderOptions<T, TProperty> NullOrPopulatedGuid<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NullOrGuidValidator());
        }

        public static IRuleBuilderOptions<T, TProperty> PopulatedGuid<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new GuidValidator());
        }
    }
}
