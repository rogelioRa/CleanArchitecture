using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Infrastructure.Data.Configuration
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("Usuario");
        
        builder.Property(e => e.Id)
            .HasColumnName("IdUsuario");

        builder.Property(e => e.Lastname)
            .HasColumnName("Apellidos")
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(30)
            .IsUnicode(false);

        builder.Property(e => e.Birthday)
            .HasColumnName("FechaNacimiento")
            .HasColumnType("date");

        builder.Property(e => e.Name)
            .HasColumnName("Nombres")
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Phone)
            .HasColumnName("Telefono")
            .HasMaxLength(10)
            .IsUnicode(false);

        builder.Property(e => e.IsActive)
            .HasColumnName("Activo")
            .HasColumnType("bit")
            .HasDefaultValue(0);
    }
  }
}