namespace Api;

public class UsuarioDTO
{
        public uint IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = "cliente";
        public DateTime FechaRegistro { get; set; }
        
}
