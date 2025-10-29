using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Pedidos realizados por los clientes
/// </summary>
public partial class Pedido
{
    public uint IdPedido { get; set; }

    public uint IdUsuario { get; set; }

    /// <summary>
    /// NULL si es pedido sin reserva
    /// </summary>
    public uint? IdReserva { get; set; }

    public uint IdEstadoPedido { get; set; }

    public uint? IdMetodoPago { get; set; }

    public DateTime FechaHoraPedido { get; set; }

    public DateTime? FechaHoraEntregaEstimada { get; set; }

    public DateTime? FechaHoraEntregaReal { get; set; }

    public bool EsPreOrden { get; set; }

    public decimal Total { get; set; }

    public string? Comentarios { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Detallepedido> Detallepedidos { get; set; } = new List<Detallepedido>();

    public virtual Estadopedido IdEstadoPedidoNavigation { get; set; } = null!;

    public virtual Metodopago? IdMetodoPagoNavigation { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
