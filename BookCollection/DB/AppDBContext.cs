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

            modelBuilder.Entity<Book>().HasData(new Book ("Anna Karenina" , "Levas Tolstojus", "978-1-60309-502-0", 1878) { Id =1 });
            modelBuilder.Entity<Book>().HasData(new Book ("Harper Lee", "978-1-60309-505-1", "To Kill a Mockingbird", 1960) { Id = 2 });
            modelBuilder.Entity<Book>().HasData(new Book ("F. Scott Fitzgerald", "978-1-60309-344-6", "The Great Gatsby", 1925) { Id = 3 });
            modelBuilder.Entity<Book>().HasData(new Book ("Gabriel Garcia Marquez test", "978-1-60309-015-5", "One Hundred Years of Solitude", 1967) { Id = 4 });
            modelBuilder.Entity<Book>().HasData(new Book ("Ralph Ellison", "978-1-60309-300-2", "Invisible Man test", 1952) { Id = 5 });
        }
    }
}
