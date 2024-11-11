using BookNook.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookNook.Data
{
    public class BookNookContext : DbContext
    {
        public BookNookContext(DbContextOptions<BookNookContext> options) : base(options) { }

        public DbSet<Usuarios> Usuarios { get; set; }
    }
}

