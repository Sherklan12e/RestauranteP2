using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public PedidosController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    // GET: api/pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidos()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.IdMetodoPagoNavigation)
            .Include(p => p.Detallepedidos)
                .ThenInclude(d => d.IdPlatoNavigation)
            .Select(p => new PedidoDTO
            {
                IdPedido = p.IdPedido,
                IdUsuario = p.IdUsuario,
                NombreUsuario = p.IdUsuarioNavigation.Nombre + " " + p.IdUsuarioNavigation.Apellido,
                IdReserva = p.IdReserva,
                IdEstadoPedido = p.IdEstadoPedido,
                EstadoNombre = p.IdEstadoPedidoNavigation.Nombre,
                IdMetodoPago = p.IdMetodoPago,
                MetodoPagoNombre = p.IdMetodoPagoNavigation != null ? p.IdMetodoPagoNavigation.TipoMedioPago : null,
                FechaHoraPedido = p.FechaHoraPedido,
                FechaHoraEntregaEstimada = p.FechaHoraEntregaEstimada,
                FechaHoraEntregaReal = p.FechaHoraEntregaReal,
                EsPreOrden = p.EsPreOrden,
                Total = p.Total,
                Comentarios = p.Comentarios,
                Detalles = p.Detallepedidos.Select(d => new DetallePedidoDTO
                {
                    IdDetallePedido = d.IdDetallePedido,
                    IdPlato = d.IdPlato,
                    PlatoNombre = d.IdPlatoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Comentarios = d.Comentarios
                }).ToList()
            })
            .ToListAsync();

        return Ok(pedidos);
    }

    // GET: api/pedidos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDTO>> GetPedido(uint id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.IdMetodoPagoNavigation)
            .Include(p => p.Detallepedidos)
                .ThenInclude(d => d.IdPlatoNavigation)
            .Where(p => p.IdPedido == id)
            .Select(p => new PedidoDTO
            {
                IdPedido = p.IdPedido,
                IdUsuario = p.IdUsuario,
                NombreUsuario = p.IdUsuarioNavigation.Nombre + " " + p.IdUsuarioNavigation.Apellido,
                IdReserva = p.IdReserva,
                IdEstadoPedido = p.IdEstadoPedido,
                EstadoNombre = p.IdEstadoPedidoNavigation.Nombre,
                IdMetodoPago = p.IdMetodoPago,
                MetodoPagoNombre = p.IdMetodoPagoNavigation != null ? p.IdMetodoPagoNavigation.TipoMedioPago : null,
                FechaHoraPedido = p.FechaHoraPedido,
                FechaHoraEntregaEstimada = p.FechaHoraEntregaEstimada,
                FechaHoraEntregaReal = p.FechaHoraEntregaReal,
                EsPreOrden = p.EsPreOrden,
                Total = p.Total,
                Comentarios = p.Comentarios,
                Detalles = p.Detallepedidos.Select(d => new DetallePedidoDTO
                {
                    IdDetallePedido = d.IdDetallePedido,
                    IdPlato = d.IdPlato,
                    PlatoNombre = d.IdPlatoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Comentarios = d.Comentarios
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (pedido == null)
        {
            return NotFound();
        }

        return Ok(pedido);
    }

    // GET: api/pedidos/usuario/{usuarioId}
    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidosPorUsuario(uint usuarioId)
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.IdMetodoPagoNavigation)
            .Include(p => p.Detallepedidos)
                .ThenInclude(d => d.IdPlatoNavigation)
            .Where(p => p.IdUsuario == usuarioId)
            .Select(p => new PedidoDTO
            {
                IdPedido = p.IdPedido,
                IdUsuario = p.IdUsuario,
                NombreUsuario = p.IdUsuarioNavigation.Nombre + " " + p.IdUsuarioNavigation.Apellido,
                IdReserva = p.IdReserva,
                IdEstadoPedido = p.IdEstadoPedido,
                EstadoNombre = p.IdEstadoPedidoNavigation.Nombre,
                IdMetodoPago = p.IdMetodoPago,
                MetodoPagoNombre = p.IdMetodoPagoNavigation != null ? p.IdMetodoPagoNavigation.TipoMedioPago : null,
                FechaHoraPedido = p.FechaHoraPedido,
                FechaHoraEntregaEstimada = p.FechaHoraEntregaEstimada,
                FechaHoraEntregaReal = p.FechaHoraEntregaReal,
                EsPreOrden = p.EsPreOrden,
                Total = p.Total,
                Comentarios = p.Comentarios,
                Detalles = p.Detallepedidos.Select(d => new DetallePedidoDTO
                {
                    IdDetallePedido = d.IdDetallePedido,
                    IdPlato = d.IdPlato,
                    PlatoNombre = d.IdPlatoNavigation.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Comentarios = d.Comentarios
                }).ToList()
            })
            .OrderByDescending(p => p.FechaHoraPedido)
            .ToListAsync();

        return Ok(pedidos);
    }

    // POST: api/pedidos
    [HttpPost]
    public async Task<ActionResult<PedidoDTO>> PostPedido(PedidoCreateDTO dto)
    {
        // Obtener el estado "Pendiente"
        var estadoPendiente = await _context.Estadopedidos
            .FirstOrDefaultAsync(e => e.Nombre == "Pendiente");

        if (estadoPendiente == null)
        {
            return BadRequest("No se encontró el estado 'Pendiente' en la base de datos");
        }

        // Validar que los platos existen y están disponibles
        foreach (var detalle in dto.Detalles)
        {
            var plato = await _context.Platos.FindAsync(detalle.IdPlato);
            if (plato == null || !plato.Activo || !plato.Disponible)
            {
                return BadRequest($"El plato con ID {detalle.IdPlato} no está disponible");
            }
        }

        // Crear el pedido
        var pedido = new Pedido
        {
            IdUsuario = dto.IdUsuario,
            IdReserva = dto.IdReserva,
            IdEstadoPedido = estadoPendiente.IdEstadoPedido,
            IdMetodoPago = dto.IdMetodoPago,
            FechaHoraPedido = DateTime.Now,
            EsPreOrden = dto.EsPreOrden,
            Comentarios = dto.Comentarios,
            Total = 0
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Crear los detalles del pedido
        decimal totalPedido = 0;
        foreach (var detalleDto in dto.Detalles)
        {
            var plato = await _context.Platos.FindAsync(detalleDto.IdPlato);
            if (plato == null) continue;

            var subtotal = plato.Precio * detalleDto.Cantidad;
            totalPedido += subtotal;

            var detalle = new Detallepedido
            {
                IdPedido = pedido.IdPedido,
                IdPlato = detalleDto.IdPlato,
                Cantidad = detalleDto.Cantidad,
                PrecioUnitario = plato.Precio,
                Subtotal = subtotal,
                Comentarios = detalleDto.Comentarios
            };

            _context.Detallepedidos.Add(detalle);
        }

        // Actualizar el total del pedido
        pedido.Total = totalPedido;
        await _context.SaveChangesAsync();

        // Retornar el pedido creado
        var pedidoDTO = await GetPedido(pedido.IdPedido);
        return pedidoDTO.Result;
    }

    // PUT: api/pedidos/{id}/estado
    [HttpPut("{id}/estado")]
    public async Task<IActionResult> UpdateEstadoPedido(uint id, [FromBody] string estadoNombre)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        var estado = await _context.Estadopedidos
            .FirstOrDefaultAsync(e => e.Nombre == estadoNombre);

        if (estado == null)
        {
            return BadRequest($"El estado '{estadoNombre}' no existe");
        }

        pedido.IdEstadoPedido = estado.IdEstadoPedido;

        // Si el pedido está listo, actualizar fecha de entrega real
        if (estadoNombre == "Listo" || estadoNombre == "Entregado")
        {
            pedido.FechaHoraEntregaReal = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pedidos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(uint id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Detallepedidos)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

        if (pedido == null)
        {
            return NotFound();
        }

        // Eliminar detalles primero
        _context.Detallepedidos.RemoveRange(pedido.Detallepedidos);
        
        // Eliminar pedido
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
