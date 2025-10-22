using AutoMapper;
using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Update;
using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;
using LojaVirtual.Domain.Interfaces;
using LojaVirtual.Infrastructure.Helper.Atelie.Core.Utils;

namespace LojaVirtual.Application.Service
{
    /// <summary>
    /// Implementação da interface do serviço de produtos (IProdutoService).
    /// Perceba que ele tem dois atributos: 
    /// _unitOfWork: permite o acesso aos reposítórios, como documentado na interface IUnitOfWork;
    /// _mapper: permite conversões entre DTOs e as entidades de domínio, uma ver que algumas camadas não tem acesso ao Domain
    /// então criasse esses mappers para que as camadas de aplicação possam converter os DTOs em entidades de domínio e vice-versa.
    /// </summary>
    public class ProdutoService : IProdutoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProdutoDisplayDTO> CreateAsync(ProdutoCreateDTO criarProdutoDisplayDTO)
        {
            var produto = _mapper.Map<Produto>(criarProdutoDisplayDTO);
            AuditHelper.UpdateAuditFields(produto);
            produto.Ativo = true;

            int index = 0;
            foreach (var variacao in produto.Variacoes)
            {
                AuditHelper.UpdateAuditFields(variacao);
                variacao.Ativo = true;
                variacao.Estoque = new Estoque
                {
                    Quantidade = criarProdutoDisplayDTO.Variacoes.ElementAt(index).QuantidadeEstoqueInicial
                };
                index++;
            }

            await _unitOfWork.ProdutoRepository.CreateAsync(produto);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProdutoDisplayDTO>(produto);
        }

        public async Task DeleteAsync(int id)
        {
            var produtoParaExcluir = await _unitOfWork.ProdutoRepository.GetByIdAsync(id);
            if (produtoParaExcluir == null)
            {
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            var possuiVendas = await _unitOfWork.ProdutoRepository.HasAssociatedSalesAsync(id);
            if (possuiVendas)
            {
                throw new InvalidOperationException("Não é possível excluir um produto que possui vendas registradas. Considere desativá-lo.");
            }

            _unitOfWork.ProdutoRepository.Delete(produtoParaExcluir);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProdutoDisplayDTO>> GetAllAsync(ProdutoQueryDTO? query)
        {
            if (query == null || (string.IsNullOrWhiteSpace(query.Nome) && string.IsNullOrWhiteSpace(query.Categoria) && !query.Ativo.HasValue))
            {
                var todosProdutos = await _unitOfWork.ProdutoRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProdutoDisplayDTO>>(todosProdutos);
            }

            var filtro = _mapper.Map<ProdutoFilter>(query);
            var produtosFiltrados = await _unitOfWork.ProdutoRepository.GetWithFiltersAsync(filtro);

            return _mapper.Map<IEnumerable<ProdutoDisplayDTO>>(produtosFiltrados);
        }

        public async Task<IEnumerable<ProdutoDisplayDTO>> GetAllAsyncWithDetails(ProdutoQueryDTO? query)
        {
            var produtosComDetalhes = await _unitOfWork.ProdutoRepository.GetAllWithDetailsAsync();

            if (query != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Nome))
                {
                    produtosComDetalhes = produtosComDetalhes.Where(p => p.Nome.Contains(query.Nome, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrWhiteSpace(query.Categoria))
                {
                    produtosComDetalhes = produtosComDetalhes.Where(p => p.Categoria != null && p.Categoria.Equals(query.Categoria, StringComparison.OrdinalIgnoreCase));
                }
                if (query.Ativo.HasValue)
                {
                    produtosComDetalhes = produtosComDetalhes.Where(p => p.Ativo == query.Ativo.Value);
                }
            }

            return _mapper.Map<IEnumerable<ProdutoDisplayDTO>>(produtosComDetalhes);
        }

        public async Task<ProdutoDisplayDTO?> GetByIdAsync(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetByIdWithVariationsAsync(id);

            return _mapper.Map<ProdutoDisplayDTO>(produto);
        }

        public async Task UpdateAsync(int id, ProdutoUpdateDTO atualizarProdutoDisplayDTO)
        {
            var produtoExistente = await _unitOfWork.ProdutoRepository.GetByIdAsync(id);
            if (produtoExistente != null)
            {
                _mapper.Map(atualizarProdutoDisplayDTO, produtoExistente);
                AuditHelper.UpdateAuditFields(produtoExistente);

                _unitOfWork.ProdutoRepository.Update(produtoExistente);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CancelAsync(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetByIdWithVariationsAsync(id);
            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            produto.Ativo = !produto.Ativo;
            AuditHelper.UpdateAuditFields(produto);

            foreach (var variacao in produto.Variacoes)
            {
                variacao.Ativo = false;
                AuditHelper.UpdateAuditFields(variacao);
            }

            await _unitOfWork.CommitAsync();
        }
    }
}
