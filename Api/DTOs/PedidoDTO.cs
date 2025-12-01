namespace Api;

public class PedidoDTO
{
    public uint IdPedido { get; set; }
    public uint IdUsuario { get; set; }
    public uint? IdReserva { get; set; }
    public uint IdEstadoPedido { get; set; }
    public uint? IdMetodoPago { get; set; }

    public DateTime FechaHoraPedido { get; set; }
    public bool EsPreOrden { get; set; }
    public decimal Total { get; set; }
    public string? Comentarios { get; set; }
    public List<PedidoDetalleDTO> Detalles { get; set; } = new();
}
