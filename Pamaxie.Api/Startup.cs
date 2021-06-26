using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pamaxie.Api.Security;
using Pamaxie.Database.Extensions;

namespace Pamaxie.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            //Checking if the Redis and SQL Database is reachable and all dandy.
            if (!DbExtensions.SqlDbCheckup(out string sqlErrors))
            {
                Console.WriteLine(sqlErrors);
                Environment.Exit(501);
            }

            if (!DbExtensions.RedisDbCheckup(out string redisErrors))
            {
                Console.WriteLine(redisErrors);
                Environment.Exit(501);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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