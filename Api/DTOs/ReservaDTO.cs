namespace Api;

public class ReservaDTO
{
        public uint IdReserva { get; set; }
        public uint IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public uint? IdMesa { get; set; }
        public string? NumeroMesa { get; set; }
        public DateTime FechaHora { get; set; }
        public byte CantidadPersonas { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Comentarios { get; set; }
        public DateTime FechaCreacion { get; set; }
}
