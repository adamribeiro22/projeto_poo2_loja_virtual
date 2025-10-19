using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VariacaoProdutosController : ControllerBase
    {
        private readonly IVariacaoProdutoService _variacaoProdutoService;

        public VariacaoProdutosController(IVariacaoProdutoService variacaoProdutoService)
        {
            _variacaoProdutoService = variacaoProdutoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] VariacaoProdutoQueryDTO? query)
        {
            var variacoes = await _variacaoProdutoService.GetAllAsync(query);
            return Ok(variacoes);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var variacao = await _variacaoProdutoService.GetByIdAsync(id);

            if (variacao == null)
            {
                return NotFound();
            }

            return Ok(variacao);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] VariacaoProdutoCreateDTO criarVariacaoProdutoDTO)
        {
            var novaVariacao = await _variacaoProdutoService.CreateAsync(criarVariacaoProdutoDTO);

            return CreatedAtAction(nameof(GetById), new { id = novaVariacao.Id }, novaVariacao);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] VariacaoProdutoCreateDTO atualizarVariacaoProdutoDTO)
        {
            var variacaoExistente = await _variacaoProdutoService.GetByIdAsync(id);
            if (variacaoExistente == null)
            {
                return NotFound();
            }

            await _variacaoProdutoService.UpdateAsync(id, atualizarVariacaoProdutoDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var variacaoExistente = await _variacaoProdutoService.GetByIdAsync(id);
            if (variacaoExistente == null)
            {
                return NotFound();
            }

            await _variacaoProdutoService.DeleteAsync(id);

            return NoContent();
        }
    }
}