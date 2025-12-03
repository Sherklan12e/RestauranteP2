namespace Api;

public class PedidoDetalleDTO
{
    public uint IdDetallePedido { get; set; }
    public uint IdPlato { get; set; }
    public string NombrePlato { get; set; } = string.Empty;
    public byte Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Comentarios { get; set; }
}
