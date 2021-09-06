using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Pamaxie.Api
{
    /// <summary>
    /// Startup class, usually gets called by <see cref="Program"/>
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="configuration">Configuration to use</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime to add services to the container for dependency injection
        /// </summary>
        /// <param name="services">Service Collection to add services to</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Load Data Storage Configuration from appsettings.json
            
            IConfigurationSection dbConfigSection = Configuration.GetSection("DbConfig");
            Environment.SetEnvironmentVariable("PamaxieSqlDb", dbConfigSection.GetValue<string>("PamaxieSqlDb"));
            Environment.SetEnvironmentVariable("PamaxieRedisAddr", dbConfigSection.GetValue<string>("PamaxieRedisAddr"));
            Environment.SetEnvironmentVariable("PamaxiePublicRedisAddr", dbConfigSection.GetValue<string>("PamaxiePublicRedisAddr"));
            Environment.SetEnvironmentVariable("ApplyMigrations", dbConfigSection.GetValue<string>("Apply Migrations"));

            services.AddControllers();
            IConfigurationSection section = Configuration.GetSection("AuthData");
            byte[] key = Encoding.ASCII.GetBytes(section.GetValue<string>("Secret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddTransient<TokenGenerator>();

            //if (TODO Validate connection to Redis)
            //{
            //    Console.WriteLine(redisErrors);
            //    Environment.Exit(501);
            //}
        }

        /// <summary>
        /// This is called by the runtime to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web-host Environment</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.UseClientRateLimiting();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}