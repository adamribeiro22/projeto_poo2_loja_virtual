using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Configurations
{
    public class EstoqueConfiguration : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("ESTOQUE");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ID_ESTOQUE");

            builder.Property(e => e.VariacaoProdutoId).HasColumnName("ID_VARIACAO_PRODUTO").IsRequired();
            builder.HasIndex(e => e.VariacaoProdutoId).IsUnique();

            builder.Property(e => e.Quantidade).HasColumnName("NR_QUANTIDADE").IsRequired().HasDefaultValue(0);

            builder.Property(e => e.CriadoEm).HasColumnName("DT_CRIADO_EM").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM").IsRequired();

            // por ser 1:1 ele especifica que a chave estrangeira está em Estoque "HasForeignKey<Estoque>"
            builder.HasOne(e => e.VariacaoProduto).WithOne(vp => vp.Estoque).HasForeignKey<Estoque>(e => e.VariacaoProdutoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
