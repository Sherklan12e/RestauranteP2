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
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
{
    var usuarios = await _context.Usuarios
        .Where(u => u.Activo)
        .Select(u => new UsuarioDTO
        {
            IdUsuario = u.IdUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Telefono = u.Telefono,
            FechaRegistro = u.FechaRegistro
        })
        .ToListAsync();

    return Ok(usuarios);
}

[HttpGet("{id}")]
public async Task<ActionResult<UsuarioDTO>> GetUsuario(uint id)
{
    var usuario = await _context.Usuarios
        .Where(u => u.IdUsuario == id && u.Activo)
        .Select(u => new UsuarioDTO
        {
            IdUsuario = u.IdUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Telefono = u.Telefono,
            FechaRegistro = u.FechaRegistro
        })
        .FirstOrDefaultAsync();

    if (usuario == null)
    {
        return NotFound();
    }

    return Ok(usuario);
}

    [HttpPost]
public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioCreateDTO dto)
{
    var usuario = new Usuario
    {
        Nombre = dto.Nombre,
        Apellido = dto.Apellido,
        Email = dto.Email,
        Telefono = dto.Telefono,
        Contrasena = dto.Contrasena,
        FechaRegistro = DateTime.Now,
        Activo = true
    };

    _context.Usuarios.Add(usuario);
    await _context.SaveChangesAsync();

    var usuarioDto = new UsuarioDTO
    {
        IdUsuario = usuario.IdUsuario,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Email = usuario.Email,
        Telefono = usuario.Telefono,
        FechaRegistro = usuario.FechaRegistro
    };

    return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuarioDto);
}

    [HttpPut("{id}")]
public async Task<IActionResult> PutUsuario(uint id, UsuarioUpdateDTO dto)
{
    var usuario = await _context.Usuarios.FindAsync(id);

    if (usuario == null || !usuario.Activo)
    {
        return NotFound();
    }

    // Mapear solo los campos editables
    usuario.Nombre = dto.Nombre;
    usuario.Apellido = dto.Apellido;
    usuario.Email = dto.Email;
    usuario.Telefono = dto.Telefono;

    // Actualizar contraseña si viene en el DTO (opcional)
    if (!string.IsNullOrWhiteSpace(dto.Contrasena))
    {
        usuario.Contrasena = dto.Contrasena;
    }

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

    private bool UsuarioExists(uint id)
    {
        return _context.Usuarios.Any(e => e.IdUsuario == id);
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
}