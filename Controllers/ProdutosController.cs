using APICatalago.Context;
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

    public IEnumerable<Produto> Get()
    {
        var produtos = _context.Produtos.ToList();
        return produtos;
    }
}