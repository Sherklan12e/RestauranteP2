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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
    {
        return await _context.Pedidos
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.Detallepedidos)
                .ThenInclude(d => d.IdPlatoNavigation)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> GetPedido(uint id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.IdUsuarioNavigation)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.Detallepedidos)
                .ThenInclude(d => d.IdPlatoNavigation)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

        if (pedido == null)
        {
            return NotFound();
        }

        return pedido;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosPorUsuario(uint usuarioId)
    {
        return await _context.Pedidos
            .Where(p => p.IdUsuario == usuarioId)
            .Include(p => p.IdEstadoPedidoNavigation)
            .Include(p => p.Detallepedidos)
                .ThenInclude(d => d.IdPlatoNavigation)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
    {
        pedido.FechaHoraPedido = DateTime.Now;
        
        // Calcular total si no viene calculado
        if (pedido.Total == 0 && pedido.Detallepedidos != null)
        {
            pedido.Total = pedido.Detallepedidos.Sum(d => d.Subtotal);
        }
        
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> UpdateEstadoPedido(uint id, [FromBody] uint estadoId)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        pedido.IdEstadoPedido = estadoId;
        
        if (estadoId == 4) // Suponiendo que 4 es "Entregado"
        {
            pedido.FechaHoraEntregaReal = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/detalles")]
    public async Task<ActionResult<Detallepedido>> AddDetallePedido(uint id, Detallepedido detalle)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        detalle.IdPedido = id;
        _context.Detallepedidos.Add(detalle);
        
        // Actualizar total del pedido
        pedido.Total += detalle.Subtotal;
        
        await _context.SaveChangesAsync();

        return Ok(detalle);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(uint id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        // Eliminar detalles primero
        var detalles = await _context.Detallepedidos.Where(d => d.IdPedido == id).ToListAsync();
        _context.Detallepedidos.RemoveRange(detalles);
        
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}