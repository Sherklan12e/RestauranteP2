namespace Api;
using System.ComponentModel.DataAnnotations;

public class ReservaCreateDTO
{
        [Required]
        public uint IdUsuario { get; set; }

        public uint? IdMesa { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [Range(1, 20)]
        public byte CantidadPersonas { get; set; }

        public string? Comentarios { get; set; }
}
