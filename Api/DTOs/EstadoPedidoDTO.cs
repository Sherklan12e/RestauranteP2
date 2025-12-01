namespace Api;

public class EstadoPedidoDTO
{
    public uint IdEstadoPedido { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
