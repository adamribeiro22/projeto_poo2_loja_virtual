using AutoMapper;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;

namespace LojaVirtual.Application.Mapper
{
    /// <summary>
    /// Aqui a gente "ensina" ao AutoMapper como transformar um tipo em outro.
    /// Fazemos essa configuração e outra no arquivo Program.cs na camada LojaVirtual.Api para ele funcionar.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Ensina ao AutoMapper a transformar um  ProdutoCreateDTO em um Produto
            CreateMap<ProdutoCreateDTO, Produto>();
            // Ensina ao AutoMapper a transformar um  Produto em um ProdutoDisplayDTO
            CreateMap<Produto, ProdutoDisplayDTO>();
            // Ensina ao AutoMapper a transformar um ProdutoQueryDTO em um ProdutoFilter
            CreateMap<ProdutoQueryDTO, ProdutoFilter>();

            CreateMap<VariacaoProdutoCreateDTO, VariacaoProduto>();
            CreateMap<VariacaoProduto, VariacaoProdutoDisplayDTO>();
            CreateMap<VariacaoProdutoQueryDTO, VariacaoProdutoFilter>();
        }
    }
}
