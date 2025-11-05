using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Detalle de platos en cada pedido
/// </summary>
public partial class Detallepedido
{
    [Key]
    public uint IdDetallePedido { get; set; }

    public uint IdPedido { get; set; }

    public uint IdPlato { get; set; }

    public byte Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    /// <summary>
    /// Comentarios especiales del cliente
    /// </summary>
    public string? Comentarios { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Plato IdPlatoNavigation { get; set; } = null!;
}
