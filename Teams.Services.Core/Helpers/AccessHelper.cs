namespace Teams.Services.Core.Helpers
{
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public static class AccessHelper
    {
        public static void ApplyCorsPolicy(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                    .WithOrigins(GetAllowedOrigins())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

                options.AddPolicy(
                    "AnyOriginPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        private static string[] GetAllowedOrigins()
        {
            return (new List<string>()
            {
                "http://localhost:4200",
                "http://localhost:5000",
                "http://localhost:5001",
                "https://devatk11.com",
            }).ToArray();
        }
    }
}
