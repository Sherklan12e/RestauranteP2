namespace Api;

public class MetodoPagoReadDTO
{
    public uint IdMetodoPago { get; set; }
    public string TipoMedioPago { get; set; } = null!;
    public bool? Activo { get; set; }
}
