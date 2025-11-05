using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Calificaciones y comentarios de platos por usuarios
/// </summary>
public partial class Calificacion
{
    [Key]
    public uint IdCalificacion { get; set; }

    public uint IdPlato { get; set; }

    public uint IdUsuario { get; set; }

    public uint? IdPedido { get; set; }

    /// <summary>
    /// Escala 1-5
    /// </summary>
    public byte Puntuacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaHora { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Plato IdPlatoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
