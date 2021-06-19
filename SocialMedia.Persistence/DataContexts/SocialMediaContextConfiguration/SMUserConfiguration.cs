using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Persistence.Configurations
{
    public class SMUserConfiguration : IEntityTypeConfiguration<SMUser>
    {
        public void Configure(EntityTypeBuilder<SMUser> builder)
        {
            builder.ToTable("SMUser");

            builder.Property(e => e.Id).HasColumnName("SMUser_Id");

            builder.Property(e => e.BirthDate).HasColumnType("date");
            
            builder.Property(e => e.Lastname).HasMaxLength(100);

            builder.Property(e => e.Name).HasMaxLength(100);

            builder.HasOne(d => d.Country)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_AspNetUsers_Country");
        }
    }
}
