using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MesasController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public MesasController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    // GET: api/mesas/disponibles/lista - Mesas disponibles para reservar
    [HttpGet("disponibles/lista")]
    public async Task<ActionResult<IEnumerable<MesaDTO>>> GetMesasDisponibles()
    {
        var mesas = await _context.Mesas
            .Include(m => m.Reservas)
            .Where(m => m.Activa == true && !m.Reservas.Any(r => r.Estado == "Confirmada"))
            .Select(m => new MesaDTO
            {
                IdMesa = m.IdMesa,
                NumeroMesa = m.NumeroMesa,
                Capacidad = m.Capacidad,
                Activa = true
            })
            .OrderBy(m => m.NumeroMesa)
            .ToListAsync();

        return Ok(mesas);
    }

    // GET: api/mesas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MesaDTO>>> GetMesas()
    {
        var mesas = await _context.Mesas
            .Select(m => new MesaDTO
            {
                IdMesa = m.IdMesa,
                NumeroMesa = m.NumeroMesa,
                Capacidad = m.Capacidad,
                Activa = m.Activa
            })
            .ToListAsync();

        return Ok(mesas);
    }

    // GET: api/mesas/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MesaDTO>> GetMesa(uint id)
    {
        var mesa = await _context.Mesas.FindAsync(id);

        if (mesa == null)
        {
            return NotFound();
        }

        return new MesaDTO
        {
            IdMesa = mesa.IdMesa,
            NumeroMesa = mesa.NumeroMesa,
            Capacidad = mesa.Capacidad,
            Activa = mesa.Activa
        };
    }

    // POST: api/mesas
    [HttpPost]
    public async Task<ActionResult<MesaDTO>> PostMesa(MesaCreateDTO dto)
    {
        var mesa = new Mesa
        {
            NumeroMesa = dto.NumeroMesa,
            Capacidad = dto.Capacidad,
            Activa = dto.Activa
        };

        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();

        var mesaDTO = new MesaDTO
        {
            IdMesa = mesa.IdMesa,
            NumeroMesa = mesa.NumeroMesa,
            Capacidad = mesa.Capacidad,
            Activa = mesa.Activa
        };

        return CreatedAtAction(nameof(GetMesa), new { id = mesa.IdMesa }, mesaDTO);
    }

    // PUT: api/mesas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMesa(uint id, MesaCreateDTO dto)
    {
        var mesa = await _context.Mesas.FindAsync(id);
        if (mesa == null)
        {
            return NotFound();
        }

        mesa.NumeroMesa = dto.NumeroMesa;
        mesa.Capacidad = dto.Capacidad;
        mesa.Activa = dto.Activa;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/mesas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMesa(uint id)
    {
        var mesa = await _context.Mesas.FindAsync(id);
        if (mesa == null)
        {
            return NotFound();
        }

        _context.Mesas.Remove(mesa);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
