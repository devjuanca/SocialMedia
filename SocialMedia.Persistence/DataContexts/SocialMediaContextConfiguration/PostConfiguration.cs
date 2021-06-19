using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> entity)
        {
            entity.ToTable("Post");
            entity.Property(e => e.PostId).HasColumnName("Post_Id");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Image).HasMaxLength(100);

            entity.Property(e => e.SmuserId)
                .IsRequired()
                .HasColumnName("SMUser_Id")
                .HasMaxLength(450);

            entity.HasOne(d => d.Smuser)
                .WithMany(p => p.Post)
                .HasForeignKey(d => d.SmuserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_AspNetUsers");
        }
    }
}
