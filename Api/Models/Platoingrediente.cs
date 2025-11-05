using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Relación entre platos e ingredientes necesarios
/// </summary>
public partial class Platoingrediente
{
    [Key]
    public uint IdPlato { get; set; }

    public uint IdIngrediente { get; set; }

    public decimal CantidadNecesaria { get; set; }

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;

    public virtual Plato IdPlatoNavigation { get; set; } = null!;
}
