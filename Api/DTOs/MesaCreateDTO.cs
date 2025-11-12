namespace Api;

public class MesaCreateDTO
{
    public string NumeroMesa { get; set; } = null!;
    public byte Capacidad { get; set; }
    public bool? Activa { get; set; } = true;
}
