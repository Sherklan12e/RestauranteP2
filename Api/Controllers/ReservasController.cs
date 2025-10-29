using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public ReservasController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
    {
        return await _context.Reservas
            .Include(r => r.IdUsuarioNavigation)
            .Include(r => r.IdMesaNavigation)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reserva>> GetReserva(uint id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.IdUsuarioNavigation)
            .Include(r => r.IdMesaNavigation)
            .FirstOrDefaultAsync(r => r.IdReserva == id);

        if (reserva == null)
        {
            return NotFound();
        }

        return reserva;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasPorUsuario(uint usuarioId)
    {
        return await _context.Reservas
            .Where(r => r.IdUsuario == usuarioId)
            .Include(r => r.IdMesaNavigation)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
    {
        reserva.FechaCreacion = DateTime.Now;
        reserva.Estado = "Pendiente";
        
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReserva", new { id = reserva.IdReserva }, reserva);
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> UpdateEstadoReserva(uint id, [FromBody] string estado)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        reserva.Estado = estado;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReserva(uint id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}