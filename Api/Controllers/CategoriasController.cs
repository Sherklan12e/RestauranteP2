using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public CategoriasController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoriaplato>>> GetCategorias()
    {
        return await _context.Categoriaplatos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoriaplato>> GetCategoria(uint id)
    {
        var categoria = await _context.Categoriaplatos.FindAsync(id);

        if (categoria == null)
        {
            return NotFound();
        }

        return categoria;
    }

    [HttpPost]
    public async Task<ActionResult<Categoriaplato>> PostCategoria(Categoriaplato categoria)
    {
        _context.Categoriaplatos.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategoria", new { id = categoria.IdCategoria }, categoria);
    }
}