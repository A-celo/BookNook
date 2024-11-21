using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookNook.Migrations
{
    /// <inheritdoc />
    public partial class AddObjetivoLecturaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoLectura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoLectura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EtiquetasLibro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    EtiquetaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtiquetasLibro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormatosLibro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatosLibro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lecturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    libro_id = table.Column<int>(type: "int", nullable: false),
                    formato_id = table.Column<int>(type: "int", nullable: false),
                    estado_id = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pagina_actual = table.Column<int>(type: "int", nullable: true),
                    tiempo_actual = table.Column<int>(type: "int", nullable: true),
                    tiempo_lectura = table.Column<int>(type: "int", nullable: true),
                    Calificacion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actualizado_en = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AnoPublicacion = table.Column<int>(type: "int", nullable: false),
                    Idioma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Subgenero = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NumeroPaginas = table.Column<int>(type: "int", nullable: false),
                    DuracionAudio = table.Column<int>(type: "int", nullable: false),
                    ImagenPortada = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Sinopsis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibrosEnLista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListaId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    FormatoId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrosEnLista", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListasLibros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    NombreLista = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasLibros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjetivoLectura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    ObjetivoAnual = table.Column<int>(type: "int", nullable: false),
                    ProgresoAnual = table.Column<int>(type: "int", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LibrosLeidos = table.Column<int>(type: "int", nullable: false),
                    LibrosRestantes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivoLectura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoLectura");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "EtiquetasLibro");

            migrationBuilder.DropTable(
                name: "FormatosLibro");

            migrationBuilder.DropTable(
                name: "Lecturas");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "LibrosEnLista");

            migrationBuilder.DropTable(
                name: "ListasLibros");

            migrationBuilder.DropTable(
                name: "ObjetivoLectura");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
