namespace Api;

public class PedidoDTO
{
    public uint IdPedido { get; set; }
    public uint IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public uint? IdReserva { get; set; }
    public uint IdEstadoPedido { get; set; }
    public string EstadoNombre { get; set; } = string.Empty;
    public uint? IdMetodoPago { get; set; }
    public string? MetodoPagoNombre { get; set; }
    public DateTime FechaHoraPedido { get; set; }
    public DateTime? FechaHoraEntregaEstimada { get; set; }
    public DateTime? FechaHoraEntregaReal { get; set; }
    public bool EsPreOrden { get; set; }
    public decimal Total { get; set; }
    public string? Comentarios { get; set; }
    public List<DetallePedidoDTO> Detalles { get; set; } = new();
}

public class DetallePedidoDTO
{
    public uint IdDetallePedido { get; set; }
    public uint IdPlato { get; set; }
    public string PlatoNombre { get; set; } = string.Empty;
    public byte Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Comentarios { get; set; }
}
