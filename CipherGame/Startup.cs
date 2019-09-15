using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CipherGame
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<CipherGameData.CipherGameContext, CipherGameData.CipherGameContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "auth_cookie";
                        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
                        
                        options.Cookie.HttpOnly = true;

                        options.Events.OnRedirectToAccessDenied = UnAuthorizedResponse;
                        options.Events.OnRedirectToLogin = UnAuthorizedResponse;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            using (var client = new CipherGameData.CipherGameContext())
            {
                client.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey("Cache-Control"))
                        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.

                    if (!context.Response.Headers.ContainsKey("Pragma"))
                        context.Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0.

                    if (!context.Response.Headers.ContainsKey("Expires"))
                        context.Response.Headers.Add("Expires", "0"); // Proxies.
                    return Task.FromResult(0);
                });
                await next();
            });

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest,
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None
            };

            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseCors(policy =>
            {
               policy.AllowAnyHeader();
               policy.AllowAnyMethod();
               policy.AllowAnyOrigin();
               policy.AllowCredentials();
            });

            app.UseAuthentication();

            app.UseMvc();
        }


        internal static Task UnAuthorizedResponse(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }
    }
}
