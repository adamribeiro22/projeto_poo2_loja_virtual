using AutoMapper;
using LojaVirtual.Application.DTO.Auth;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Update;
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
            CreateMap<ProdutoCreateDTO, Produto>();
            CreateMap<ProdutoQueryDTO, ProdutoFilter>();
            CreateMap<Produto, ProdutoDisplayDTO>();
            CreateMap<ProdutoUpdateDTO, Produto>();

            CreateMap<VariacaoProdutoCreateDTO, VariacaoProduto>();
            CreateMap<VariacaoProdutoQueryDTO, VariacaoProdutoFilter>();
            CreateMap<VariacaoProduto, VariacaoProdutoDisplayDTO>();

            CreateMap<EstoqueCreateDTO, Estoque>();
            CreateMap<EstoqueQueryDTO, EstoqueFilter>();
            CreateMap<Estoque, EstoqueDisplayDTO>();

            CreateMap<RegisterRequestDTO, Usuario>();
            CreateMap<Usuario, UsuarioDisplayDTO>()    
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString())); // Usuario tem Enum, DTO tem string, aqui ele faz essa conversão pra string

            CreateMap<VendaCreateDTO, Venda>();
            CreateMap<VendaQueryDTO, VendaFilter>();
            CreateMap<Venda, VendaDisplayDTO>()
                .ForMember(
                    dest => dest.NomeUsuario,
                    opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nome : "Utilizador Removido")
                )
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ItemVendaCreateDTO, ItemVenda>();
            CreateMap<ItemVendaQueryDTO, ItemVendaFilter>();
            CreateMap<ItemVenda, ItemVendaDisplayDTO>()
                .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.VariacaoProduto.Produto.Nome))
                .ForMember(dest => dest.Tamanho, opt => opt.MapFrom(src => src.VariacaoProduto.Tamanho))
                .ForMember(dest => dest.Cor, opt => opt.MapFrom(src => src.VariacaoProduto.Cor));
        }
    }
}
