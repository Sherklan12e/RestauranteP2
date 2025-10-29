using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
namespace Api;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly RestauranteDisponibilidadContext _context;

    public UsuariosController(RestauranteDisponibilidadContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.Where(u => u.Activo).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(uint id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null || !usuario.Activo)
        {
            return NotFound();
        }

        return usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        usuario.FechaRegistro = DateTime.Now;
        usuario.Activo = true;
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(uint id, Usuario usuario)
    {
        if (id != usuario.IdUsuario)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(uint id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        usuario.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private bool UsuarioExists(uint id)
    {
        return _context.Usuarios.Any(e => e.IdUsuario == id && e.Activo);
    }
}