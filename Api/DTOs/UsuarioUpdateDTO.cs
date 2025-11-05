namespace Api;

public class UsuarioUpdateDTO
{
 // No incluimos IdUsuario ni FechaRegistro ni Activo (esas las gestiona el servidor/BD)
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        // Si permitís que el usuario cambie la contraseña por la API:
        public string? Contrasena { get; set; }
}
