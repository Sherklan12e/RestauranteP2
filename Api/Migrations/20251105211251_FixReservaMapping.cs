using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class FixReservaMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Reservas_IdReservaNavigationIdReserva",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Mesas_IdMesaNavigationIdMesa",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_usuario_IdUsuarioNavigationIdUsuario",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_IdMesaNavigationIdMesa",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_IdUsuarioNavigationIdUsuario",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "IdMesaNavigationIdMesa",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "IdUsuarioNavigationIdUsuario",
                table: "Reservas");

            migrationBuilder.RenameTable(
                name: "Reservas",
                newName: "reserva");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "reserva",
                newName: "idUsuario");

            migrationBuilder.RenameColumn(
                name: "IdMesa",
                table: "reserva",
                newName: "idMesa");

            migrationBuilder.RenameColumn(
                name: "IdReserva",
                table: "reserva",
                newName: "idReserva");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHora",
                table: "reserva",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "reserva",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "reserva",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Comentarios",
                table: "reserva",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<sbyte>(
                name: "CantidadPersonas",
                table: "reserva",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "reserva",
                column: "idReserva");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_idMesa",
                table: "reserva",
                column: "idMesa");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_idUsuario",
                table: "reserva",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_reserva_IdReservaNavigationIdReserva",
                table: "Pedidos",
                column: "IdReservaNavigationIdReserva",
                principalTable: "reserva",
                principalColumn: "idReserva");

            migrationBuilder.AddForeignKey(
                name: "fk_Reserva_Mesa",
                table: "reserva",
                column: "idMesa",
                principalTable: "Mesas",
                principalColumn: "IdMesa");

            migrationBuilder.AddForeignKey(
                name: "fk_Reserva_Usuario",
                table: "reserva",
                column: "idUsuario",
                principalTable: "usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_reserva_IdReservaNavigationIdReserva",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "fk_Reserva_Mesa",
                table: "reserva");

            migrationBuilder.DropForeignKey(
                name: "fk_Reserva_Usuario",
                table: "reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "reserva");

            migrationBuilder.DropIndex(
                name: "IX_reserva_idMesa",
                table: "reserva");

            migrationBuilder.DropIndex(
                name: "IX_reserva_idUsuario",
                table: "reserva");

            migrationBuilder.RenameTable(
                name: "reserva",
                newName: "Reservas");

            migrationBuilder.RenameColumn(
                name: "idUsuario",
                table: "Reservas",
                newName: "IdUsuario");

            migrationBuilder.RenameColumn(
                name: "idMesa",
                table: "Reservas",
                newName: "IdMesa");

            migrationBuilder.RenameColumn(
                name: "idReserva",
                table: "Reservas",
                newName: "IdReserva");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHora",
                table: "Reservas",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Reservas",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Reservas",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Comentarios",
                table: "Reservas",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<byte>(
                name: "CantidadPersonas",
                table: "Reservas",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<uint>(
                name: "IdMesaNavigationIdMesa",
                table: "Reservas",
                type: "int unsigned",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "IdUsuarioNavigationIdUsuario",
                table: "Reservas",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas",
                column: "IdReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdMesaNavigationIdMesa",
                table: "Reservas",
                column: "IdMesaNavigationIdMesa");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdUsuarioNavigationIdUsuario",
                table: "Reservas",
                column: "IdUsuarioNavigationIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Reservas_IdReservaNavigationIdReserva",
                table: "Pedidos",
                column: "IdReservaNavigationIdReserva",
                principalTable: "Reservas",
                principalColumn: "IdReserva");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Mesas_IdMesaNavigationIdMesa",
                table: "Reservas",
                column: "IdMesaNavigationIdMesa",
                principalTable: "Mesas",
                principalColumn: "IdMesa");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_usuario_IdUsuarioNavigationIdUsuario",
                table: "Reservas",
                column: "IdUsuarioNavigationIdUsuario",
                principalTable: "usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
