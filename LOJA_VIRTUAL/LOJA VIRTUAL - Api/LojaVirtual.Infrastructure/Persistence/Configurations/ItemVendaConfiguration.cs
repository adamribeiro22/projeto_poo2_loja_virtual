using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Configurations
{
    public class ItemVendaConfiguration : IEntityTypeConfiguration<ItemVenda>
    {
        public void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            builder.ToTable("ITENS_VENDA");

            builder.HasKey(iv => iv.Id);
            builder.Property(iv => iv.Id).HasColumnName("ID_ITEM_VENDA");

            builder.Property(iv => iv.VendaId).HasColumnName("ID_VENDA").IsRequired();
            builder.Property(iv => iv.VariacaoProdutoId).HasColumnName("ID_VARIACAO_PRODUTO").IsRequired();
            builder.Property(iv => iv.Quantidade).HasColumnName("NR_QUANTIDADE").IsRequired();
            builder.Property(iv => iv.PrecoUnitario).HasColumnName("VL_PRECO_UNITARIO").IsRequired().HasColumnType("decimal(10, 2)");

            builder.Property(iv => iv.CriadoEm).HasColumnName("DT_CRIADO_EM").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(iv => iv.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM").IsRequired();

            builder.HasOne(iv => iv.Venda).WithMany(v => v.Itens).HasForeignKey(iv => iv.VendaId).OnDelete(DeleteBehavior.Cascade);

            // Impede deletar uma variação se existe uma venda com ela no carrinho
            // Por isso, tem se "FL_ATIVO" em VariacaoProduto, tornamos ela inativa ao invés de deletar
            builder.HasOne(iv => iv.VariacaoProduto).WithMany(vp => vp.ItensVenda).HasForeignKey(iv => iv.VariacaoProdutoId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
