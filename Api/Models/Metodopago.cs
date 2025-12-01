    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace Api.Models;

    /// <summary>
    /// Métodos de pago disponibles
    /// </summary>
    public partial class Metodopago
    {
        [Key]
        public uint IdMetodoPago { get; set; }

        public string TipoMedioPago { get; set; } = null!;

        public bool? Activo { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
