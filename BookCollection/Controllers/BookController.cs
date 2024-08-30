using BookCollection.Models;
using BookCollection.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookCollection.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMemoryCache _cache;
        private const string key = "bookList";

        public BookController(IBookService bookService, IMemoryCache cache)
        {
            _bookService = bookService;
            _cache = cache;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            var cachedBookList = await _cache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                    return await _bookService.GetAllBooks();
                });

            if (!cachedBookList.Any() || cachedBookList == null)
            {
                return NotFound();
            }

            return Ok(cachedBookList);
        }

        [MapToApiVersion("1")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [MapToApiVersion("1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            var bookCheck = await _bookService.GetBookById(id);
            if (bookCheck == null)
            {
                return NotFound();
            }

            if (id != book.Id)
            {
                return BadRequest($"Book with id = {id} does not match book to update BookId = {book.Id}");
            }

            if (string.IsNullOrEmpty(book.Title))
            {
                return BadRequest($"The provided Title cannot be null or empty");
            }

            if (string.IsNullOrEmpty(book.Auhtor))
            {
                return BadRequest($"The provided Auhtor cannot be null or empty");
            }

            if (!_bookService.CheckISBNFormat(book.ISBN))
            {
                return BadRequest($"The provided ISBN is in bad format, ISBN = {book.ISBN}");
            }

            if (!_bookService.CheckFutureYearFormat(book.PublicationYear))
            {
                return BadRequest($"The provided PublicationYear is in the future, PublicationYear = {book.PublicationYear}");
            }

            await _bookService.UpdateBook(book);

            return NoContent();
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (await _bookService.BookExistsByISBN(book.ISBN))
            {
                return Conflict($"A book with provided ISBN already exist ISBN = {book.ISBN}");
            }

            if (!_bookService.CheckISBNFormat(book.ISBN))
            {
                return BadRequest($"The provided ISBN is in bad format, ISBN = {book.ISBN}");
            }

            if (!_bookService.CheckFutureYearFormat(book.PublicationYear))
            {
                return BadRequest($"The provided PublicationYear is in the future, PublicationYear = {book.PublicationYear}");
            }

            if (string.IsNullOrEmpty(book.Title))
            {
                return BadRequest($"The provided Title cannot be null or empty");
            }

            if (string.IsNullOrEmpty(book.Auhtor))
            {
                return BadRequest($"The provided Auhtor cannot be null or empty");
            }

            await _bookService.AddBook(book);

            return Created();
        }

        [MapToApiVersion("1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBook(book);

            return NoContent();
        }

        [MapToApiVersion("1")]
        [HttpPost("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string query)
        {
            var results = await _bookService.GetBooksByTerm(query);

            if (!results.Any()) 
            { 
                return NoContent();
            }

            return Ok(results);
        }
    }
}
