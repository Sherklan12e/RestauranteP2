using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoriaplato",
                columns: table => new
                {
                    idCategoria = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idCategoria);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Configuracionrestaurantes",
                columns: table => new
                {
                    IdConfiguracion = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Clave = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracionrestaurantes", x => x.IdConfiguracion);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "estadopedido",
                columns: table => new
                {
                    idEstadoPedido = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEstadoPedido);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Horariorestaurantes",
                columns: table => new
                {
                    IdHorario = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiaSemana = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    HoraApertura = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraCierre = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horariorestaurantes", x => x.IdHorario);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Ingredientes",
                columns: table => new
                {
                    IdIngrediente = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadMedida = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockActual = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    EsCritico = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredientes", x => x.IdIngrediente);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "mesa",
                columns: table => new
                {
                    idMesa = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroMesa = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacidad = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idMesa);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Metodopago",
                columns: table => new
                {
                    IdMetodoPago = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TipoMedioPago = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metodopago", x => x.IdMetodoPago);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idUsuario = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Apellido = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contrasena = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idUsuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "plato",
                columns: table => new
                {
                    idPlato = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idCategoria = table.Column<uint>(type: "int unsigned", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TiempoPreparacion = table.Column<uint>(type: "int unsigned", nullable: false),
                    ImagenURL = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Disponible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EsMenuDelDia = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPlato);
                    table.ForeignKey(
                        name: "fk_Plato_Categoria",
                        column: x => x.idCategoria,
                        principalTable: "categoriaplato",
                        principalColumn: "idCategoria",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "reserva",
                columns: table => new
                {
                    idReserva = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idUsuario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idMesa = table.Column<uint>(type: "int unsigned", nullable: true),
                    FechaHora = table.Column<DateTime>(type: "datetime", nullable: false),
                    CantidadPersonas = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Estado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comentarios = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idReserva);
                    table.ForeignKey(
                        name: "fk_Reserva_Mesa",
                        column: x => x.idMesa,
                        principalTable: "mesa",
                        principalColumn: "idMesa");
                    table.ForeignKey(
                        name: "fk_Reserva_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Historialdisponibilidads",
                columns: table => new
                {
                    IdHistorial = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPlato = table.Column<uint>(type: "int unsigned", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Disponible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Motivo = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdPlatoNavigationIdPlato = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historialdisponibilidads", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_Historialdisponibilidads_plato_IdPlatoNavigationIdPlato",
                        column: x => x.IdPlatoNavigationIdPlato,
                        principalTable: "plato",
                        principalColumn: "idPlato",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Platoingredientes",
                columns: table => new
                {
                    IdPlato = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdIngrediente = table.Column<uint>(type: "int unsigned", nullable: false),
                    CantidadNecesaria = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IdIngredienteNavigationIdIngrediente = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdPlatoNavigationIdPlato = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platoingredientes", x => x.IdPlato);
                    table.ForeignKey(
                        name: "FK_Platoingredientes_Ingredientes_IdIngredienteNavigationIdIngr~",
                        column: x => x.IdIngredienteNavigationIdIngrediente,
                        principalTable: "Ingredientes",
                        principalColumn: "IdIngrediente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Platoingredientes_plato_IdPlatoNavigationIdPlato",
                        column: x => x.IdPlatoNavigationIdPlato,
                        principalTable: "plato",
                        principalColumn: "idPlato",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    idPedido = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idUsuario = table.Column<uint>(type: "int unsigned", nullable: false),
                    idReserva = table.Column<uint>(type: "int unsigned", nullable: true),
                    idEstadoPedido = table.Column<uint>(type: "int unsigned", nullable: false),
                    idMetodoPago = table.Column<uint>(type: "int unsigned", nullable: true),
                    FechaHoraPedido = table.Column<DateTime>(type: "datetime", nullable: false),
                    FechaHoraEntregaEstimada = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaHoraEntregaReal = table.Column<DateTime>(type: "datetime", nullable: true),
                    EsPreOrden = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Comentarios = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPedido);
                    table.ForeignKey(
                        name: "fk_Pedido_EstadoPedido",
                        column: x => x.idEstadoPedido,
                        principalTable: "estadopedido",
                        principalColumn: "idEstadoPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_Pedido_MetodoPago",
                        column: x => x.idMetodoPago,
                        principalTable: "Metodopago",
                        principalColumn: "IdMetodoPago");
                    table.ForeignKey(
                        name: "fk_Pedido_Reserva",
                        column: x => x.idReserva,
                        principalTable: "reserva",
                        principalColumn: "idReserva");
                    table.ForeignKey(
                        name: "fk_Pedido_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Calificacions",
                columns: table => new
                {
                    IdCalificacion = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPlato = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdUsuario = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdPedido = table.Column<uint>(type: "int unsigned", nullable: true),
                    Puntuacion = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Comentario = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdPedidoNavigationIdPedido = table.Column<uint>(type: "int unsigned", nullable: true),
                    IdPlatoNavigationIdPlato = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdUsuarioNavigationIdUsuario = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificacions", x => x.IdCalificacion);
                    table.ForeignKey(
                        name: "FK_Calificacions_pedido_IdPedidoNavigationIdPedido",
                        column: x => x.IdPedidoNavigationIdPedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido");
                    table.ForeignKey(
                        name: "FK_Calificacions_plato_IdPlatoNavigationIdPlato",
                        column: x => x.IdPlatoNavigationIdPlato,
                        principalTable: "plato",
                        principalColumn: "idPlato",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificacions_usuario_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "detallepedido",
                columns: table => new
                {
                    idDetallePedido = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idPedido = table.Column<uint>(type: "int unsigned", nullable: false),
                    idPlato = table.Column<uint>(type: "int unsigned", nullable: false),
                    Cantidad = table.Column<sbyte>(type: "tinyint", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Comentarios = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idDetallePedido);
                    table.ForeignKey(
                        name: "fk_DetallePedido_Pedido",
                        column: x => x.idPedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_DetallePedido_Plato",
                        column: x => x.idPlato,
                        principalTable: "plato",
                        principalColumn: "idPlato",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacions_IdPedidoNavigationIdPedido",
                table: "Calificacions",
                column: "IdPedidoNavigationIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacions_IdPlatoNavigationIdPlato",
                table: "Calificacions",
                column: "IdPlatoNavigationIdPlato");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacions_IdUsuarioNavigationIdUsuario",
                table: "Calificacions",
                column: "IdUsuarioNavigationIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_detallepedido_idPedido",
                table: "detallepedido",
                column: "idPedido");

            migrationBuilder.CreateIndex(
                name: "IX_detallepedido_idPlato",
                table: "detallepedido",
                column: "idPlato");

            migrationBuilder.CreateIndex(
                name: "IX_Historialdisponibilidads_IdPlatoNavigationIdPlato",
                table: "Historialdisponibilidads",
                column: "IdPlatoNavigationIdPlato");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_idEstadoPedido",
                table: "pedido",
                column: "idEstadoPedido");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_idMetodoPago",
                table: "pedido",
                column: "idMetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_idReserva",
                table: "pedido",
                column: "idReserva");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_idUsuario",
                table: "pedido",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_plato_idCategoria",
                table: "plato",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Platoingredientes_IdIngredienteNavigationIdIngrediente",
                table: "Platoingredientes",
                column: "IdIngredienteNavigationIdIngrediente");

            migrationBuilder.CreateIndex(
                name: "IX_Platoingredientes_IdPlatoNavigationIdPlato",
                table: "Platoingredientes",
                column: "IdPlatoNavigationIdPlato");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_idMesa",
                table: "reserva",
                column: "idMesa");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_idUsuario",
                table: "reserva",
                column: "idUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificacions");

            migrationBuilder.DropTable(
                name: "Configuracionrestaurantes");

            migrationBuilder.DropTable(
                name: "detallepedido");

            migrationBuilder.DropTable(
                name: "Historialdisponibilidads");

            migrationBuilder.DropTable(
                name: "Horariorestaurantes");

            migrationBuilder.DropTable(
                name: "Platoingredientes");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "plato");

            migrationBuilder.DropTable(
                name: "estadopedido");

            migrationBuilder.DropTable(
                name: "Metodopago");

            migrationBuilder.DropTable(
                name: "reserva");

            migrationBuilder.DropTable(
                name: "categoriaplato");

            migrationBuilder.DropTable(
                name: "mesa");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
