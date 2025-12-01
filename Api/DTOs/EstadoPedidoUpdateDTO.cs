using System.ComponentModel.DataAnnotations;
namespace Api;

public class EstadoPedidoUpdateDTO
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}
