﻿using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Estados posibles de un pedido (Pendiente, En Preparación, Listo, Entregado, Cancelado)
/// </summary>
public partial class Estadopedido
{
    public uint IdEstadoPedido { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
