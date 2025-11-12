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

    // GET: api/platos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatoDTO>>> GetPlatos()
    {
        var platos = await _context.Platos
            .Include(p => p.IdCategoriaNavigation)
            .Where(p => p.Activo)
            .Select(p => new PlatoDTO
            {
                IdPlato = p.IdPlato,
                IdCategoria = p.IdCategoria,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                TiempoPreparacion = p.TiempoPreparacion,
                ImagenUrl = p.ImagenUrl,
                Disponible = p.Disponible,
                EsMenuDelDia = p.EsMenuDelDia,
                Activo = p.Activo,
                CategoriaNombre = p.IdCategoriaNavigation.Nombre
            })
            .ToListAsync();

        return Ok(platos);
    }

    // GET: api/platos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PlatoDTO>> GetPlato(uint id)
    {
        var plato = await _context.Platos
            .Include(p => p.IdCategoriaNavigation)
            .Where(p => p.IdPlato == id && p.Activo)
            .Select(p => new PlatoDTO
            {
                IdPlato = p.IdPlato,
                IdCategoria = p.IdCategoria,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                TiempoPreparacion = p.TiempoPreparacion,
                ImagenUrl = p.ImagenUrl,
                Disponible = p.Disponible,
                EsMenuDelDia = p.EsMenuDelDia,
                Activo = p.Activo,
                CategoriaNombre = p.IdCategoriaNavigation.Nombre
            })
            .FirstOrDefaultAsync();

        if (plato == null)
            return NotFound();

        return Ok(plato);
    }

    // POST: api/platos
    [HttpPost]
    public async Task<ActionResult<PlatoDTO>> PostPlato(PlatoCreateDTO dto)
    {
        var plato = new Plato
        {
            IdCategoria = dto.IdCategoria,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            TiempoPreparacion = dto.TiempoPreparacion,
            ImagenUrl = dto.ImagenUrl,
            Disponible = dto.Disponible,
            EsMenuDelDia = dto.EsMenuDelDia,
            Activo = true
        };

        _context.Platos.Add(plato);
        await _context.SaveChangesAsync();

        var categoria = await _context.Categoriaplatos.FindAsync(dto.IdCategoria);

        var platoDTO = new PlatoDTO
        {
            IdPlato = plato.IdPlato,
            IdCategoria = plato.IdCategoria,
            Nombre = plato.Nombre,
            Descripcion = plato.Descripcion,
            Precio = plato.Precio,
            TiempoPreparacion = plato.TiempoPreparacion,
            ImagenUrl = plato.ImagenUrl,
            Disponible = plato.Disponible,
            EsMenuDelDia = plato.EsMenuDelDia,
            Activo = plato.Activo,
            CategoriaNombre = categoria?.Nombre
        };

        return CreatedAtAction(nameof(GetPlato), new { id = plato.IdPlato }, platoDTO);
    }

    // PUT: api/platos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlato(uint id, PlatoUpdateDTO dto)
    {
        var plato = await _context.Platos.FindAsync(id);

        if (plato == null || !plato.Activo)
            return NotFound();

        plato.Nombre = dto.Nombre;
        plato.Descripcion = dto.Descripcion;
        plato.Precio = dto.Precio;
        plato.TiempoPreparacion = dto.TiempoPreparacion;
        plato.ImagenUrl = dto.ImagenUrl;
        plato.Disponible = dto.Disponible;
        plato.EsMenuDelDia = dto.EsMenuDelDia;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE lógico: api/platos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlato(uint id)
    {
        var plato = await _context.Platos.FindAsync(id);

        if (plato == null)
            return NotFound();

        plato.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
