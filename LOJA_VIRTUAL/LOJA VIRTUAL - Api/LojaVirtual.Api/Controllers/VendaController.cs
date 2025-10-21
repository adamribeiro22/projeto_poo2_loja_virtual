using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Update;
using LojaVirtual.Application.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] VendaQueryDTO? query)
        {
            var vendas = await _vendaService.GetAllAsync(query);

            return Ok(vendas);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { title = "Not Found", status = 404, detail = $"Venda com ID {id} não encontrada." });
            }

            return Ok(venda);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(VendaCreateDTO dto)
        {
            try
            {
                var novaVenda = await _vendaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = novaVenda.Id }, novaVenda);
            }
            catch (ValidationException ex)
            {
                var problemDetails = new
                {
                    title = "Uma ou mais validações tiverem erros.",
                    status = 400,
                    errors = ex.Errors
                };

                return BadRequest(problemDetails);
            }
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] AtualizarStatusVendaDTO dto)
        {
            try
            {
                await _vendaService.UpdateStatusAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { title = "Not Found", status = 404, detail = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { title = "Bad Request", status = 400, detail = ex.Message });
            }
        }

        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _vendaService.CancelAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { title = "Not Found", status = 404, detail = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { title = "Bad Request", status = 400, detail = ex.Message });
            }
        }
    }
}