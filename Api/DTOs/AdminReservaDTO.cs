namespace Api.DTOs;

public class AdminReservaDTO
{
    public uint IdReserva { get; set; }
    public uint IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public string NumeroMesa { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public byte CantidadPersonas { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
