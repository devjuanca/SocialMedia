using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Entities.CustomEntities;
using SocialMedia.Persistence.Configurations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SocialMedia.Persistence
{
    public partial class SocialMediaContext : IdentityDbContext<SMUser>
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=USUARIO-VAIO\\SQLEXPRESS;Initial Catalog=SocialMedia2.0;Integrated Security=True");
        //    }
        //}


        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<SMUser>SMUsers{get;set;}
        public virtual DbSet<Country> Country { get; set; }


        public virtual DbSet<SocialMedia.Domain.Entities.CustomEntities.S_CommentsFromPost> CommentsProc { get; set; }
     
     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            //modelBuilder.ApplyConfiguration(new SMUserConfiguration());
            //Estos metodos aplican la configuracion de las clases definidas.
            //De esta forma no se sobrecarga el DBContext con DB muy grandes.
            base.OnModelCreating(modelBuilder);  //Configura el esquema necesario de la clase base IdentityDbContext.

            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //Registra las configuraciones a nivel de Assembly pata no tener que hacerlo de a uno.

            modelBuilder.Ignore<S_CommentsFromPost>(); //Entidad no mapeada en BD.
            modelBuilder.Entity<S_CommentsFromPost>().HasKey(a => a.Comment_Id);
            //Custom Entity para recibir resultados de un Stored Procedure.
        }


    }
}
