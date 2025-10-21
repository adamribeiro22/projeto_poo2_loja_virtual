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
    public class VariacaoProdutoService : IVariacaoProdutoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VariacaoProdutoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VariacaoProdutoDisplayDTO> CreateAsync(VariacaoProdutoCreateDTO dto)
        {
            var variacao = _mapper.Map<VariacaoProduto>(dto);
            AuditHelper.UpdateAuditFields(variacao);
            variacao.Ativo = true;

            var novoEstoque = new Estoque
            {
                Quantidade = dto.QuantidadeEstoqueInicial
            };
            AuditHelper.UpdateAuditFields(novoEstoque);

            variacao.Estoque = novoEstoque;

            await _unitOfWork.VariacaoProdutoRepository.CreateAsync(variacao);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<VariacaoProdutoDisplayDTO>(variacao);
        }

        public async Task DeleteAsync(int id)
        {
            var variacaoParaExcluir = await _unitOfWork.VariacaoProdutoRepository.GetByIdAsync(id);
            if (variacaoParaExcluir == null)
            {
                throw new KeyNotFoundException($"Variação de produto com ID {id} não encontrada.");
            }

            var possuiVendas = await _unitOfWork.VariacaoProdutoRepository.HasAssociatedSalesAsync(id);
            if (possuiVendas)
            {
                throw new InvalidOperationException("Não é possível excluir uma variação de produto que já possui vendas registradas. Considere desativá-la.");
            }

            _unitOfWork.VariacaoProdutoRepository.Delete(variacaoParaExcluir);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<VariacaoProdutoDisplayDTO>> GetAllAsync(VariacaoProdutoQueryDTO? query)
        {
            bool isQueryEmpty = query == null ||
                                (!query.ProdutoId.HasValue &&
                                 string.IsNullOrWhiteSpace(query.Tamanho) &&
                                 string.IsNullOrWhiteSpace(query.Cor) &&
                                 !query.PrecoMinimo.HasValue &&
                                 !query.PrecoMaximo.HasValue &&
                                 !query.Ativo.HasValue);

            if (isQueryEmpty)
            {
                var todasVariacoes = await _unitOfWork.VariacaoProdutoRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<VariacaoProdutoDisplayDTO>>(todasVariacoes);
            }

            var filtro = _mapper.Map<VariacaoProdutoFilter>(query);
            var variacoesFiltradas = await _unitOfWork.VariacaoProdutoRepository.GetWithFiltersAsync(filtro);

            return _mapper.Map<IEnumerable<VariacaoProdutoDisplayDTO>>(variacoesFiltradas);
        }

        public async Task<VariacaoProdutoDisplayDTO?> GetByIdAsync(int id)
        {
            var variacao = await _unitOfWork.VariacaoProdutoRepository.GetByIdAsync(id);
            return _mapper.Map<VariacaoProdutoDisplayDTO>(variacao);
        }

        public async Task UpdateAsync(int id, VariacaoProdutoCreateDTO dto)
        {
            var variacaoExistente = await _unitOfWork.VariacaoProdutoRepository.GetByIdAsync(id, vp => vp.Estoque);
            if (variacaoExistente != null)
            {
                _mapper.Map(dto, variacaoExistente);
                AuditHelper.UpdateAuditFields(variacaoExistente);

                variacaoExistente.Estoque.Quantidade = dto.QuantidadeEstoqueInicial;
                AuditHelper.UpdateAuditFields(variacaoExistente.Estoque);

                _unitOfWork.VariacaoProdutoRepository.Update(variacaoExistente);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task CancelAsync(int id)
        {
            var variacao = await _unitOfWork.VariacaoProdutoRepository.GetByIdAsync(id);
            if (variacao == null)
            {
                throw new KeyNotFoundException($"Variação de produto com ID {id} não encontrada.");
            }

            variacao.Ativo = false;
            AuditHelper.UpdateAuditFields(variacao);

            _unitOfWork.VariacaoProdutoRepository.Update(variacao);
            await _unitOfWork.CommitAsync();
        }
    }
}
