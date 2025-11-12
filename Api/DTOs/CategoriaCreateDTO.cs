using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class CategoriaCreateDTO
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
        public string? Descripcion { get; set; }
    }
}