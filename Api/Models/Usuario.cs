﻿using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Tabla de usuarios/clientes del restaurante
/// </summary>
public partial class Usuario
{
    public uint IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Contrasena { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; } = true;

 // Navigation properties
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    public virtual ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>
    ();
}
