using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Configurations
{
    public class VendaConfiguration : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("VENDAS");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasColumnName("ID_VENDA");

            builder.Property(v => v.UsuarioId).HasColumnName("ID_USUARIO");
            builder.Property(v => v.DataVenda).HasColumnName("DT_VENDA").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(v => v.ValorTotal).HasColumnName("VL_TOTAL_VENDA").IsRequired().HasColumnType("decimal(10, 2)");
            builder.Property(v => v.Status).HasColumnName("TP_STATUS").IsRequired();

            builder.Property(v => v.CriadoEm).HasColumnName("DT_CRIADO_EM").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(v => v.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM").IsRequired();

            builder.HasOne(v => v.Usuario).WithMany(u => u.Vendas).HasForeignKey(v => v.UsuarioId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
