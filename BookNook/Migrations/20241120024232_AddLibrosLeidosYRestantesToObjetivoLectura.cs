using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookNook.Migrations
{
    /// <inheritdoc />
    public partial class AddLibrosLeidosYRestantesToObjetivoLectura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ObjetivoLectura",
                table: "ObjetivoLectura");

            migrationBuilder.RenameTable(
                name: "ObjetivoLectura",
                newName: "objetivos_lectura");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "objetivos_lectura",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "ProgresoAnual",
                table: "objetivos_lectura",
                newName: "progreso_anual");

            migrationBuilder.RenameColumn(
                name: "ObjetivoAnual",
                table: "objetivos_lectura",
                newName: "objetivo_anual");

            migrationBuilder.RenameColumn(
                name: "CreadoEn",
                table: "objetivos_lectura",
                newName: "creado_en");

            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "objetivos_lectura",
                newName: "año");

            migrationBuilder.RenameColumn(
                name: "ActualizadoEn",
                table: "objetivos_lectura",
                newName: "actualizado_en");

            migrationBuilder.AddPrimaryKey(
                name: "PK_objetivos_lectura",
                table: "objetivos_lectura",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_objetivos_lectura",
                table: "objetivos_lectura");

            migrationBuilder.RenameTable(
                name: "objetivos_lectura",
                newName: "ObjetivoLectura");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "ObjetivoLectura",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "progreso_anual",
                table: "ObjetivoLectura",
                newName: "ProgresoAnual");

            migrationBuilder.RenameColumn(
                name: "objetivo_anual",
                table: "ObjetivoLectura",
                newName: "ObjetivoAnual");

            migrationBuilder.RenameColumn(
                name: "creado_en",
                table: "ObjetivoLectura",
                newName: "CreadoEn");

            migrationBuilder.RenameColumn(
                name: "año",
                table: "ObjetivoLectura",
                newName: "Ano");

            migrationBuilder.RenameColumn(
                name: "actualizado_en",
                table: "ObjetivoLectura",
                newName: "ActualizadoEn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObjetivoLectura",
                table: "ObjetivoLectura",
                column: "Id");
        }
    }
}
