using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Api.Controllers
{
    /// <summary>
    /// Controlador de Produtos, definimos cada end point aqui, que será usado para fazer as requisições do front.
    /// Como pode notar, ele tem uma instancia de IProdutoService, e ao chamar um método de ProdutoService, la dentro ele chama do repositório,
    /// que acessa o banco, etc... assim por diante, fazendo todas verificações e configurações que montamos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet] // definição do método HTTP, nesse caso GET
        [ProducesResponseType(StatusCodes.Status200OK)] // indica os tipos de retorno que podem ser esperados nesse endpoint "GetAll", facilita a compreensão na hora de criar o front
        public async Task<IActionResult> GetAll([FromQuery] ProdutoQueryDTO? query) // FromQuery indica que o parâmetro "query" será passado via query string na URL, como por exemplo: /api/produtos?nome=produto1&categoria=eletronicos
        {
            var produtos = await _produtoService.GetAllAsync(query);
            return Ok(produtos); // retorna status code de 200, o Ok
        }

        [HttpGet("{id}")] // novamente método HTTP, mas que recebe um id
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);

            if (produto == null)
            {
                return NotFound(); // 404 not found
            }

            return Ok(produto);
        }

        [HttpPost] // método http Post nesse caso
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProdutoCreateDTO criarProdutoDTO) // FromBody indica que o objeto será recebido no corpo da requisição, como JSON por exemplo
        {
            var novoProduto = await _produtoService.CreateAsync(criarProdutoDTO);

            return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProdutoCreateDTO atualizarProdutoDTO)
        {
            var produtoExistente = await _produtoService.GetByIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            await _produtoService.UpdateAsync(id, atualizarProdutoDTO);

            return NoContent(); // Retorna 204 No Content para indicar sucesso sem corpo de resposta
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var produtoExistente = await _produtoService.GetByIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            await _produtoService.DeleteAsync(id);

            return NoContent(); // Retorna 204 No Content para indicar sucesso sem corpo de resposta
        }
    }
}