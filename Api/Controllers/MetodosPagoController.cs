using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetodosPagoController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public MetodosPagoController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    // GET: api/metodospago
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetMetodosPago()
    {
        var metodos = await _context.Metodopago
            .Where(m => m.Activo == true)
            .Select(m => new
            {
                idMetodoPago = m.IdMetodoPago,
                tipoMedioPago = m.TipoMedioPago,
                activo = m.Activo
            })
            .ToListAsync();

        return Ok(metodos);
    }

    // GET: api/metodospago/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMetodoPago(uint id)
    {
        var metodo = await _context.Metodopago
            .Where(m => m.IdMetodoPago == id)
            .Select(m => new
            {
                idMetodoPago = m.IdMetodoPago,
                tipoMedioPago = m.TipoMedioPago,
                activo = m.Activo
            })
            .FirstOrDefaultAsync();

        if (metodo == null)
        {
            return NotFound();
        }

        return Ok(metodo);
    }
}
