using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Persistence.Configurations
{
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("Comment");
            entity.Property(e => e.CommentId).HasColumnName("Comment_Id");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.Property(e => e.PostId).HasColumnName("Post_Id");

            entity.Property(e => e.SmuserId)
                .IsRequired()
                .HasColumnName("SMUser_Id")
                .HasMaxLength(450);

            entity.HasOne(d => d.Post)
                .WithMany(p => p.Comment)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Post");

            entity.HasOne(d => d.Smuser)
                .WithMany(p => p.Comment)
                .HasForeignKey(d => d.SmuserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_AspNetUsers");
        }
    }
}
