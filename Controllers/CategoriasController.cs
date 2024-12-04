using APICatalago.Context;
using APICatalago.Dtos;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriasController : ControllerBase
{
    public readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task <ActionResult<IEnumerable<Categoria>>> Get()
    {
        var produto = _context.Categorias.ToListAsync();
        return await produto;
    }

    [HttpGet("{id}")]
    public ActionResult<CategoriaDto> Get(int id)
    {
        var categoria = _context.Categorias.Where(c => c.CategoriaId == id).Select( x =>new CategoriaDto()
        {
            CategoriaId = x.CategoriaId,
            Nome = x.Nome
        }).FirstOrDefault();

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