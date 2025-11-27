using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaplatoController : ControllerBase
    {
        private readonly RestauranteDisponibilidadContext _context;

        public CategoriaplatoController(RestauranteDisponibilidadContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Categoriaplato
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaPlatoDto>>> GetCategorias()
        {
            var categorias = await _context.Categoriaplatos
                .Select(c => new CategoriaPlatoDto
                {
                    IdCategoria = c.IdCategoria,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // ✅ GET: api/Categoriaplato/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaPlatoDto>> GetCategoria(uint id)
        {
            var categoria = await _context.Categoriaplatos
                .Where(c => c.IdCategoria == id)
                .Select(c => new CategoriaPlatoDto
                {
                    IdCategoria = c.IdCategoria,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        // ✅ POST: api/Categoriaplato
        [HttpPost]
        public async Task<ActionResult<CategoriaPlatoDto>> CreateCategoria(CategoriaCreateDTO dto)
        {
            var categoria = new Categoriaplato
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.Categoriaplatos.Add(categoria);
            await _context.SaveChangesAsync();

            var categoriaDto = new CategoriaPlatoDto
            {
                IdCategoria = categoria.IdCategoria,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.IdCategoria }, categoriaDto);
        }

        // ✅ PUT: api/Categoriaplato/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(uint id, CategoriaCreateDTO dto)
        {
            var categoria = await _context.Categoriaplatos.FindAsync(id);
            if (categoria == null)
                return NotFound();

            categoria.Nombre = dto.Nombre;
            categoria.Descripcion = dto.Descripcion;

            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/Categoriaplato/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(uint id)
        {
            var categoria = await _context.Categoriaplatos.FindAsync(id);
            if (categoria == null)
                return NotFound();

            _context.Categoriaplatos.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
