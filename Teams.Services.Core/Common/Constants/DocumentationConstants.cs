namespace Teams.Services.Core.Common.Constants
{
    public static class DocumentationConstants
    {
        public enum HeaderAuthorizationType
        {
            AccountId,

            UserId,

            SubscriptionKey
        }
        
        public static string AccountIdSecurityValueLabel => "AccountId";
        
        public static string UserIdSecurityValueLabel => "UserId";
        
        public static string SubscriptionKeySecurityValueLabel => "SubscriptionKey";
        
        public static string SecurityValueLocationTypeLabel => "header";
    }
}
