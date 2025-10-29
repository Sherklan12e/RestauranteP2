﻿using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Métodos de pago disponibles
/// </summary>
public partial class Metodopago
{
    public uint IdMetodoPago { get; set; }

    public string TipoMedioPago { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
