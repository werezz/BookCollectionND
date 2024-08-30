using BookCollection.DB;
using BookCollection.Models;
using BookCollection.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BookCollection.Services
{
    public class BookService : IBookService
    {
        private readonly AppDBContext _context;

        public BookService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksByTerm(string term)
        {
            return await _context.Books.Where(e => e.Auhtor.Contains(term) || e.Title.Contains(term)).ToListAsync();
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

        public bool CheckISBNFormat(string? input)
        {
            if (CheckISBNLenght(input) && CheckForLetters(input)) return true;
            return false;
        }

        public bool CheckFutureYearFormat(int input)
        {
            DateTime localDate = DateTime.Now;
            if (localDate.Year > input) return true;
            return false;
        }
        public Task<bool> BookExistsByISBN(string? isbn)
        {
            return _context.Books.AnyAsync(e => e.ISBN == isbn);
        }

        private bool CheckISBNLenght(string? input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            var ISBNStripped = Regex.Replace(input, "[^0-9]", "");

            if (ISBNStripped.Length == 13) return true;

            return false;
        }

        private bool CheckForLetters(string? input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            if (input.ToCharArray().All(c => char.IsLetter(c)) == false) return true;
            return false;
        }
    }
}
