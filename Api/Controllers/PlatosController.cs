using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatosController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public PlatosController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Plato>>> GetPlatos()
    {
        return await _context.Platos
            .Where(p => p.Activo)
            .Include(p => p.IdCategoriaNavigation)
            .ToListAsync();
    }

    [HttpGet("disponibles")]
    public async Task<ActionResult<IEnumerable<Plato>>> GetPlatosDisponibles()
    {
        return await _context.Platos
            .Where(p => p.Disponible && p.Activo)
            .Include(p => p.IdCategoriaNavigation)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Plato>> GetPlato(uint id)
    {
        var plato = await _context.Platos
            .Include(p => p.IdCategoriaNavigation)
            .FirstOrDefaultAsync(p => p.IdPlato == id && p.Activo);

        if (plato == null)
        {
            return NotFound();
        }

        return plato;
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<ActionResult<IEnumerable<Plato>>> GetPlatosPorCategoria(uint categoriaId)
    {
        return await _context.Platos
            .Where(p => p.IdCategoria == categoriaId && p.Activo)
            .Include(p => p.IdCategoriaNavigation)
            .ToListAsync();
    }

    [HttpGet("menu-del-dia")]
    public async Task<ActionResult<IEnumerable<Plato>>> GetMenuDelDia()
    {
        return await _context.Platos
            .Where(p => p.EsMenuDelDia && p.Disponible && p.Activo)
            .Include(p => p.IdCategoriaNavigation)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Plato>> PostPlato(Plato plato)
    {
        plato.Activo = true;
        plato.Disponible = true;
        
        _context.Platos.Add(plato);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPlato", new { id = plato.IdPlato }, plato);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlato(uint id, Plato plato)
    {
        if (id != plato.IdPlato)
        {
            return BadRequest();
        }

        _context.Entry(plato).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlatoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlato(uint id)
    {
        var plato = await _context.Platos.FindAsync(id);
        if (plato == null)
        {
            return NotFound();
        }

        plato.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlatoExists(uint id)
    {
        return _context.Platos.Any(e => e.IdPlato == id && e.Activo);
    }
}