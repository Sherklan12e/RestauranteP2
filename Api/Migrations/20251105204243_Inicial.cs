using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categoriaplatos",
                columns: table => new
                {
                    IdCategoria = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoriaplatos", x => x.IdCategoria);
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
                name: "Estadopedidos",
                columns: table => new
                {
                    IdEstadoPedido = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estadopedidos", x => x.IdEstadoPedido);
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
                name: "Mesas",
                columns: table => new
                {
                    IdMesa = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroMesa = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacidad = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesas", x => x.IdMesa);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Metodopagos",
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
                    table.PrimaryKey("PK_Metodopagos", x => x.IdMetodoPago);
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
                        principalTable: "Categoriaplatos",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    IdReserva = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdMesa = table.Column<uint>(type: "int unsigned", nullable: true),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CantidadPersonas = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Estado = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comentarios = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdMesaNavigationIdMesa = table.Column<uint>(type: "int unsigned", nullable: true),
                    IdUsuarioNavigationIdUsuario = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_Reservas_Mesas_IdMesaNavigationIdMesa",
                        column: x => x.IdMesaNavigationIdMesa,
                        principalTable: "Mesas",
                        principalColumn: "IdMesa");
                    table.ForeignKey(
                        name: "FK_Reservas_usuario_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
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
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdReserva = table.Column<uint>(type: "int unsigned", nullable: true),
                    IdEstadoPedido = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdMetodoPago = table.Column<uint>(type: "int unsigned", nullable: true),
                    FechaHoraPedido = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaHoraEntregaEstimada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaHoraEntregaReal = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EsPreOrden = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Comentarios = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdEstadoPedidoNavigationIdEstadoPedido = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdMetodoPagoNavigationIdMetodoPago = table.Column<uint>(type: "int unsigned", nullable: true),
                    IdReservaNavigationIdReserva = table.Column<uint>(type: "int unsigned", nullable: true),
                    IdUsuarioNavigationIdUsuario = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Estadopedidos_IdEstadoPedidoNavigationIdEstadoPedido",
                        column: x => x.IdEstadoPedidoNavigationIdEstadoPedido,
                        principalTable: "Estadopedidos",
                        principalColumn: "IdEstadoPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Metodopagos_IdMetodoPagoNavigationIdMetodoPago",
                        column: x => x.IdMetodoPagoNavigationIdMetodoPago,
                        principalTable: "Metodopagos",
                        principalColumn: "IdMetodoPago");
                    table.ForeignKey(
                        name: "FK_Pedidos_Reservas_IdReservaNavigationIdReserva",
                        column: x => x.IdReservaNavigationIdReserva,
                        principalTable: "Reservas",
                        principalColumn: "IdReserva");
                    table.ForeignKey(
                        name: "FK_Pedidos_usuario_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
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
                        name: "FK_Calificacions_Pedidos_IdPedidoNavigationIdPedido",
                        column: x => x.IdPedidoNavigationIdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido");
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
                name: "Detallepedidos",
                columns: table => new
                {
                    IdDetallePedido = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPedido = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdPlato = table.Column<uint>(type: "int unsigned", nullable: false),
                    Cantidad = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Comentarios = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdPedidoNavigationIdPedido = table.Column<uint>(type: "int unsigned", nullable: false),
                    IdPlatoNavigationIdPlato = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detallepedidos", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_Detallepedidos_Pedidos_IdPedidoNavigationIdPedido",
                        column: x => x.IdPedidoNavigationIdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detallepedidos_plato_IdPlatoNavigationIdPlato",
                        column: x => x.IdPlatoNavigationIdPlato,
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
                name: "IX_Detallepedidos_IdPedidoNavigationIdPedido",
                table: "Detallepedidos",
                column: "IdPedidoNavigationIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Detallepedidos_IdPlatoNavigationIdPlato",
                table: "Detallepedidos",
                column: "IdPlatoNavigationIdPlato");

            migrationBuilder.CreateIndex(
                name: "IX_Historialdisponibilidads_IdPlatoNavigationIdPlato",
                table: "Historialdisponibilidads",
                column: "IdPlatoNavigationIdPlato");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdEstadoPedidoNavigationIdEstadoPedido",
                table: "Pedidos",
                column: "IdEstadoPedidoNavigationIdEstadoPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdMetodoPagoNavigationIdMetodoPago",
                table: "Pedidos",
                column: "IdMetodoPagoNavigationIdMetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdReservaNavigationIdReserva",
                table: "Pedidos",
                column: "IdReservaNavigationIdReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdUsuarioNavigationIdUsuario",
                table: "Pedidos",
                column: "IdUsuarioNavigationIdUsuario");

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
                name: "IX_Reservas_IdMesaNavigationIdMesa",
                table: "Reservas",
                column: "IdMesaNavigationIdMesa");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdUsuarioNavigationIdUsuario",
                table: "Reservas",
                column: "IdUsuarioNavigationIdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificacions");

            migrationBuilder.DropTable(
                name: "Configuracionrestaurantes");

            migrationBuilder.DropTable(
                name: "Detallepedidos");

            migrationBuilder.DropTable(
                name: "Historialdisponibilidads");

            migrationBuilder.DropTable(
                name: "Horariorestaurantes");

            migrationBuilder.DropTable(
                name: "Platoingredientes");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "plato");

            migrationBuilder.DropTable(
                name: "Estadopedidos");

            migrationBuilder.DropTable(
                name: "Metodopagos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Categoriaplatos");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
