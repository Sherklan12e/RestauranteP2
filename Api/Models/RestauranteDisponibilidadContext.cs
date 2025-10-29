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
    public virtual DbSet<Pedido> Pedidos { get; set; }
    public virtual DbSet<Detallepedido> Detallepedidos { get; set; }
    public virtual DbSet<Estadopedido> Estadopedidos { get; set; }
    public virtual DbSet<Reserva> Reservas { get; set; }
    public virtual DbSet<Calificacion> Calificacions { get; set; }
    public virtual DbSet<Mesa> Mesas { get; set; }
    public virtual DbSet<Metodopago> Metodopagos { get; set; }
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

        // Configuraciones adicionales para otras tablas...
        // (Puedes mantener tu configuración actual aquí)

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}