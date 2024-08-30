using BookCollection.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCollection.DB
{
    public class AppDBContext() : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase("TestBook");
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
            .HasKey(p => new { p.Id });

            modelBuilder.Entity<Book>().HasData(new Book { Id = 1, Auhtor = "Levas Tolstojus", ISBN = "978-1-60309-502-0", PublicationYear = 1878, Title = "Anna Karenina" });
            modelBuilder.Entity<Book>().HasData(new Book { Id = 2, Auhtor = "Harper Lee", ISBN = "978-1-60309-505-1", PublicationYear = 1960, Title = "To Kill a Mockingbird" });
            modelBuilder.Entity<Book>().HasData(new Book { Id = 3, Auhtor = "F. Scott Fitzgerald", ISBN = "978-1-60309-344-6", PublicationYear = 1925, Title = "The Great Gatsby" });
            modelBuilder.Entity<Book>().HasData(new Book { Id = 4, Auhtor = "Gabriel Garcia Marquez test", ISBN = "978-1-60309-015-5", PublicationYear = 1967, Title = "One Hundred Years of Solitude" });
            modelBuilder.Entity<Book>().HasData(new Book { Id = 5, Auhtor = "Ralph Ellison", ISBN = "978-1-60309-300-2", PublicationYear = 1952, Title = "Invisible Man test" });
        }
    }
}
