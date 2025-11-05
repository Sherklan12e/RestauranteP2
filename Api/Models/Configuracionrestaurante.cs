using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Configuraciones generales del sistema (tiempo antes de cierre para pedidos, etc.)
/// </summary>
public partial class Configuracionrestaurante
{
    [Key]
    public uint IdConfiguracion { get; set; }

    public string Clave { get; set; } = null!;

    public string Valor { get; set; } = null!;

    public string? Descripcion { get; set; }
}
