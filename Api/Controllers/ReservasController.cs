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
    public async Task<ActionResult<IEnumerable<ReservaDTO>>> GetReservas()
    {
        var reservas = await _context.Reservas
            .Include(r => r.IdUsuarioNavigation)
            .Include(r => r.IdMesaNavigation)
            .Select(r => new ReservaDTO
            {
                IdReserva = r.IdReserva,
                IdUsuario = r.IdUsuario,
                NombreUsuario = r.IdUsuarioNavigation.Nombre + " " + r.IdUsuarioNavigation.Apellido,
                IdMesa = r.IdMesa,
                NumeroMesa = r.IdMesaNavigation != null ? r.IdMesaNavigation.NumeroMesa : null,
                FechaHora = r.FechaHora,
                CantidadPersonas = r.CantidadPersonas,
                Estado = r.Estado,
                Comentarios = r.Comentarios,
                FechaCreacion = r.FechaCreacion
            })
            .ToListAsync();

        return Ok(reservas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservaDTO>> GetReserva(uint id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.IdUsuarioNavigation)
            .Include(r => r.IdMesaNavigation)
            .Where(r => r.IdReserva == id)
            .Select(r => new ReservaDTO
            {
                IdReserva = r.IdReserva,
                IdUsuario = r.IdUsuario,
                NombreUsuario = r.IdUsuarioNavigation.Nombre + " " + r.IdUsuarioNavigation.Apellido,
                IdMesa = r.IdMesa,
                NumeroMesa = r.IdMesaNavigation != null ? r.IdMesaNavigation.NumeroMesa : null,
                FechaHora = r.FechaHora,
                CantidadPersonas = r.CantidadPersonas,
                Estado = r.Estado,
                Comentarios = r.Comentarios,
                FechaCreacion = r.FechaCreacion
            })
            .FirstOrDefaultAsync();

        if (reserva == null)
        {
            return NotFound();
        }

        return Ok(reserva);
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<ReservaDTO>>> GetReservasPorUsuario(uint usuarioId)
    {
        var reservas = await _context.Reservas
            .Where(r => r.IdUsuario == usuarioId)
            .Include(r => r.IdMesaNavigation)
            .Include(r => r.IdUsuarioNavigation)
            .Select(r => new ReservaDTO
            {
                IdReserva = r.IdReserva,
                IdUsuario = r.IdUsuario,
                NombreUsuario = r.IdUsuarioNavigation.Nombre + " " + r.IdUsuarioNavigation.Apellido,
                IdMesa = r.IdMesa,
                NumeroMesa = r.IdMesaNavigation != null ? r.IdMesaNavigation.NumeroMesa : null,
                FechaHora = r.FechaHora,
                CantidadPersonas = r.CantidadPersonas,
                Estado = r.Estado,
                Comentarios = r.Comentarios,
                FechaCreacion = r.FechaCreacion
            })
            .ToListAsync();

        return Ok(reservas);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaDTO>> PostReserva(ReservaCreateDTO dto)
    {
        // Validar que la mesa existe
        if (dto.IdMesa.HasValue)
        {
            var mesa = await _context.Mesas.FindAsync(dto.IdMesa.Value);
            if (mesa == null)
                return BadRequest("La mesa seleccionada no existe");

            // Validar que la cantidad de personas no exceda la capacidad de la mesa
            if (dto.CantidadPersonas > mesa.Capacidad)
                return BadRequest($"La mesa tiene capacidad para {mesa.Capacidad} personas, pero solicitaste {dto.CantidadPersonas}");
        }

        var reserva = new Reserva
        {
            IdUsuario = dto.IdUsuario,
            IdMesa = dto.IdMesa,
            FechaHora = dto.FechaHora,
            CantidadPersonas = dto.CantidadPersonas,
            Comentarios = dto.Comentarios,
            Estado = "Pendiente",
            FechaCreacion = DateTime.Now
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        var reservaDTO = new ReservaDTO
        {
            IdReserva = reserva.IdReserva,
            IdUsuario = reserva.IdUsuario,
            IdMesa = reserva.IdMesa,
            FechaHora = reserva.FechaHora,
            CantidadPersonas = reserva.CantidadPersonas,
            Estado = reserva.Estado,
            Comentarios = reserva.Comentarios,
            FechaCreacion = reserva.FechaCreacion
        };

        return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reservaDTO);
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