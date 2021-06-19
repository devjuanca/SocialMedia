using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Application.Bussiness_Rules;
using SocialMedia.Application.DTOs;
using SocialMedia.Application.Entities;
using SocialMedia.Application.Repository.AdminUser;
using SocialMedia.Application.Repository.Authentication;
using SocialMedia.Application.Repository.Comment.Gets;
using SocialMedia.Application.Repository.Comment.Posts;
using SocialMedia.Application.Repository.Country;
using SocialMedia.Application.Repository.Post.Gets;
using SocialMedia.Application.Repository.Post.Posts;
using SocialMedia.Application.Repository.SMUser.Gets;
using SocialMedia.Application.Repository.SMUser.Posts;
using SocialMedia.Application.Services;
using SocialMedia.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Application.Extentions
{
    public static class ServiceCollectionExtention
    {
   

        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddTransient<IPost_Gets, Post_Gets>();
            services.AddTransient<IPost_Posts, Post_Posts>();
            services.AddTransient<IComment_Gets, Comment_Gets>();
            services.AddTransient<IComment_Posts, Comment_Posts>();
            services.AddTransient<ISMUser_Gets, SMUser_Gets>();
            services.AddTransient<ISMUser_Posts, SMUser_Posts>();
            services.AddTransient<IPostBussinessRules, PostBussinessRules>();
            services.AddTransient<ICommentBussinessRules, CommentBussinessRules>();
            services.AddTransient<ISMUserBussinessRules, SMUserBussinessRules>();
            services.AddTransient<IAuthentication, Authentication>();
            services.AddTransient<IAdminUser, AdminUser>();
            services.AddTransient<ISendEmail, SendEmail>(); 
            services.AddTransient<IConfirmEmail, ConfirmEmail>();
            services.AddTransient<IAccountConfirmed, AccountConfirmed>();
            services.AddTransient<IChangePasswordService, ChangePasswordService>();
            services.AddSingleton<IProcessIdentityErrors, ProcessIdentityErrors>(); 
            services.AddTransient<IResetPasswordService, ResetPasswordService>();
            services.AddTransient<IUploadImages, UploadImages>();
            services.AddTransient<ICountry, Country>();

            services.AddSingleton<IUriService>(
                provider =>
                {
                    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                    var request = accesor.HttpContext.Request;
                    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(absoluteUri);
                });
            //Este servicio se utiliza para obtener el Uri real de la aplicacion Ej. https://www.misapis.com.
            //Conforma el Uri y lo inyecta en el constructor de la clase UriService para se usado segun convenga.
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PagingConfiguration>(options => configuration.GetSection("Pagination").Bind(options));
            services.Configure<MailSettings>(options => configuration.GetSection("MailSettings").Bind(options));
            //AL EXTENDER ESTE TIPO DE SERVICIO SE AÑADE EL LAMBDA Y EL BIND. 
            //De esta forma se configura un servicio que utiliza secciones de configuracion del appsettings.
            //PagingConfiguration es una clase con los parametros idem a la seccion de configuracion Paginations.
            //Haciendo esto mapea los valores de la seccion de configuracion al objeto inyectado donde se necesite.
            //En este caso se uso en el Repositorio Post_Gets para obtener opciones por defecto de paginacion.
        }

    }
}
