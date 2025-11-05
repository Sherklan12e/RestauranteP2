using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Reservas de mesas
/// </summary>
public partial class Reserva
{
    [Key]
    public uint IdReserva { get; set; }

    public uint IdUsuario { get; set; }

    public uint? IdMesa { get; set; }

    public DateTime FechaHora { get; set; }

    public byte CantidadPersonas { get; set; }

    public string Estado { get; set; } = null!;

    public string? Comentarios { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Mesa? IdMesaNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
