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
    public class EstoqueService : IEstoqueService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;

        public EstoqueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EstoqueDisplayDTO> CreateAsync(EstoqueCreateDTO criarEstoqueDto)
        {
            var estoque = _mapper.Map<Estoque>(criarEstoqueDto);
            AuditHelper.UpdateAuditFields(estoque);
            
            await _unitOfWork.EstoqueRepository.CreateAsync(estoque);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<EstoqueDisplayDTO>(estoque);
        }

        public async Task DeleteAsync(int id)
        {
            var estoque = await _unitOfWork.EstoqueRepository.GetByIdAsync(id);
            if (estoque != null)
            {
                _unitOfWork.EstoqueRepository.Delete(estoque);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<IEnumerable<EstoqueDisplayDTO>> GetAllAsync(EstoqueQueryDTO? query)
        {
            bool isQueryEmpty = query == null ||
                                    (!query.VariacaoProdutoId.HasValue &&
                                     !query.QuantidadeMinima.HasValue &&
                                     !query.QuantidadeMaxima.HasValue);

            if (isQueryEmpty)
            {
                var estoques = await _unitOfWork.EstoqueRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<EstoqueDisplayDTO>>(estoques);
            }

            var filtro = _mapper.Map<EstoqueFilter>(query);
            var estoquesFiltrados = await _unitOfWork.EstoqueRepository.GetWithFiltersAsync(filtro);

            return _mapper.Map<IEnumerable<EstoqueDisplayDTO>>(estoquesFiltrados);
        }

        public async Task<EstoqueDisplayDTO?> GetByIdAsync(int id)
        {
            var estoque = await _unitOfWork.EstoqueRepository.GetByIdAsync(id);
            return _mapper.Map<EstoqueDisplayDTO>(estoque);
        }

        public async Task UpdateAsync(int id, EstoqueCreateDTO atualizarProdutoDto)
        {
            var estoque = await _unitOfWork.EstoqueRepository.GetByIdAsync(id);
            if(estoque != null)
            {
                _mapper.Map(atualizarProdutoDto, estoque);
                AuditHelper.UpdateAuditFields(estoque);

                _unitOfWork.EstoqueRepository.Update(estoque);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
