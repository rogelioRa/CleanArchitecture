using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Infrastructure.Data.Configuration
{
  public class CommentConfiguration : IEntityTypeConfiguration<Comment>
  {
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("Comentario");

        builder.Property(e => e.Id)
            .HasColumnName("IdComentario")
            .ValueGeneratedNever();

        builder.Property(e => e.Description)
            .HasColumnName("Descripcion")
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(e => e.Date)
            .HasColumnName("Fecha")
            .HasColumnType("datetime");

        builder.HasOne(d => d.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(d => d.PostId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Comentario_Publicacion");

        builder.HasOne(d => d.User)
            .WithMany(p => p.Comments)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Comentario_Usuario");
    }
  }
}