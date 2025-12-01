using System.ComponentModel.DataAnnotations;
namespace Api;

public class MetodoPagoUpdateDTO
{
    [Required]
    public string TipoMedioPago { get; set; } = null!;

    public bool? Activo { get; set; }
}
