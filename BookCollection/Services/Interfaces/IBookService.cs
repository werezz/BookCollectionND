using BookCollection.Models;

namespace BookCollection.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book?> GetBookById(int id);
        Task UpdateBook(Book book);
        Task AddBook(Book book);
        Task DeleteBook(Book book);
        Task<bool> BookExistsByISBN(string? isbn);
        Task<IEnumerable<Book>> GetBooksByTerm(string term);
        bool CheckISBNFormat(string? input);
        bool CheckFutureYearFormat(int input);
    }
}
