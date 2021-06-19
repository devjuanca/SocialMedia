using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Application.Extentions;
using SocialMedia.Application.Filters.ExceptionFilters;
using SocialMedia.Application.Services;
using SocialMedia.Persistence;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SocialMedia.Api
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
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExeptionFilter>();
            });
            //.ConfigureApiBehaviorOptions(
            //options => { options.SuppressModelStateInvalidFilter = true; } 
            // inabilitar la validacion del modelo de [ApiController])
            //Este metodo esta deprecated, mejor usar el manejo de errores por defecto de [ApiController]
            //y estandarizar el formato que la api regresa errores de la misma forma que el por defecto.

            

            services.AddDbContext<SocialMediaContext>(options =>
            {

                options.UseSqlServer(Configuration.GetConnectionString("SocialMedia"));
            });
            //, op => op.EnableRetryOnFailure()  ---> Esta opcion no permite utilizar Transactions

            services

               .AddIdentityCore<SocialMedia.Domain.Entities.SMUser>(opt =>
               {
                   opt.Password.RequireDigit = true;
                   opt.Password.RequiredLength = 6;
                   opt.Lockout.MaxFailedAccessAttempts = 5;
                   opt.User.RequireUniqueEmail = true;
                   opt.SignIn.RequireConfirmedEmail = true;
               })
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<SocialMedia.Domain.Entities.SMUser>>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddUserManager<UserManager<SocialMedia.Domain.Entities.SMUser>>()
                .AddEntityFrameworkStores<SocialMediaContext>();


            services.AddMyServices();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            //services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Social Media API",
                    Version = "v1"
                });

                //Habilitar Autorizacion por Token Jwt en Swagger.
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
             {
                   new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                         }
                     },
                     new string[] {}

             }
         });

                var xmlFile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            //Este servicio es para generar el json de documentacion generado por swagger.
            //SawggerBuckle AspNetCore nuget packet.

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],
                        ValidAudience = Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                    };

                });
            services.AddOptions(Configuration);
            //services.Configure<PagingConfiguration>(Configuration.GetSection("Pagination"));
            //De esta forma se configura un servicio que utiliza secciones de configuracion del appsettings.
            //PagingConfiguration es una clase con los parametros idem a la seccion de configuracion Paginations.
            //Haciendo esto mapea los valores de la seccion de configuracion al objeto inyectado donde se necesite.
            //En este caso se uso en el Repositorio Post_Gets para obtener opciones por defecto de paginacion.

            services.AddMvcCore().AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
            //Registrar el servicio de FluentValidators, Se registran los Assamblies del dominio para 
            //no hacerlo de a uno y pq los validators estan en un proyecto distinto a la API.

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("http://example.com",
            //                                "http://www.contoso.com");
            //        });
            //});
            /*La seguridad del explorador evita que una página web realice solicitudes a un dominio diferente 
             * del que atendió a dicha página web. Esta restricción se denomina directiva de mismo origen. 
             * La directiva de mismo origen evita que un sitio malintencionado lea información confidencial de otro sitio. 
             * A veces, es posible que quiera permitir que otros sitios realicen solicitudes entre orígenes a la aplicación.*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API V1");
                //options.RoutePrefix = string.Empty;
                //ESTO SE ELIMINA CUANDO SUBO LA API A IIS Y LO HAGO EN UN SUBDOMINIO.
            });
            app.UseRouting();

            app.UseCors(options =>
             options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(env.ContentRootPath, "MyStaticFiles")),
                RequestPath = "/MyStaticFiles"
            });
            //crear acceso a carpeta de archivos estaticos solo con acceso autorizado
            //RequestPath es la ruta mapeada relativa para acceder a la carpeta física

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
