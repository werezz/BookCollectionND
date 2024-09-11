using BookCollection.DB;
using BookCollection.Models;
using BookCollection.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookCollection.Services
{
    public class BookService : IBookService
    {
        private readonly AppDBContext _context;

        public BookService(AppDBContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<IEnumerable<Book>> GetBooksByTerm(string term)
        {
            return await _context.Books.Where(e => e.Author.Contains(term) || e.Title.Contains(term)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public Task<bool> BookExistsByISBN(string? isbn)
        {
            return _context.Books.AnyAsync(e => e.ISBN == isbn);
        }

        public Task<bool> CheckIfUpdatingTheSameBook(string? isbn, int id)
        {
            return _context.Books.AnyAsync(e => e.ISBN == isbn && e.Id == id);
        }
    }
}
