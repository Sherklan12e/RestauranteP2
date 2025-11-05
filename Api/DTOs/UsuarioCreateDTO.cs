namespace Api;

public class UsuarioCreateDTO
{
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Contrasena { get; set; } = string.Empty;
}
