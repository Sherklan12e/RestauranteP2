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
    public List<AdminPedidoDetalleDTO> Detalles { get; set; } = new List<AdminPedidoDetalleDTO>();
}

public class AdminPedidoDetalleDTO
{
    public uint IdDetallePedido { get; set; }
    public uint IdPlato { get; set; }
    public string NombrePlato { get; set; } = string.Empty;
    public byte Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
