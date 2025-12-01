namespace Api;

public class PedidoDetalleCreateDTO
{
    public uint IdPlato { get; set; }
    public byte Cantidad { get; set; }
    public string? Comentarios { get; set; }
}
