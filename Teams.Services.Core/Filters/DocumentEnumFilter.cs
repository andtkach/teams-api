namespace Teams.Services.Core.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DocumentEnumFilter : IDocumentFilter
    {
        /// <inheritdoc />
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var schemaDictionaryItem in swaggerDoc.Definitions)
            {
                var schema = schemaDictionaryItem.Value;
                foreach (var propertyDictionaryItem in schema.Properties)
                {
                    var property = propertyDictionaryItem.Value;
                    var propertyEnums = property.Enum;
                    if (propertyEnums != null && propertyEnums.Count > 0)
                    {
                        property.Description += DescribeEnum(propertyEnums);
                    }
                }
            }

            if (swaggerDoc.Paths.Count <= 0)
            {
                return;
            }

            var apiVersions = context.ApiDescriptions.Select(s => s.GroupName);
            for (int i = swaggerDoc.Paths.Keys.Count - 1; i >= 0; i--)
            {
                if (!apiVersions.Contains(swaggerDoc.Paths.Keys.ToList()[i].Split('/')[2]))
                {
                    swaggerDoc.Paths.Remove(swaggerDoc.Paths.Keys.ToList()[i]);
                }
            }

            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Parameters);
                var possibleParameterisedOperations = new List<Operation> { pathItem.Get, pathItem.Post, pathItem.Put };
                possibleParameterisedOperations.FindAll(x => x != null)
                    .ForEach(x => DescribeEnumParameters(x.Parameters));
            }
        }

        private static void DescribeEnumParameters(IList<IParameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                if (param.Extensions.ContainsKey("enum") && param.Extensions["enum"] is IList<object> paramEnums &&
                    paramEnums.Count > 0)
                {
                    param.Description += DescribeEnum(paramEnums);
                }
            }
        }

        private static string DescribeEnum(IEnumerable<object> enums)
        {
            var enumDescriptions = new List<string>();
            Type type = null;
            foreach (var enumOption in enums)
            {
                if (type == null)
                {
                    type = enumOption.GetType();
                }

                enumDescriptions.Add($"{Convert.ChangeType(enumOption, type.GetEnumUnderlyingType())} = {Enum.GetName(type, enumOption)}");
            }

            return $"{Environment.NewLine}{string.Join(Environment.NewLine, enumDescriptions)}";
        }
    }
}
