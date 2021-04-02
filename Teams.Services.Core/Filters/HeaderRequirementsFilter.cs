namespace Teams.Services.Core.Filters
{
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Teams.Services.Core.Common.Constants;

    public class HeaderRequirementsFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument document, DocumentFilterContext context)
        {
            document.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    {
                        DocumentationConstants.HeaderAuthorizationType.SubscriptionKey.ToString(), new string[] { }
                    }
                }
            };
        }
    }
}
