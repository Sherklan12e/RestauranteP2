namespace Api;

public class CalificacionDTO
{
public uint IdCalificacion { get; set; }
        public uint IdPlato { get; set; }
        public uint IdUsuario { get; set; }
        public uint? IdPedido { get; set; }
        public byte Puntuacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime FechaHora { get; set; }

        // Opcional: si querés mostrar nombres o datos relacionados
        public string? NombrePlato { get; set; }
        public string? NombreUsuario { get; set; }
}
