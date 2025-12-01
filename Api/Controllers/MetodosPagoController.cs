using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
namespace Api.Controllers;

[Route("api/[controller]")]

[ApiController]
public class MetodoPagoController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public MetodoPagoController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    // GET: api/MetodoPago
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MetodoPagoReadDTO>>> GetMetodosPago()
    {
        return await _context.Set<Metodopago>()
            .Select(m => new MetodoPagoReadDTO
            {
                IdMetodoPago = m.IdMetodoPago,
                TipoMedioPago = m.TipoMedioPago,
                Activo = m.Activo
            })
            .ToListAsync();
    }

    // GET: api/MetodoPago/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MetodoPagoReadDTO>> GetMetodoPago(uint id)
    {
        var metodo = await _context.Set<Metodopago>().FindAsync(id);
        if (metodo == null)
            return NotFound();

        return new MetodoPagoReadDTO
        {
            IdMetodoPago = metodo.IdMetodoPago,
            TipoMedioPago = metodo.TipoMedioPago,
            Activo = metodo.Activo
        };
    }

    // POST: api/MetodoPago
    [HttpPost]
    public async Task<ActionResult> CreateMetodoPago(MetodoPagoCreateDTO dto)
    {
        var nuevo = new Metodopago
        {
            TipoMedioPago = dto.TipoMedioPago,
            Activo = dto.Activo
        };

        _context.Set<Metodopago>().Add(nuevo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMetodoPago), new { id = nuevo.IdMetodoPago }, dto);
    }

    // PUT: api/MetodoPago/5
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMetodoPago(uint id, MetodoPagoUpdateDTO dto)
    {
        var metodo = await _context.Set<Metodopago>().FindAsync(id);

        if (metodo == null)
            return NotFound();

        metodo.TipoMedioPago = dto.TipoMedioPago;
        metodo.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/MetodoPago/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMetodoPago(uint id)
    {
        var metodo = await _context.Set<Metodopago>().FindAsync(id);

        if (metodo == null)
            return NotFound();

        _context.Remove(metodo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}