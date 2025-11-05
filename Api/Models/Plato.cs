using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Api.Models;

/// <summary>
/// Platos del menú del restaurante
/// </summary>
public partial class Plato
{
    [Key]
    public uint IdPlato { get; set; }

    public uint IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    /// <summary>
    /// Tiempo en minutos
    /// </summary>
    public uint TiempoPreparacion { get; set; }

    public string? ImagenUrl { get; set; }

    // CAMBIAR de bool? a bool con valores por defecto
    public bool Disponible { get; set; } = true;

    public bool EsMenuDelDia { get; set; } = false;

    // CAMBIAR de bool? a bool con valores por defecto
    public bool Activo { get; set; } = true;

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Detallepedido> Detallepedidos { get; set; } = new List<Detallepedido>();

    public virtual ICollection<Historialdisponibilidad> Historialdisponibilidads { get; set; } = new List<Historialdisponibilidad>();

    public virtual Categoriaplato IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Platoingrediente> Platoingredientes { get; set; } = new List<Platoingrediente>();
}