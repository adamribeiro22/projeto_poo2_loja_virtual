using AutoMapper;
using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Update;
using LojaVirtual.Application.Exception;
using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Enums;
using LojaVirtual.Domain.Filters;
using LojaVirtual.Domain.Interfaces;
using LojaVirtual.Infrastructure.Helper.Atelie.Core.Utils;

namespace LojaVirtual.Application.Service
{
    public class VendaService : IVendaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private record VendaValidationResult(Dictionary<string, string[]> Errors, List<VariacaoProduto> Variacoes);

        public VendaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VendaDisplayDTO> CreateAsync(VendaCreateDTO dto)
        {
            var validationResult = await ValidarVendaAsync(dto);
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            var novaVenda = ProcessarLogicaDaVenda(dto, validationResult.Variacoes);

            await _unitOfWork.VendaRepository.CreateAsync(novaVenda);
            await _unitOfWork.CommitAsync();

            var vendaCriada = await _unitOfWork.VendaRepository.GetByIdWithDetailsAsync(novaVenda.Id);

            return _mapper.Map<VendaDisplayDTO>(vendaCriada);
        }

        public async Task<VendaDisplayDTO?> GetByIdAsync(int id)
        {
            var venda = await _unitOfWork.VendaRepository.GetByIdWithDetailsAsync(id);

            return _mapper.Map<VendaDisplayDTO>(venda);
        }

        public async Task<IEnumerable<VendaDisplayDTO>> GetAllAsync(VendaQueryDTO? query)
        {
            var filtro = _mapper.Map<VendaFilter>(query);
            var todasVendas = await _unitOfWork.VendaRepository.GetWithFiltersAsync(filtro);

            return _mapper.Map<IEnumerable<VendaDisplayDTO>>(todasVendas);
        }

        public async Task UpdateStatusAsync(int id, AtualizarStatusVendaDTO dto)
        {
            var venda = await _unitOfWork.VendaRepository.GetByIdAsync(id);
            if (venda == null)
            {
                throw new KeyNotFoundException($"Venda com ID {id} não encontrada.");
            }

            if (venda.Status == StatusVenda.Cancelada || venda.Status == StatusVenda.Entregue)
            {
                throw new InvalidOperationException("Não é possível alterar o status de uma venda que já foi entregue ou cancelada.");
            }

            venda.Status = dto.NovoStatus;
            AuditHelper.UpdateAuditFields(venda);

            _unitOfWork.VendaRepository.Update(venda);
            await _unitOfWork.CommitAsync();
        }

        public async Task CancelAsync(int id)
        {
            var venda = await _unitOfWork.VendaRepository.GetByIdWithDetailsAsync(id);

            if (venda == null)
            {
                throw new KeyNotFoundException($"Venda com ID {id} não encontrada.");
            }

            if (venda.Status != StatusVenda.Pendente && venda.Status != StatusVenda.Confirmada)
            {
                throw new InvalidOperationException($"Não é possível cancelar uma venda que já foi entregue, cancelada ou confirmada.");
            }

            foreach (var item in venda.Itens)
            {
                if (item.VariacaoProduto?.Estoque != null)
                {
                    item.VariacaoProduto.Estoque.Quantidade += item.Quantidade;
                }
            }

            venda.Status = StatusVenda.Cancelada;
            AuditHelper.UpdateAuditFields(venda);

            await _unitOfWork.CommitAsync();
        }


        private Venda ProcessarLogicaDaVenda(VendaCreateDTO dto, List<VariacaoProduto> variacoesDoBanco)
        {
            var variacoesDictionary = variacoesDoBanco.ToDictionary(v => v.Id);
            var novaVenda = new Venda { UsuarioId = dto.UsuarioId };
            decimal valorTotalCalculado = 0;

            foreach (var itemDto in dto.Itens)
            {
                var variacao = variacoesDictionary[itemDto.VariacaoProdutoId];
                var novoItem = new ItemVenda
                {
                    VariacaoProdutoId = itemDto.VariacaoProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = variacao.Preco
                };

                novaVenda.Itens.Add(novoItem);
                variacao.Estoque.Quantidade -= itemDto.Quantidade;
                valorTotalCalculado += novoItem.Quantidade * novoItem.PrecoUnitario;
            }

            novaVenda.ValorTotal = valorTotalCalculado;
            novaVenda.DataVenda = DateTime.UtcNow;
            AuditHelper.UpdateAuditFields(novaVenda);

            return novaVenda;
        }

        private async Task<VendaValidationResult> ValidarVendaAsync(VendaCreateDTO dto)
        {
            var errors = new Dictionary<string, string[]>();
            List<VariacaoProduto> variacoesDoBanco = new();

            var usuario = await _unitOfWork.UsuarioRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
            {
                errors.Add(nameof(dto.UsuarioId), [$"Utilizador com ID {dto.UsuarioId} não encontrado."]);
            }

            if (dto.Itens == null || !dto.Itens.Any())
            {
                errors.Add(nameof(dto.Itens), ["A venda deve conter pelo menos um item."]);
                return new VendaValidationResult(errors, variacoesDoBanco);
            }

            var variacaoIds = dto.Itens.Select(i => i.VariacaoProdutoId).ToList();
            variacoesDoBanco = await _unitOfWork.VariacaoProdutoRepository.GetByIdsWithStockAsync(variacaoIds);
            var variacoesDictionary = variacoesDoBanco.ToDictionary(v => v.Id);

            foreach (var (itemDto, index) in dto.Itens.Select((item, index) => (item, index)))
            {
                if (!variacoesDictionary.TryGetValue(itemDto.VariacaoProdutoId, out var variacao))
                {
                    errors.Add($"Itens[{index}].VariacaoProdutoId", [$"Produto com variação ID {itemDto.VariacaoProdutoId} não encontrado."]);
                }
                else if (variacao.Estoque == null || variacao.Estoque.Quantidade < itemDto.Quantidade)
                {
                    errors.Add($"Itens[{index}].Quantidade", [$"Estoque insuficiente para o produto (ID: {variacao.Id})."]);
                }
            }

            return new VendaValidationResult(errors, variacoesDoBanco);
        }
    }
}
