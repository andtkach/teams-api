namespace Teams.Services.API.Helpers
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Teams.Services.Core.Common.Constants;

    internal static class ConfigurationHelper
    {
        internal static void InitializeServices(
            IApplicationBuilder app,
            IHostingEnvironment env, 
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{LicenseConstants.ApiDocumentationLabel.Replace(".", " ")} {description.GroupName.ToLower()}");
                    }
                });

            app.UseMvc();
        }
    }
}
