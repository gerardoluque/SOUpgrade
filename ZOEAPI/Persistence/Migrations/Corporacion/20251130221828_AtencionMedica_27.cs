using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Persistence.Migrations.Corporacion
{
    /// <inheritdoc />
    public partial class AtencionMedica_27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertasBitacora",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<int>(type: "int", nullable: false),
                    Asunto = table.Column<int>(type: "int", nullable: false),
                    UsuarioAccion = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Detalles = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Leido = table.Column<bool>(type: "bit", nullable: false),
                    FechaLeido = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioLeyo = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasBitacora", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBitacora_Asunto",
                table: "AlertasBitacora",
                column: "Asunto");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBitacora_FechaCreacion",
                table: "AlertasBitacora",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBitacora_Leido",
                table: "AlertasBitacora",
                column: "Leido");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBitacora_Motivo",
                table: "AlertasBitacora",
                column: "Motivo");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBitacora_Motivo_Asunto_Leido_FechaCreacion",
                table: "AlertasBitacora",
                columns: new[] { "Motivo", "Asunto", "Leido", "FechaCreacion" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasBitacora");
        }
    }
}
