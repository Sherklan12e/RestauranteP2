using System.ComponentModel.DataAnnotations;
namespace Api;

public class ReservaEstadoDTO
{
[Required]
        public string Estado { get; set; } = string.Empty;
}
