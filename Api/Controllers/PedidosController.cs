using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly RestauranteDisponibilidadContext _context;

        public PedidosController(RestauranteDisponibilidadContext context)
        {
            _context = context;
        }

        // ============================
        //      CREAR PEDIDO
        // ============================
        [HttpPost]
        public async Task<IActionResult> CrearPedido(PedidoCreateDTO dto)
        {
            var estadoPendiente = await _context.Estadopedido
                .FirstOrDefaultAsync(e => e.Nombre == "Pendiente");

            if (estadoPendiente == null)
                return BadRequest("No existe el estado 'Pendiente' en la base de datos.");

            var pedido = new Pedido
            {
                IdUsuario = dto.IdUsuario,
                IdReserva = dto.IdReserva,
                IdMetodoPago = dto.IdMetodoPago,
                IdEstadoPedido = estadoPendiente.IdEstadoPedido,
                EsPreOrden = dto.EsPreOrden,
                Comentarios = dto.Comentarios,
                FechaHoraPedido = DateTime.Now,
                Total = 0
            };

            foreach (var d in dto.Detalles)
            {
                var plato = await _context.Platos.FindAsync(d.IdPlato);
                if (plato == null)
                    return BadRequest($"No existe el plato con ID {d.IdPlato}");

                var detalle = new Detallepedido
                {
                    IdPlato = d.IdPlato,
                    Cantidad = d.Cantidad,
                    Comentarios = d.Comentarios,
                    PrecioUnitario = plato.Precio,
                    Subtotal = plato.Precio * d.Cantidad
                };

                pedido.Detallepedidos.Add(detalle);
                pedido.Total += detalle.Subtotal;
            }

            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Pedido creado correctamente", pedido.IdPedido });
        }

        // ============================
        //      VER PEDIDO POR ID
        // ============================
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> ObtenerPedido(uint id)
        {
            var pedido = await _context.Pedido
                .Include(p => p.Detallepedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound("Pedido no encontrado");

            var dto = new PedidoDTO
            {
                IdPedido = pedido.IdPedido,
                IdUsuario = pedido.IdUsuario,
                IdReserva = pedido.IdReserva,
                IdEstadoPedido = pedido.IdEstadoPedido,
                IdMetodoPago = pedido.IdMetodoPago,
                FechaHoraPedido = pedido.FechaHoraPedido,
                EsPreOrden = pedido.EsPreOrden,
                Total = pedido.Total,
                Comentarios = pedido.Comentarios,
                Detalles = pedido.Detallepedidos.Select(d => new PedidoDetalleDTO
                {
                    IdDetallePedido = d.IdDetallePedido,
                    IdPlato = d.IdPlato,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Comentarios = d.Comentarios
                }).ToList()
            };

            return Ok(dto);
        }

        // ============================
        //      ACTUALIZAR PEDIDO
        // ============================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPedido(uint id, PedidoCreateDTO dto)
        {
            var pedido = await _context.Pedido
                .Include(p => p.Detallepedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound("Pedido no encontrado");

            pedido.IdUsuario = dto.IdUsuario;
            pedido.IdReserva = dto.IdReserva;
            pedido.IdMetodoPago = dto.IdMetodoPago;
            pedido.EsPreOrden = dto.EsPreOrden;
            pedido.Comentarios = dto.Comentarios;

            pedido.Detallepedidos.Clear();
            pedido.Total = 0;

            foreach (var d in dto.Detalles)
            {
                var plato = await _context.Platos.FindAsync(d.IdPlato);
                if (plato == null)
                    return BadRequest($"No existe el plato con ID {d.IdPlato}");

                var detalle = new Detallepedido
                {
                    IdPlato = d.IdPlato,
                    Cantidad = d.Cantidad,
                    Comentarios = d.Comentarios,
                    PrecioUnitario = plato.Precio,
                    Subtotal = plato.Precio * d.Cantidad
                };

                pedido.Detallepedidos.Add(detalle);
                pedido.Total += detalle.Subtotal;
            }

            await _context.SaveChangesAsync();
            return Ok("Pedido actualizado correctamente");
        }

        // ============================
        //      ELIMINAR PEDIDO
        // ============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPedido(uint id)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Pedido no encontrado");

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();

            return Ok("Pedido eliminado correctamente");
        }
    }
}
