using System.ComponentModel.DataAnnotations;

namespace Api;

public class PedidoCreateDTO
{
    [Required]
    public uint IdUsuario { get; set; }
    
    public uint? IdReserva { get; set; }
    
    public uint? IdMetodoPago { get; set; }
    
    public bool EsPreOrden { get; set; } = false;
    
    public string? Comentarios { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "El pedido debe tener al menos un plato")]
    public List<DetallePedidoCreateDTO> Detalles { get; set; } = new();
}

public class DetallePedidoCreateDTO
{
    [Required]
    public uint IdPlato { get; set; }
    
    [Required]
    [Range(1, 50, ErrorMessage = "La cantidad debe estar entre 1 y 50")]
    public byte Cantidad { get; set; }
    
    public string? Comentarios { get; set; }
}
