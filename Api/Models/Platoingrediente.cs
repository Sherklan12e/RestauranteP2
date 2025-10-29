using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Relación entre platos e ingredientes necesarios
/// </summary>
public partial class Platoingrediente
{
    public uint IdPlato { get; set; }

    public uint IdIngrediente { get; set; }

    public decimal CantidadNecesaria { get; set; }

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;

    public virtual Plato IdPlatoNavigation { get; set; } = null!;
}
