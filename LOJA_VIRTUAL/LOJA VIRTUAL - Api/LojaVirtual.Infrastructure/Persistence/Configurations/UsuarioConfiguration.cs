using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("USUARIOS");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("ID_USUARIO");

            builder.Property(u => u.Nome).HasColumnName("NM_USUARIO").IsRequired().HasMaxLength(255);
            builder.Property(u => u.Email).HasColumnName("DS_EMAIL").IsRequired().HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.SenhaHash).HasColumnName("DS_SENHA_HASH").IsRequired().HasMaxLength(255);
            builder.Property(u => u.Tipo).HasColumnName("TP_USUARIO").IsRequired();

            builder.Property(u => u.CriadoEm).HasColumnName("DT_CRIADO_EM").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(u => u.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM").IsRequired();
        }
    }
}
