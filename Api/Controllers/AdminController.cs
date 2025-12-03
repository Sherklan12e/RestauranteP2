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
            .ThenInclude(d => d.IdPlatoNavigation)
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
                CantidadPlatos = p.Detallepedidos.Count,
                Detalles = p.Detallepedidos.Select(d => new AdminPedidoDetalleDTO
                {
                    IdDetallePedido = d.IdDetallePedido,
                    IdPlato = d.IdPlato,
                    NombrePlato = d.IdPlatoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
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

        // Si se confirma la reserva, marcar la mesa como ocupada
        if (dto.NuevoEstado == "Confirmada" && reserva.IdMesa.HasValue)
        {
            var mesa = await _context.Mesas.FindAsync(reserva.IdMesa.Value);
            if (mesa != null)
            {
                mesa.Activa = false; // Marcar como ocupada (no disponible)
            }
        }

        // Si se cancela o completa la reserva, marcar la mesa como disponible nuevamente
        if ((dto.NuevoEstado == "Cancelada" || dto.NuevoEstado == "Completada") && reserva.IdMesa.HasValue)
        {
            var mesa = await _context.Mesas.FindAsync(reserva.IdMesa.Value);
            if (mesa != null)
            {
                mesa.Activa = true; // Marcar como disponible
            }
        }

        reserva.Estado = dto.NuevoEstado;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/admin/usuarios - Listar todos los usuarios con pedidos y reservas
    [HttpGet("usuarios")]
    public async Task<ActionResult<IEnumerable<object>>> GetTodosUsuarios([FromQuery] uint usuarioId)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

        var usuarios = await _context.Usuarios
            .Include(u => u.Pedidos)
            .Include(u => u.Reservas)
            .Select(u => new
            {
                u.IdUsuario,
                u.Nombre,
                u.Apellido,
                u.Email,
                u.Telefono,
                u.Rol,
                u.Activo,
                u.FechaRegistro,
                TotalPedidos = u.Pedidos.Count,
                TotalReservas = u.Reservas.Count,
                UltimoPedido = u.Pedidos.OrderByDescending(p => p.FechaHoraPedido).FirstOrDefault() != null 
                    ? u.Pedidos.OrderByDescending(p => p.FechaHoraPedido).FirstOrDefault().FechaHoraPedido 
                    : (DateTime?)null,
                UltimaReserva = u.Reservas.OrderByDescending(r => r.FechaHora).FirstOrDefault() != null 
                    ? u.Reservas.OrderByDescending(r => r.FechaHora).FirstOrDefault().FechaHora 
                    : (DateTime?)null
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
            .Include(m => m.Reservas)
            .Select(m => new
            {
                m.IdMesa,
                m.NumeroMesa,
                m.Capacidad,
                // Una mesa está activa (disponible) si:
                // 1. Su estado Activa es true Y
                // 2. No tiene reservas confirmadas
                Activa = m.Activa == true && !m.Reservas.Any(r => r.Estado == "Confirmada"),
                TieneReservaConfirmada = m.Reservas.Any(r => r.Estado == "Confirmada")
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

    // POST: api/admin/platos - Crear nuevo plato
    [HttpPost("platos")]
    public async Task<ActionResult<object>> CrearPlato([FromQuery] uint usuarioId, [FromBody] PlatoCreateDTO dto)
    {
        if (!IsAdmin(usuarioId))
            return Forbid();

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

        return CreatedAtAction(nameof(GetTodosPlatos), new { usuarioId }, new
        {
            plato.IdPlato,
            plato.Nombre,
            plato.Descripcion,
            plato.Precio,
            plato.Disponible,
            plato.Activo,
            Categoria = categoria?.Nombre
        });
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
