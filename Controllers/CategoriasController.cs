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

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasProdutos()
    {
        try
        {
            var categoria = await _context?.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).Take(5).Select(
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

    [HttpGet("{id}")]
    public async Task <ActionResult<CategoriaDto>> GetById(int id)
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

    [HttpPost]
    public async Task <ActionResult<CategoriaDto>> Post(Categoria categoria)
    {
        if (categoria is null || !ModelState.IsValid)
        {
            BadRequest();
        }
        
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
        
        return new CreatedAtRouteResult("ObterCategoria", new {id = categoria.CategoriaId}, categoria);
    }
    
    
    [HttpPut("{id}")]
    public ActionResult<Categoria> Put(int id, Categoria categoria)
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
    public async Task <ActionResult<Categoria>> Delete(int id)
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