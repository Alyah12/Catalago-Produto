using APICatalago.Context;
using APICatalago.Dtos;
using APICatalago.Migrations;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriasController : Controller
{
    public readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }
    // GET: /categorias/produtos
    // Retorna as categorias com os produtos associados, limitando a 5 categorias.  
    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasProdutos()
    {
        try
        {
            var categoria = await _context?.Categorias.Include(p => p.Produtos)
                .Where(c => c.CategoriaId <= 5).Take(5).Select(
                c => new CategoriaDto
                {
                    CategoriaId = c.CategoriaId,
                    Nome = c.Nome,
                }).AsNoTracking().ToListAsync();

            if (categoria is null)
            {
                return BadRequest();
            }
            
            return categoria;
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter Categorias: {e.Message}");
        }
    }
    // GET: Categorias
    // Esse metodo retorna as categorias com paginação.
    [HttpGet("categorias")]
    public async Task <ActionResult<IEnumerable<CategoriaDto>>> GetAll (int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var categoria = await _context.Categorias
                .Select( x => new CategoriaDto()
                {
                    CategoriaId = x.CategoriaId,
                    Nome = x.Nome
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
 
            if (categoria is null)
            {
                return BadRequest();
            }
            return Ok(categoria);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter Categorias: {e.Message}");
        }
    }
    // Busca categorias por ID
    [HttpGet("{id}")]
    public async Task <ActionResult<CategoriaDto>> GetByIdAsync(int id)
    {
        var categoria = await _context.Categorias.Where(c => c.CategoriaId == id).Select( x =>new CategoriaDto()
        {
            CategoriaId = x.CategoriaId,
            Nome = x.Nome
        }).FirstOrDefaultAsync();

        if (categoria is null)
        {
            NotFound();
        }
        return Ok(categoria);
    }
    // Adiciona uma nova categoria, caso ter sucesso retorna 200.
    
    [HttpPost]
    public async Task <ActionResult<CategoriaDto>> PostAsync(Categoria categoria)
    {
        try 
        {
            if (categoria is null || !ModelState.IsValid)
            {
                BadRequest();
            }

            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }
        catch (Exception e) when (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            // Log the exception (not shown here for brevity)
            System.Console.WriteLine("Hoje é sexta-feira, não é permitido inserir categorias.");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir Categoria: {e.Message}");
        }
    }
    
    // Modifica uma determinada categoria pelo ID, salva e retorna 200.
    [HttpPut("{id}")]
    public ActionResult<Categoria> PutAsync(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
           return BadRequest();
        }
        
        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChangesAsync();
        return Ok(categoria);
    }

    [HttpDelete("{id}")]
    public async Task <ActionResult<Categoria>> DeleteAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria is null)
        {
            return BadRequest();
        } 
        _context.Categorias.Remove(categoria);
       await _context.SaveChangesAsync();
        
        return Ok(categoria);
    }
}