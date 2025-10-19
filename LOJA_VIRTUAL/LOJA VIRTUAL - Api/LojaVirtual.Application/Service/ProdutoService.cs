using AutoMapper;
using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
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

            await _unitOfWork.ProdutoRepository.CreateAsync(produto);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProdutoDisplayDTO>(produto);
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetByIdAsync(id);
            if (produto != null)
            {
                _unitOfWork.ProdutoRepository.Delete(produto);
                await _unitOfWork.CommitAsync();
            }
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

        public async Task<ProdutoDisplayDTO?> GetByIdAsync(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetByIdAsync(id);

            return _mapper.Map<ProdutoDisplayDTO>(produto);
        }

        public async Task UpdateAsync(int id, ProdutoCreateDTO atualizarProdutoDisplayDTO)
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
    }
}
