using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SocialMedia.Application.CustomExceptions;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //almaceno la variable IHost devuelta por Build() antes de Run() para acceder a los servicios.
            try
            {
                //Este codigo crea un usuario por defecto con claim "Admin" en caso que no existan usuarios en la BD.
                using (var scope = host.Services.CreateScope())
                {   
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger(); // de donde saca la configuracion para los logs.


                    var context = scope.ServiceProvider.GetRequiredService<SocialMediaContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SocialMedia.Domain.Entities.SMUser>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    //Accedo a los servicios necesarios para crear el usuario por defecto.

                    bool t = context.Database.EnsureCreated();
                    //Me aseguro que la DB existe, de no ser asi se crea la DB.
                    if (!context.Users.Any()) // Si no existen usuarios en la DB
                    {
                        var adminUser = new SocialMedia.Domain.Entities.SMUser()
                        {
                            UserName = configuration.GetSection("DefaultUserData").GetSection("Username").Value,
                            Email = configuration.GetSection("DefaultUserData").GetSection("Email").Value,
                            EmailConfirmed = true
                        };
                        var password = configuration.GetSection("DefaultUserData").GetSection("Password").Value;

                        var result = userManager.CreateAsync(adminUser, password).GetAwaiter().GetResult();

                        roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

                        userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();


                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
                errors.Add("ApiError", new string[] { e.Message });
                throw new ApiExceptions(e.Message, errors);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog(); // Usar SeriLog como logs provider.
                    
                });
    }
}
