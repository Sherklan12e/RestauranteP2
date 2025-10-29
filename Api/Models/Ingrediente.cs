using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Ingredientes para control de disponibilidad
/// </summary>
public partial class Ingrediente
{
    public uint IdIngrediente { get; set; }

    public string Nombre { get; set; } = null!;

    /// <summary>
    /// kg, litros, unidades, etc.
    /// </summary>
    public string UnidadMedida { get; set; } = null!;

    public decimal StockActual { get; set; }

    public decimal StockMinimo { get; set; }

    /// <summary>
    /// Si es crítico para la disponibilidad
    /// </summary>
    public bool EsCritico { get; set; }

    public virtual ICollection<Platoingrediente> Platoingredientes { get; set; } = new List<Platoingrediente>();
}
