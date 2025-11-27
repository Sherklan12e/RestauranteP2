using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.DTOs;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public AdminController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    // Verificar si un usuario es admin
    private bool IsAdmin(uint usuarioId)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);
        return usuario?.Rol == "admin";
    }

    // GET: api/admin/pedidos - Listar todos los pedidos
    [HttpGet("pedidos")]
    public async Task<ActionResult<IEnumerable<AdminPedidoDTO>>> GetTodosPedidos([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var pedidos = await _context.Pedido
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.Detallepedidos)
            .Select(p => new AdminPedidoDTO
            {
                IdPedido = p.IdPedido,
                IdUsuario = p.IdUsuario,
                NombreUsuario = p.IdUsuarioNavigation.Nombre + " " + p.IdUsuarioNavigation.Apellido,
                EmailUsuario = p.IdUsuarioNavigation.Email,
                FechaHoraPedido = p.FechaHoraPedido,
                Total = p.Total,
                Estado = p.IdEstadoPedidoNavigation.Nombre,
                Comentarios = p.Comentarios,
                CantidadPlatos = p.Detallepedidos.Count
            })
            .OrderByDescending(p => p.FechaHoraPedido)
            .ToListAsync();

        return Ok(pedidos);
    }

    // PUT: api/admin/pedidos/{id}/estado - Actualizar estado de pedido
    [HttpPut("pedidos/{id}/estado")]
    public async Task<IActionResult> ActualizarEstadoPedido(uint id, [FromQuery] uint usuarioId, [FromBody] ActualizarEstadoDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var pedido = await _context.Pedido.FindAsync(id);
        if (pedido == null)
            return NotFound();

        var estado = await _context.Estadopedido.FirstOrDefaultAsync(e => e.Nombre == dto.NuevoEstado);
        if (estado == null)
            return BadRequest("Estado inválido");

        pedido.IdEstadoPedido = estado.IdEstadoPedido;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/admin/reservas - Listar todas las reservas
    [HttpGet("reservas")]
    public async Task<ActionResult<IEnumerable<AdminReservaDTO>>> GetTodasReservas([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var reservas = await _context.Reservas
            .Include(r => r.IdUsuarioNavigation)
            .Include(r => r.IdMesaNavigation)
            .Select(r => new AdminReservaDTO
            {
                IdReserva = r.IdReserva,
                IdUsuario = r.IdUsuario,
                NombreUsuario = r.IdUsuarioNavigation.Nombre + " " + r.IdUsuarioNavigation.Apellido,
                EmailUsuario = r.IdUsuarioNavigation.Email,
                NumeroMesa = r.IdMesaNavigation != null ? r.IdMesaNavigation.NumeroMesa : "Sin mesa",
                FechaHora = r.FechaHora,
                CantidadPersonas = r.CantidadPersonas,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion
            })
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();

        return Ok(reservas);
    }

    // PUT: api/admin/reservas/{id}/estado - Actualizar estado de reserva
    [HttpPut("reservas/{id}/estado")]
    public async Task<IActionResult> ActualizarEstadoReserva(uint id, [FromQuery] uint usuarioId, [FromBody] ActualizarEstadoDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
            return NotFound();

        reserva.Estado = dto.NuevoEstado;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/admin/usuarios - Listar todos los usuarios
    [HttpGet("usuarios")]
    public async Task<ActionResult<IEnumerable<AdminUsuarioDTO>>> GetTodosUsuarios([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var usuarios = await _context.Usuarios
            .Select(u => new AdminUsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Telefono = u.Telefono,
                Rol = u.Rol,
                Activo = u.Activo,
                FechaRegistro = u.FechaRegistro
            })
            .OrderByDescending(u => u.FechaRegistro)
            .ToListAsync();

        return Ok(usuarios);
    }

    // PUT: api/admin/usuarios/{id}/rol - Cambiar rol de usuario
    [HttpPut("usuarios/{id}/rol")]
    public async Task<IActionResult> CambiarRolUsuario(uint id, [FromQuery] uint usuarioId, [FromBody] CambiarRolDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();

        if (dto.NuevoRol != "admin" && dto.NuevoRol != "cliente")
            return BadRequest("Rol inválido. Debe ser 'admin' o 'cliente'");

        usuario.Rol = dto.NuevoRol;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/admin/usuarios/{id}/estado - Activar/desactivar usuario
    [HttpPut("usuarios/{id}/estado")]
    public async Task<IActionResult> CambiarEstadoUsuario(uint id, [FromQuery] uint usuarioId, [FromBody] CambiarEstadoDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();

        usuario.Activo = dto.Activo;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/admin/estadisticas - Obtener estadísticas generales
    [HttpGet("estadisticas")]
    public async Task<ActionResult<EstadisticasDTO>> GetEstadisticas([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var totalPedidos = await _context.Pedido.CountAsync();
        var totalReservas = await _context.Reservas.CountAsync();
        var totalUsuarios = await _context.Usuarios.CountAsync();
        var totalPlatos = await _context.Platos.CountAsync();

        var ingresosTotales = await _context.Pedido.SumAsync(p => p.Total);

        var estadisticas = new EstadisticasDTO
        {
            TotalPedidos = totalPedidos,
            TotalReservas = totalReservas,
            TotalUsuarios = totalUsuarios,
            TotalPlatos = totalPlatos,
            IngresosTotales = ingresosTotales
        };

        return Ok(estadisticas);
    }

    // GET: api/admin/mesas - Listar todas las mesas
    [HttpGet("mesas")]
    public async Task<ActionResult<IEnumerable<object>>> GetTodasMesas([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var mesas = await _context.Mesas
            .Select(m => new
            {
                m.IdMesa,
                m.NumeroMesa,
                m.Capacidad,
                m.Activa
            })
            .OrderBy(m => m.NumeroMesa)
            .ToListAsync();

        return Ok(mesas);
    }

    // PUT: api/admin/mesas/{id}/estado - Activar/desactivar mesa
    [HttpPut("mesas/{id}/estado")]
    public async Task<IActionResult> CambiarEstadoMesa(uint id, [FromQuery] uint usuarioId, [FromBody] CambiarEstadoDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var mesa = await _context.Mesas.FindAsync(id);
        if (mesa == null)
            return NotFound();

        mesa.Activa = dto.Activo;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/admin/platos - Listar todos los platos
    [HttpGet("platos")]
    public async Task<ActionResult<IEnumerable<object>>> GetTodosPlatos([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var platos = await _context.Platos
            .Include(p => p.IdCategoriaNavigation)
            .Select(p => new
            {
                p.IdPlato,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                p.Disponible,
                p.Activo,
                Categoria = p.IdCategoriaNavigation.Nombre
            })
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return Ok(platos);
    }

    // PUT: api/admin/platos/{id}/disponibilidad - Cambiar disponibilidad de plato
    [HttpPut("platos/{id}/disponibilidad")]
    public async Task<IActionResult> CambiarDisponibilidadPlato(uint id, [FromQuery] uint usuarioId, [FromBody] CambiarEstadoDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var plato = await _context.Platos.FindAsync(id);
        if (plato == null)
            return NotFound();

        plato.Disponible = dto.Activo;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/admin/platos/{id} - Eliminar (desactivar) plato
    [HttpDelete("platos/{id}")]
    public async Task<IActionResult> EliminarPlato(uint id, [FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var plato = await _context.Platos.FindAsync(id);
        if (plato == null)
            return NotFound();

        plato.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
