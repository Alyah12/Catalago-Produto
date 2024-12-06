using APICatalago.Context;
using APICatalago.Dtos;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[ApiController]
[Route("[controller]")]

public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }
 
    [HttpGet]      
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _context.Produtos.ToListAsync();
        if (produtos is null)
        {
            return NotFound("Produto não encontrado...");
        }
        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<ProdutoDto> Get(int id)
    {
        var produto = _context.Produtos?.Where(x => x.ProdutoId == id).Select(x => new ProdutoDto
        {
            ProdutoId = x.ProdutoId,
            Descricao = x.Descricao,
            Preco = x.Preco
        }).FirstOrDefault();
        
        if (produto is null)
        {
            return NotFound();
        }
        return Ok(produto);
    }

    [HttpPost]
    public async Task <ActionResult<ProdutoDto>> Post([FromBody] Produto produto)
    {
        if (produto is null || !ModelState.IsValid)
        {
            BadRequest();
        }
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
        
        return  CreatedAtRoute("ObterProduto",new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Produto> Put(int id, Produto produto)
    {

        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }
        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public async Task <ActionResult<Produto>> Delete(int id)
    {
       var produto = await _context.Produtos.FindAsync(id);
    
        if (produto is null)
        {
            return NotFound();
        }
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        
        return Ok(produto); 
    }
}