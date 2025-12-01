using System.ComponentModel.DataAnnotations;
namespace Api;

public class MetodoPagoCreateDTO
{
    [Required]
    public string TipoMedioPago { get; set; } = null!;

    public bool Activo { get; set; } = true;
}
