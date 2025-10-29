using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Mesas del restaurante
/// </summary>
public partial class Mesa
{
    public uint IdMesa { get; set; }

    public string NumeroMesa { get; set; } = null!;

    public byte Capacidad { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
