using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialMedia.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.UI
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = "/Login/Login";
                    options.LogoutPath = "/Login/Login";
                    options.AccessDeniedPath = "/Login/Login";
                    options.SlidingExpiration = true;
                });

            services.AddControllersWithViews();


            services.AddHttpClient("ClientSMApi", options =>
            {
                options.BaseAddress = new Uri(Configuration["ApiUri"]);

            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60); // Tiempo de expiración   
            });

            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IPostsService, PostsService>();

            var assamblies = AppDomain.CurrentDomain.GetAssemblies();

            services.AddMvcCore()
                .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(assamblies);


            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Tiempo inactivo
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });




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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(options =>
            options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           );

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}");
            });
        }
    }
}
