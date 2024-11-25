using BookNook.Models;
using BookNook.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Data
{
    public class BookNookContext : DbContext
    {
        public BookNookContext(DbContextOptions<BookNookContext> options)
        : base(options)
        {
        }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<ListasLibros> ListasLibros { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<LibroEnLista> LibrosEnLista { get; set; }
        public DbSet<Lecturas> Lecturas { get; set; }
        public DbSet<ObjetivosLectura> ObjetivosLectura { get; set; }
        public DbSet<EstadoLectura> EstadoLectura { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }
        public DbSet<EtiquetaLibro> EtiquetasLibro { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseMySql("YourConnectionString",
                              new MySqlServerVersion(new Version(8, 0, 21)),
                              mySqlOptions => mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            }
        }
    }
}


