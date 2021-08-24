using System;
using System.Collections.Generic;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Pamaxie.Database.Extensions.Sql;
using Pamaxie.Website.Services;

namespace Pamaxie.Website 
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

        
        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// This method gets called by the runtime to add services to the container for dependency injection
        /// </summary>
        /// <param name="services">Service Collection to add services to</param>
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection dbConfigSection = Configuration.GetSection("Pamaxie");
            string sqlConString =  dbConfigSection.GetValue<string>("PamaxieSqlDb");
            Environment.SetEnvironmentVariable("PamaxieSqlDb", sqlConString);
            Environment.SetEnvironmentVariable("ApplyMigrations", dbConfigSection.GetValue<string>("Apply Migrations"));

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<BrowserService>();
            services.AddScoped<UserService>();
            services.AddScoped<EmailSender>();

            // ReSharper disable once UnusedVariable
            CookiePolicyOptions cookiePolicy = new()
            {
                Secure = CookieSecurePolicy.Always,
            };

            //Add rate limit
            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();
            services.Configure<ClientRateLimitOptions>(option =>
            {
                option.EnableEndpointRateLimiting = false;
                option.StackBlockedRequests = true;
                option.HttpStatusCode = 429;
                option.ClientWhitelist = new List<string> {"", ""};
                option.GeneralRules = new List<RateLimitRule>
                {
                    new()
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 2
                    },
                    new()
                    {
                        Endpoint = "*",
                        Period = "15m",
                        Limit = 100
                    },
                    new()
                    {
                        Endpoint = "*",
                        Period = "12h",
                        Limit = 1000
                    },
                    new()
                    {
                        Endpoint = "*",
                        Period = "7d",
                        Limit = 10000
                    }
                };
            });
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            //Add the Mudblazor Theme
            services.AddMudServices();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = false;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration["Google:ClientId"];
                options.ClientSecret = Configuration["Google:ClientSecret"];
                options.ClaimActions.MapJsonKey("urn:google:profile", "link");
                options.ClaimActions.MapJsonKey("urn:google:image", "picture");
            });

            services.AddApplicationInsightsTelemetry();
            
            //Adds access to the HTTP Context
            services.AddHttpContextAccessor();
            
            if (!DbExtensions.SqlDbCheckup(out string error))
            {
                Console.WriteLine(error);
                Environment.Exit(501);
            }
        }


        /// <summary>
        /// This is called by the runtime to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web-host Environment</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseIpRateLimiting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
