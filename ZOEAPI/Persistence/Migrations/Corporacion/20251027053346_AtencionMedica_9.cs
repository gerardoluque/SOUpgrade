using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_SustanciaActiva_TipoPresentacion",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "TipoPresentacion",
                table: "Medicamentos");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Medicamentos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClasificacionId",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaveArticulo",
                table: "Medicamentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Consecutivo",
                table: "Medicamentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorporacionId",
                table: "Medicamentos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescripcionArticulo",
                table: "Medicamentos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Farmacia",
                table: "Medicamentos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "Medicamentos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IndicadorConIVA",
                table: "Medicamentos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PNoCMS",
                table: "Medicamentos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeIVA",
                table: "Medicamentos",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitarioConIVA",
                table: "Medicamentos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitarioSinIVA",
                table: "Medicamentos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentacionId",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMaximo",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMinimo",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoArticuloId",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoControlId",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadMedidaId",
                table: "Medicamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Activo",
                table: "Medicamentos",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_ClaveArticulo",
                table: "Medicamentos",
                column: "ClaveArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Consecutivo",
                table: "Medicamentos",
                column: "Consecutivo");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_SustanciaActiva_PresentacionId",
                table: "Medicamentos",
                columns: new[] { "SustanciaActiva", "PresentacionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_Activo",
                table: "Medicamentos");

            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_ClaveArticulo",
                table: "Medicamentos");

            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_Consecutivo",
                table: "Medicamentos");

            migrationBuilder.DropIndex(
                name: "IX_Medicamentos_SustanciaActiva_PresentacionId",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "ClasificacionId",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "ClaveArticulo",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "Consecutivo",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "CorporacionId",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "DescripcionArticulo",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "Farmacia",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "FechaAlta",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "IndicadorConIVA",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "PNoCMS",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "PorcentajeIVA",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "PrecioUnitarioConIVA",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "PrecioUnitarioSinIVA",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "PresentacionId",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "StockMaximo",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "TipoArticuloId",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "TipoControlId",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "UnidadMedidaId",
                table: "Medicamentos");

            migrationBuilder.AddColumn<int>(
                name: "TipoPresentacion",
                table: "Medicamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_SustanciaActiva_TipoPresentacion",
                table: "Medicamentos",
                columns: new[] { "SustanciaActiva", "TipoPresentacion" });
        }
    }
}
