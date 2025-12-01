namespace Api;

public class PedidoCreateDTO
{
    public uint IdUsuario { get; set; }
    public uint? IdReserva { get; set; }
    public uint IdMetodoPago { get; set; }
    public bool EsPreOrden { get; set; }
    public string? Comentarios { get; set; }

    public List<PedidoDetalleCreateDTO> Detalles { get; set; } = new();

}
