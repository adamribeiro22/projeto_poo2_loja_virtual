using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Configurations
{
    public class VariacaoProdutoConfiguration : IEntityTypeConfiguration<VariacaoProduto>
    {
        public void Configure(EntityTypeBuilder<VariacaoProduto> builder)
        {
            builder.ToTable("VARIACOES_PRODUTO");

            builder.HasKey(vp => vp.Id);
            builder.Property(vp => vp.Id).HasColumnName("ID_VARIACAO_PRODUTO");

            builder.Property(vp => vp.ProdutoId).HasColumnName("ID_PRODUTO").IsRequired();
            builder.Property(vp => vp.Tamanho).HasColumnName("DS_TAMANHO").HasMaxLength(50);
            builder.Property(vp => vp.Cor).HasColumnName("DS_COR").HasMaxLength(50);
            builder.Property(vp => vp.Preco).HasColumnName("VL_PRECO").IsRequired().HasColumnType("decimal(10, 2)");
            builder.Property(vp => vp.Ativo).HasColumnName("FL_ATIVO").IsRequired().HasDefaultValue(true);

            builder.Property(vp => vp.CriadoEm).HasColumnName("DT_CRIADO_EM").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(vp => vp.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM").IsRequired();

            builder.HasOne(vp => vp.Produto).WithMany(p => p.Variacoes).HasForeignKey(vp => vp.ProdutoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
