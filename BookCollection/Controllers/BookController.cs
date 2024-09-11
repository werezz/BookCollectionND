using BookCollection.DTOs;
using BookCollection.Models;
using BookCollection.Services.Interfaces;
using BookCollection.Utility;
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

            return Ok(cachedBookList.Select(book => book.ToBookDto()));
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

            return Ok(book.ToBookDto());
        }

        [MapToApiVersion("1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, UpdateBookDto bookDto)
        {
            var bookCheck = await _bookService.GetBookById(id);
            if (bookCheck == null)
            {
                return NotFound();
            }

            if (!await _bookService.CheckIfUpdatingTheSameBook(bookDto.ISBN, id))
            {
                return Conflict($"A book with provided ISBN already exist in another record ISBN = {bookDto.ISBN}");
            }

            await _bookService.UpdateBook(bookDto.ToBookEntity(id));

            return NoContent();
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public async Task<ActionResult> PostBook(CreateBookDto bookDto)
        {
            if (await _bookService.BookExistsByISBN(bookDto.ISBN))
            {
                return Conflict($"A book with provided ISBN already exist ISBN = {bookDto.ISBN}");
            }

            await _bookService.AddBook(bookDto.ToBookEntity());

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

            return Ok(results.Select(book => book.ToBookDto()));
        }
    }
}
