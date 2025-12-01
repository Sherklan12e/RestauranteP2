using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoPedidoController : ControllerBase
    {
        private readonly RestauranteDisponibilidadContext _context;

        public EstadoPedidoController(RestauranteDisponibilidadContext context)
        {
            _context = context;
        }

        // GET: api/EstadoPedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoPedidoDTO>>> GetAll()
        {
            var estados = await _context.Estadopedido
                .Select(e => new EstadoPedidoDTO
                {
                    IdEstadoPedido = e.IdEstadoPedido,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                })
                .ToListAsync();

            return Ok(estados);
        }

        // GET: api/EstadoPedido/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoPedidoDTO>> GetById(uint id)
        {
            var estado = await _context.Estadopedido
                .Where(e => e.IdEstadoPedido == id)
                .Select(e => new EstadoPedidoDTO
                {
                    IdEstadoPedido = e.IdEstadoPedido,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                })
                .FirstOrDefaultAsync();

            if (estado == null)
                return NotFound("EstadoPedido no encontrado");

            return Ok(estado);
        }

        // POST: api/EstadoPedido
        [HttpPost]
        public async Task<ActionResult> Create(EstadoPedidoCreateDTO dto)
        {
            var nuevo = new Estadopedido
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.Estadopedido.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "EstadoPedido creado", id = nuevo.IdEstadoPedido });
        }

        // PUT: api/EstadoPedido/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(uint id, EstadoPedidoUpdateDTO dto)
        {
            var estado = await _context.Estadopedido.FindAsync(id);

            if (estado == null)
                return NotFound("EstadoPedido no encontrado");

            estado.Nombre = dto.Nombre;
            estado.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();

            return Ok("EstadoPedido actualizado");
        }

        // DELETE: api/EstadoPedido/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(uint id)
        {
            var estado = await _context.Estadopedido.FindAsync(id);

            if (estado == null)
                return NotFound("EstadoPedido no encontrado");

            _context.Estadopedido.Remove(estado);
            await _context.SaveChangesAsync();

            return Ok("EstadoPedido eliminado");
        }
    }
}