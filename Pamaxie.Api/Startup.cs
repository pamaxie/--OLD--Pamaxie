using System;
using System.Text;
using AspNetCore.Proxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Authentication;

namespace Pamaxie.Api
{
    /// <summary>
    /// Startup class, usually gets called by <see cref="Program"/>
    /// </summary>
    public sealed class Startup
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
        /// Gets called by the runtime to add Services to the container
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            byte[] key = Encoding.ASCII.GetBytes("");
            services.AddAuthentication(x =>
                    {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.RefreshInterval = new TimeSpan(0, 10, 0);
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "Pamaxie",
                        ValidAudience = "Pamaxie"
                    };
                    });

            services.AddSwaggerGen(o => {
                o.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Pamaxie Database API",
                        Version = "v1"
                    });
            });

            services.AddTransient<JwtTokenGenerator>();
        }

        /// <summary>
        /// Gets called by the runtime to configure HTTP Services
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> Builder</param>
        /// <param name="env">Hosting Environment</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.RunProxy(proxy => proxy.UseHttp("https://database.pamaxie.com"));
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}