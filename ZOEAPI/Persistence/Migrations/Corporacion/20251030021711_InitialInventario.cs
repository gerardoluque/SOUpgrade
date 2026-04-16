using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class InitialInventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntradasMedicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroPedido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroRemision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RFCProveedor = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaFactura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEntrada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoMoneda = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PorcentajeIVA = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ImporteSinIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ImporteConIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Recibido = table.Column<bool>(type: "bit", nullable: true),
                    FechaRecepcion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CorporacionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FarmaciaId = table.Column<int>(type: "int", nullable: false),
                    Ejercicio = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: true),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponsableMovimiento = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UsuarioModificoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasMedicamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalidasMedicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaSolicitante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmpleadoSolicitante = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PacienteId = table.Column<int>(type: "int", nullable: true),
                    CorporacionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FarmaciaId = table.Column<int>(type: "int", nullable: false),
                    Ejercicio = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: true),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponsableMovimiento = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UsuarioModificoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidasMedicamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetalleEntradasMedicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntradaMedicamentoId = table.Column<int>(type: "int", nullable: false),
                    MedicamentoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitarioSinIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecioUnitarioConIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubtotalSinIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubtotalConIVA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Lote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleEntradasMedicamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleEntradasMedicamentos_EntradasMedicamentos_EntradaMedicamentoId",
                        column: x => x.EntradaMedicamentoId,
                        principalTable: "EntradasMedicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleEntradasMedicamentos_Medicamentos_MedicamentoId",
                        column: x => x.MedicamentoId,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleSalidasMedicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalidaMedicamentoId = table.Column<int>(type: "int", nullable: false),
                    MedicamentoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleSalidasMedicamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleSalidasMedicamentos_Medicamentos_MedicamentoId",
                        column: x => x.MedicamentoId,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleSalidasMedicamentos_SalidasMedicamentos_SalidaMedicamentoId",
                        column: x => x.SalidaMedicamentoId,
                        principalTable: "SalidasMedicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEntradasMedicamentos_EntradaMedicamentoId",
                table: "DetalleEntradasMedicamentos",
                column: "EntradaMedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEntradasMedicamentos_MedicamentoId",
                table: "DetalleEntradasMedicamentos",
                column: "MedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSalidasMedicamentos_MedicamentoId",
                table: "DetalleSalidasMedicamentos",
                column: "MedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSalidasMedicamentos_SalidaMedicamentoId",
                table: "DetalleSalidasMedicamentos",
                column: "SalidaMedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasMedicamentos_Ejercicio_FarmaciaId",
                table: "EntradasMedicamentos",
                columns: new[] { "Ejercicio", "FarmaciaId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntradasMedicamentos_FarmaciaId_FechaMovimiento",
                table: "EntradasMedicamentos",
                columns: new[] { "FarmaciaId", "FechaMovimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_EntradasMedicamentos_FechaMovimiento",
                table: "EntradasMedicamentos",
                column: "FechaMovimiento");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasMedicamentos_NumeroDocumento",
                table: "EntradasMedicamentos",
                column: "NumeroDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_SalidasMedicamentos_Ejercicio_FarmaciaId",
                table: "SalidasMedicamentos",
                columns: new[] { "Ejercicio", "FarmaciaId" });

            migrationBuilder.CreateIndex(
                name: "IX_SalidasMedicamentos_FarmaciaId_FechaMovimiento",
                table: "SalidasMedicamentos",
                columns: new[] { "FarmaciaId", "FechaMovimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_SalidasMedicamentos_FechaMovimiento",
                table: "SalidasMedicamentos",
                column: "FechaMovimiento");

            migrationBuilder.CreateIndex(
                name: "IX_SalidasMedicamentos_NumeroDocumento",
                table: "SalidasMedicamentos",
                column: "NumeroDocumento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleEntradasMedicamentos");

            migrationBuilder.DropTable(
                name: "DetalleSalidasMedicamentos");

            migrationBuilder.DropTable(
                name: "EntradasMedicamentos");

            migrationBuilder.DropTable(
                name: "SalidasMedicamentos");
        }
    }
}
