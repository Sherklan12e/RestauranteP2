using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Registro histórico de cambios en disponibilidad de platos
/// </summary>
public partial class Historialdisponibilidad
{
    public uint IdHistorial { get; set; }

    public uint IdPlato { get; set; }

    public DateTime FechaHora { get; set; }

    public bool Disponible { get; set; }

    /// <summary>
    /// Razón del cambio de disponibilidad
    /// </summary>
    public string? Motivo { get; set; }

    public virtual Plato IdPlatoNavigation { get; set; } = null!;
}
