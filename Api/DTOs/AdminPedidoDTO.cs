namespace Api.DTOs;

public class AdminPedidoDTO
{
    public uint IdPedido { get; set; }
    public uint IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public DateTime FechaHoraPedido { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Comentarios { get; set; }
    public int CantidadPlatos { get; set; }
}
