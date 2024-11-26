using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers;

[ApiController]
[Route("[controller]")]

public class ProdutosController : ControllerBase
{
    private readonly DbContextOptions _context;

    public ProdutosController(DbContextOptions context)
    {
        _context = context;
    }

    public IEnumerable<Produto> Get()
    {
        var produtos = _context.Produtos.ToList();
        return produtos;
    }
}