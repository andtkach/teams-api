namespace Teams.Services.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Teams.Services.API.Helpers;
    using Teams.Services.Core.Helpers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.ApplyCorsPolicy(services);
            this.ApplySingletons(services, this.Configuration);
            this.ApplyValidation(services);
            this.ApplyVersioning(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ConfigurationHelper.InitializeServices(app, env, provider);
        }

        private void ApplyCorsPolicy(IServiceCollection services)
        {
            AccessHelper.ApplyCorsPolicy(services);
        }

        private void ApplyVersioning(IServiceCollection services)
        {
            VersioningHelper.ApplyVersioning(services);
        }

        private void ApplySingletons(IServiceCollection services, IConfiguration configuration)
        {
            SingletonHelper.ApplySingletons(services, configuration);
        }

        private void ApplyValidation(IServiceCollection services)
        {
            ValidationHelper.ApplyValidators(services);
        }
    }
}
