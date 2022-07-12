using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenericRepository.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contrasena = table.Column<string>(type: "varchar(20)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    Genero = table.Column<string>(type: "varchar(20)", nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: true),
                    Identificacion = table.Column<string>(type: "varchar(20)", nullable: true),
                    Direccion = table.Column<string>(type: "varchar(100)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCuenta = table.Column<string>(type: "varchar(20)", nullable: false),
                    TipoCuenta = table.Column<string>(type: "varchar(20)", nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuenta_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    TipoMovimiento = table.Column<string>(type: "varchar(1)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CuentaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimiento_Cuenta_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_ClienteId",
                table: "Cuenta",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_CuentaId",
                table: "Movimiento",
                column: "CuentaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimiento");

            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
