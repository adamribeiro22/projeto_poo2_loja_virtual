using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuração da entidade Produto para o Entity Framework Core.
    /// Aqui a gente define como a tabela PRODUTOS será mapeada no banco de dados.
    /// Representa as flags dos atributos do banco de dados, atrelada ao AppDbContext.
    /// </summary>
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("PRODUTOS");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("ID_PRODUTO");

            builder.Property(p => p.Nome).HasColumnName("NM_PRODUTO").IsRequired().HasMaxLength(255);
            builder.Property(p => p.Descricao).HasColumnName("DS_PRODUTO").HasColumnType("text");
            builder.Property(p => p.Categoria).HasColumnName("NM_CATEGORIA").HasMaxLength(100);
            builder.Property(p => p.Ativo).HasColumnName("FL_ATIVO").IsRequired().HasDefaultValue(true);

            builder.Property(p => p.CriadoEm).HasColumnName("DT_CRIADO_EM").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(p => p.AtualizadoEm).HasColumnName("DT_ATUALIZADO_EM").IsRequired();
        }
    }
}
