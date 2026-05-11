using CvTracker.Web.Data;
using CvTracker.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;

namespace CvTracker.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                    options.Cookie.Name = "AdasConnect.Auth";
                    options.SlidingExpiration = true;
                });

            var mvc = services.AddControllersWithViews();
            // Always compile Razor from source .cshtml on each request (avoids stale embedded views).
            // For production at scale, switch to compile-on-build and remove this line.
            mvc.AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogWarning(
                "ADAS Connect host: Environment={Env}, ContentRoot={Root}",
                env.EnvironmentName,
                env.ContentRootPath);

            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Adas-Ui-Shell"] = "v2-2026";
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            if (env.IsDevelopment())
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                    }
                });
            }
            else
            {
                app.UseStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            try
            {
                DbSeeder.Seed(app.ApplicationServices);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database seed failed. Check SQL Server is running and the connection string in appsettings.json.");
            }

            app.UseEndpoints(endpoints =>
            {
                // Plain-text probe: open https://localhost:XXXX/__adas/ping — proves THIS build & ContentRoot.
                endpoints.MapGet("/__adas/ping", async context =>
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync(
                        "adas-ui-v2-ping\n" +
                        "ContentRoot=" + env.ContentRootPath + "\n" +
                        "Environment=" + env.EnvironmentName + "\n");
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
