using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Pomelo.EntityFrameworkCore;


namespace Api.Models;

public partial class RestauranteDisponibilidadContext : DbContext
{
    public RestauranteDisponibilidadContext(DbContextOptions<RestauranteDisponibilidadContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Plato> Platos { get; set; }
    public virtual DbSet<Categoriaplato> Categoriaplatos { get; set; }
    public virtual DbSet<Pedido> Pedido { get; set; }
    public virtual DbSet<Detallepedido> Detallepedidos { get; set; }
    public virtual DbSet<Estadopedido> Estadopedido { get; set; }
    public virtual DbSet<Reserva> Reservas { get; set; }
    public virtual DbSet<Calificacion> Calificacions { get; set; }
    public virtual DbSet<Mesa> Mesas { get; set; }
    public virtual DbSet<Metodopago> Metodopago { get; set; }
    public virtual DbSet<Ingrediente> Ingredientes { get; set; }
    public virtual DbSet<Platoingrediente> Platoingredientes { get; set; }
    public virtual DbSet<Historialdisponibilidad> Historialdisponibilidads { get; set; }
    public virtual DbSet<Horariorestaurante> Horariorestaurantes { get; set; }
    public virtual DbSet<Configuracionrestaurante> Configuracionrestaurantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración para MySQL
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        // Configuración de la tabla Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");
            entity.ToTable("usuario");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.Apellido).HasMaxLength(45);
            entity.Property(e => e.Email).HasMaxLength(60);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Contrasena).HasMaxLength(64);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Rol).HasMaxLength(20).HasDefaultValue("cliente");
        });

        // Configuración de la tabla Plato
        modelBuilder.Entity<Plato>(entity =>
        {
            entity.HasKey(e => e.IdPlato).HasName("PRIMARY");
            entity.ToTable("plato");
            entity.Property(e => e.IdPlato).HasColumnName("idPlato");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Precio).HasColumnType("decimal(10,2)");
            entity.Property(e => e.ImagenUrl).HasMaxLength(200).HasColumnName("ImagenURL");
            entity.Property(e => e.Disponible).HasDefaultValue(true);
            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdCategoriaNavigation)
                .WithMany(p => p.Platos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("fk_Plato_Categoria");
        });
// Configuración de la tabla Reserva
    modelBuilder.Entity<Reserva>(entity =>
{
    entity.HasKey(e => e.IdReserva).HasName("PRIMARY");
    entity.ToTable("reserva"); // 👈 fuerza el nombre correcto

    entity.Property(e => e.IdReserva).HasColumnName("idReserva");
    entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
    entity.Property(e => e.IdMesa).HasColumnName("idMesa");
    entity.Property(e => e.FechaHora).HasColumnType("datetime");
    entity.Property(e => e.CantidadPersonas).HasColumnType("tinyint");
    entity.Property(e => e.Estado).HasMaxLength(50);
    entity.Property(e => e.Comentarios).HasMaxLength(255);
    entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

    entity.HasOne(d => d.IdUsuarioNavigation)
        .WithMany(p => p.Reservas)
        .HasForeignKey(d => d.IdUsuario)
        .HasConstraintName("fk_Reserva_Usuario");

    entity.HasOne(d => d.IdMesaNavigation)
        .WithMany(p => p.Reservas)
        .HasForeignKey(d => d.IdMesa)
        .HasConstraintName("fk_Reserva_Mesa");
});

        // Configuración de la tabla Mesa
        modelBuilder.Entity<Mesa>(entity =>
    {
        entity.HasKey(e => e.IdMesa).HasName("PRIMARY");
        entity.ToTable("mesa"); // 👈 fuerza a usar el nombre correcto en MySQL

            entity.Property(e => e.IdMesa).HasColumnName("idMesa");
            entity.Property(e => e.NumeroMesa).HasMaxLength(20);
            entity.Property(e => e.Capacidad).HasColumnType("tinyint");
            entity.Property(e => e.Activa).HasDefaultValue(true);
        });

        // Configuración de la tabla Categoriaplato
        modelBuilder.Entity<Categoriaplato>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");
            entity.ToTable("categoriaplato"); // 

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
        });

        // Configuración de la tabla Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PRIMARY");
            entity.ToTable("pedido");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.IdReserva).HasColumnName("idReserva");
            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.IdMetodoPago).HasColumnName("idMetodoPago");
            entity.Property(e => e.FechaHoraPedido).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraEntregaEstimada).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraEntregaReal).HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Comentarios).HasMaxLength(255);

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_Pedido_Usuario");

            entity.HasOne(d => d.IdEstadoPedidoNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEstadoPedido)
                .HasConstraintName("fk_Pedido_EstadoPedido");

            entity.HasOne(d => d.IdMetodoPagoNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdMetodoPago)
                .HasConstraintName("fk_Pedido_MetodoPago");

            entity.HasOne(d => d.IdReservaNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("fk_Pedido_Reserva");
        });

        // Configuración de la tabla Estadopedido
        modelBuilder.Entity<Estadopedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido).HasName("PRIMARY");
            entity.ToTable("estadopedido");

            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
        });

        // Configuración de la tabla Detallepedido
        modelBuilder.Entity<Detallepedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PRIMARY");
            entity.ToTable("detallepedido");

            entity.Property(e => e.IdDetallePedido).HasColumnName("idDetallePedido");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdPlato).HasColumnName("idPlato");
            entity.Property(e => e.Cantidad).HasColumnType("tinyint");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Comentarios).HasMaxLength(255);

            entity.HasOne(d => d.IdPedidoNavigation)
                .WithMany(p => p.Detallepedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("fk_DetallePedido_Pedido");

            entity.HasOne(d => d.IdPlatoNavigation)
                .WithMany(p => p.Detallepedidos)
                .HasForeignKey(d => d.IdPlato)
                .HasConstraintName("fk_DetallePedido_Plato");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}