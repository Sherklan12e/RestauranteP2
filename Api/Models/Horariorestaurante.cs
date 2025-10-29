﻿using System;
using System.Collections.Generic;

namespace Api.Models;

/// <summary>
/// Horarios de operación del restaurante
/// </summary>
public partial class Horariorestaurante
{
    public uint IdHorario { get; set; }

    /// <summary>
    /// 0=Domingo, 1=Lunes, ..., 6=Sábado
    /// </summary>
    public byte DiaSemana { get; set; }

    public TimeOnly HoraApertura { get; set; }

    public TimeOnly HoraCierre { get; set; }

    public bool? Activo { get; set; }
}
