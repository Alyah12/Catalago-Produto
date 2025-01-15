using APICatalago.Context;
using APICatalago.Dtos;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }
 
    [HttpGet]      
    public async Task <ActionResult<IEnumerable<Produto>>> GetAll()
    {
        try
        { 
            var produtos = await _context.Produtos.Take(5).Where(c => c.ProdutoId <= 5).ToListAsync();
            if (produtos is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produtos);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os produtos. Erro: {e.Message}");
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task <ActionResult<ProdutoDto>> GetById (int id)
    {
        var produto = await _context?.Produtos.Where(x => x.ProdutoId == id).Select(x => new ProdutoDto
        {
            ProdutoId = x.ProdutoId,
            Descricao = x.Descricao,
            Preco = x.Preco
        }).FirstOrDefaultAsync();
        
        if (produto is null)
        {
            return NotFound($"Não foi possível encontrar o produto com id {id}...");
        }
        return Ok(produto);
    }

    [HttpPost]
    public async Task <ActionResult<ProdutoDto>> Post([FromBody] Produto produto)
    {
        if (produto is null || !ModelState.IsValid)
        {
            BadRequest("Produto inválido...");
        }
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
        
        return  CreatedAtRoute("ObterProduto",new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<Produto> Put(int id, Produto produto)
    {

        if (id != produto.ProdutoId)
        {
            return BadRequest("Produto não encontrado...");
        }
        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task <ActionResult<Produto>> Delete(int id)
    {
       var produto = await _context.Produtos.FindAsync(id);
    
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        
        return Ok(produto); 
    }
}