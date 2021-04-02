namespace Teams.Services.API.Extensions
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Teams.Services.API.Authorization;
    using Teams.Services.Core.Common.Constants;

    public static class HeaderExtensions
    {
        public static SubscriptionAuthorizationInformation Validate(this IHeaderDictionary headers)
        {
            return new SubscriptionAuthorizationInformation
            {
                SubscriptionKey = headers.ExtractSubscriptionKey(),
            };
        }

        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        private static Guid ExtractSubscriptionKey(this IHeaderDictionary headers)
        {
            Guid subscriptionKey = headers.ContainsKey("SubscriptionKey") 
                ? new Guid(headers["SubscriptionKey"].First()) 
                : Guid.Empty;

#if DEBUG
            return (subscriptionKey == Guid.Empty) ? SystemConstants.SystemSubscriptionKey : subscriptionKey;
#else
            return subscriptionKey;
#endif
        }
    }
}
