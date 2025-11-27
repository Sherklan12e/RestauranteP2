using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalificacionesController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public CalificacionesController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    // GET: api/calificaciones
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalificacionDTO>>> GetCalificaciones()
    {
        var calificaciones = await _context.Calificacions
            .Include(c => c.IdPlatoNavigation)
            .Include(c => c.IdUsuarioNavigation)
            .Select(c => new CalificacionDTO
            {
                IdCalificacion = c.IdCalificacion,
                IdPlato = c.IdPlato,
                IdUsuario = c.IdUsuario,
                IdPedido = c.IdPedido,
                Puntuacion = c.Puntuacion,
                Comentario = c.Comentario,
                FechaHora = c.FechaHora,
                NombrePlato = c.IdPlatoNavigation.Nombre,
                NombreUsuario = c.IdUsuarioNavigation.Nombre + " " + c.IdUsuarioNavigation.Apellido
            })
            .ToListAsync();

        return Ok(calificaciones);
    }

    // GET: api/calificaciones/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CalificacionDTO>> GetCalificacion(uint id)
    {
        var calificacion = await _context.Calificacions
            .Include(c => c.IdPlatoNavigation)
            .Include(c => c.IdUsuarioNavigation)
            .Where(c => c.IdCalificacion == id)
            .Select(c => new CalificacionDTO
            {
                IdCalificacion = c.IdCalificacion,
                IdPlato = c.IdPlato,
                IdUsuario = c.IdUsuario,
                IdPedido = c.IdPedido,
                Puntuacion = c.Puntuacion,
                Comentario = c.Comentario,
                FechaHora = c.FechaHora,
                NombrePlato = c.IdPlatoNavigation.Nombre,
                NombreUsuario = c.IdUsuarioNavigation.Nombre + " " + c.IdUsuarioNavigation.Apellido
            })
            .FirstOrDefaultAsync();

        if (calificacion == null)
        {
            return NotFound();
        }

        return Ok(calificacion);
    }

    // GET: api/calificaciones/plato/{platoId}
    [HttpGet("plato/{platoId}")]
    public async Task<ActionResult<IEnumerable<CalificacionDTO>>> GetCalificacionesPorPlato(uint platoId)
    {
        var calificaciones = await _context.Calificacions
            .Include(c => c.IdPlatoNavigation)
            .Include(c => c.IdUsuarioNavigation)
            .Where(c => c.IdPlato == platoId)
            .Select(c => new CalificacionDTO
            {
                IdCalificacion = c.IdCalificacion,
                IdPlato = c.IdPlato,
                IdUsuario = c.IdUsuario,
                IdPedido = c.IdPedido,
                Puntuacion = c.Puntuacion,
                Comentario = c.Comentario,
                FechaHora = c.FechaHora,
                NombrePlato = c.IdPlatoNavigation.Nombre,
                NombreUsuario = c.IdUsuarioNavigation.Nombre + " " + c.IdUsuarioNavigation.Apellido
            })
            .OrderByDescending(c => c.FechaHora)
            .ToListAsync();

        return Ok(calificaciones);
    }

    // GET: api/calificaciones/usuario/{usuarioId}
    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<CalificacionDTO>>> GetCalificacionesPorUsuario(uint usuarioId)
    {
        var calificaciones = await _context.Calificacions
            .Include(c => c.IdPlatoNavigation)
            .Include(c => c.IdUsuarioNavigation)
            .Where(c => c.IdUsuario == usuarioId)
            .Select(c => new CalificacionDTO
            {
                IdCalificacion = c.IdCalificacion,
                IdPlato = c.IdPlato,
                IdUsuario = c.IdUsuario,
                IdPedido = c.IdPedido,
                Puntuacion = c.Puntuacion,
                Comentario = c.Comentario,
                FechaHora = c.FechaHora,
                NombrePlato = c.IdPlatoNavigation.Nombre,
                NombreUsuario = c.IdUsuarioNavigation.Nombre + " " + c.IdUsuarioNavigation.Apellido
            })
            .OrderByDescending(c => c.FechaHora)
            .ToListAsync();

        return Ok(calificaciones);
    }

    // POST: api/calificaciones
    [HttpPost]
    public async Task<ActionResult<CalificacionDTO>> PostCalificacion(CalificacionCreateDTO dto)
    {
        // Validar que el plato existe
        var plato = await _context.Platos.FindAsync(dto.IdPlato);
        if (plato == null)
        {
            return BadRequest("El plato especificado no existe");
        }

        // Validar que el usuario existe
        var usuario = await _context.Usuarios.FindAsync(dto.IdUsuario);
        if (usuario == null)
        {
            return BadRequest("El usuario especificado no existe");
        }

        // Verificar si el usuario ya calificó este plato
        var existeCalificacion = await _context.Calificacions
            .AnyAsync(c => c.IdPlato == dto.IdPlato && c.IdUsuario == dto.IdUsuario);

        if (existeCalificacion)
        {
            return BadRequest("Ya has calificado este plato anteriormente");
        }

        // Crear la calificación
        var calificacion = new Calificacion
        {
            IdPlato = dto.IdPlato,
            IdUsuario = dto.IdUsuario,
            IdPedido = dto.IdPedido,
            Puntuacion = dto.Puntuacion,
            Comentario = dto.Comentario,
            FechaHora = DateTime.Now
        };

        _context.Calificacions.Add(calificacion);
        await _context.SaveChangesAsync();

        // Retornar la calificación creada
        var calificacionDTO = await GetCalificacion(calificacion.IdCalificacion);
        return calificacionDTO.Result;
    }

    // PUT: api/calificaciones/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCalificacion(uint id, CalificacionCreateDTO dto)
    {
        var calificacion = await _context.Calificacions.FindAsync(id);
        if (calificacion == null)
        {
            return NotFound();
        }

        calificacion.Puntuacion = dto.Puntuacion;
        calificacion.Comentario = dto.Comentario;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/calificaciones/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalificacion(uint id)
    {
        var calificacion = await _context.Calificacions.FindAsync(id);
        if (calificacion == null)
        {
            return NotFound();
        }

        _context.Calificacions.Remove(calificacion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
