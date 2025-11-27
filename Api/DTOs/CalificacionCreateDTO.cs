using System.ComponentModel.DataAnnotations;

namespace Api;

public class CalificacionCreateDTO
{
    [Required]
    public uint IdPlato { get; set; }
    
    [Required]
    public uint IdUsuario { get; set; }
    
    public uint? IdPedido { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
    public byte Puntuacion { get; set; }
    
    [MaxLength(300)]
    public string? Comentario { get; set; }
}
