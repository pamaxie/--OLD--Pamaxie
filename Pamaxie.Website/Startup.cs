using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Pamaxie.Database.Extensions;
using Pamaxie.Website.Services;

namespace Pamaxie.Website

{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection dbConfigSection = Configuration.GetSection("Pamaxie");
            var sqlConString =  dbConfigSection.GetValue<string>("PamaxieSqlDb");
            Environment.SetEnvironmentVariable("PamaxieSqlDb", sqlConString);
            Environment.SetEnvironmentVariable("ApplyMigrations", dbConfigSection.GetValue<string>("Apply Migrations"));

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<BrowserService>();
            //Adds access to the HTTP Context
            services.AddHttpContextAccessor();

            CookiePolicyOptions cookiePolicy = new CookiePolicyOptions()
            {
                Secure = CookieSecurePolicy.Always,
            };

            //Add the Mudblazor Theme
            services.AddMudServices();
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
            
            if (!DbExtensions.SqlDbCheckup(out var error))
            {
                Console.WriteLine(error);
                Environment.Exit(501);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
