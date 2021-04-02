namespace Teams.Services.API.Helpers
{
    using System.IO;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Swashbuckle.AspNetCore.Swagger;
    using Teams.Services.Core.Common.Constants;
    using Teams.Services.Core.Filters;

    public static class VersioningHelper
    {
        public static void ApplyVersioning(IServiceCollection services)
        {
            services
                .AddMvc(config =>
                {
                    config.Filters.Add(typeof(ValidateModelStateAttribute));
                    config.Filters.Add(typeof(ServiceExceptionFilter));
                })
                .AddFluentValidation();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvcCore()
                    .AddJsonFormatters()
                    .AddVersionedApiExplorer(
                          options =>
                          {
                              options.GroupNameFormat = "'v'VVV";
                              options.SubstituteApiVersionInUrl = true;
                              options.DefaultApiVersion = new ApiVersion(1, 0);
                          });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;

                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionSelector = new LowestImplementedApiVersionSelector(o);
            });

            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(
                            description.GroupName, 
                            new Info()
                            {
                                Title = $"{LicenseConstants.ApiCollectionLabel} {description.ApiVersion}",
                                Version = description.ApiVersion.ToString(),
                                Description = description.IsDeprecated ? $"{LicenseConstants.ApiDescriptionLabel} (Deprecated)" : $"{LicenseConstants.ApiDescriptionLabel}",
                                TermsOfService = LicenseConstants.ApiTermsOfServiceUrl,
                                Contact = new Contact
                                {
                                    Name = LicenseConstants.ApiContactLabel,
                                    Email = LicenseConstants.ApiContactUrl
                                },
                                License = new License
                                {
                                    Name = LicenseConstants.ApiLicenseLabel,
                                    Url = LicenseConstants.ApiLicenseUrl
                                },
                            });
                    }

                    options.AddSecurityDefinition(
                        DocumentationConstants.HeaderAuthorizationType.SubscriptionKey.ToString(), 
                        new ApiKeyScheme
                        {
                            Description = "Subscription key",
                            Name = DocumentationConstants.SubscriptionKeySecurityValueLabel,
                            In = DocumentationConstants.SecurityValueLocationTypeLabel
                        });

                    options.DocumentFilter<HeaderRequirementsFilter>();
                    options.DocumentFilter<DocumentEnumFilter>();

                    options.OperationFilter<SwaggerDefaultValues>();
                    options.OrderActionsBy(o => o.GroupName);
                    options.IncludeXmlComments(GenerateCommentFilePath());
                });
        }

        private static string GenerateCommentFilePath()
        {
            return Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{LicenseConstants.ApiDocumentationLabel}.xml");
        }
    }
}
