using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Categorías de platos (Entradas, Principales, Postres, Menú del Día, etc.)
/// </summary>
public partial class Categoriaplato
{
    [Key]
    public uint IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Plato> Platos { get; set; } = new List<Plato>();
}
