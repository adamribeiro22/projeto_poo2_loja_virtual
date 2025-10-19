using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] EstoqueQueryDTO? query)
        {
            var estoque = await _estoqueService.GetAllAsync(query);
            return Ok(estoque);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var estoque = await _estoqueService.GetByIdAsync(id);

            if(estoque == null)
            {
                return NotFound();
            }

            return Ok(estoque);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] EstoqueCreateDTO criarEstoqueDTO)
        {
            var novoEstoque = await _estoqueService.CreateAsync(criarEstoqueDTO);
            return CreatedAtAction(nameof(GetById), new { id = novoEstoque.Id }, novoEstoque);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] EstoqueCreateDTO atualizarEstoqueDTO)
        {
            var estoqueExistente = await _estoqueService.GetByIdAsync(id);
            if (estoqueExistente == null)
            {
                return NotFound();
            }

            await _estoqueService.UpdateAsync(id, atualizarEstoqueDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var estoqueExistente = await _estoqueService.GetByIdAsync(id);
            if (estoqueExistente == null)
            {
                return NotFound();
            }

            await _estoqueService.DeleteAsync(id);

            return NoContent();
        }
    }
}
