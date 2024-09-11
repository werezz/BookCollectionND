using BookCollection.Controllers;
using BookCollection.Models;
using BookCollection.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace BookCollectionTests
{
    [TestFixture]
    public class BookControllerTests : IDisposable
    {

        private readonly IBookService _bookService;
        private readonly BookController _bookController;
        private readonly IMemoryCache _memoryCache;
        public BookControllerTests()
        {
            _bookService = Substitute.For<IBookService>();
            _memoryCache = Substitute.For<IMemoryCache>();
            _bookController = new BookController(_bookService, _memoryCache);
        }

        [TearDown]
        public void Dispose() { }

        [Test]
        public async Task GetBooks_ReturnsNotFound_WhenNoItemsAreFound()
        {
            _bookService.GetAllBooks().Returns(Task.FromResult(Enumerable.Empty<Book>()));

            var result = await _bookController.GetBooks();
            var notFoundObjectResult = new NotFoundObjectResult(result);

            Assert.That(notFoundObjectResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task GetBooks_ReturnsOk_WhenItemsAreFound()
        {
            IEnumerable<Book> bookList = [new Book ("Anna Karenina", "Levas Tolstojus", "978-1-60309-502-0", 1878)];

            _bookService.GetAllBooks().Returns(Task.FromResult(bookList));

            var result = await _bookController.GetBooks();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteBook_ReturnsNotFound_WhenNoResourceToDelete()
        {
            var result = await _bookController.DeleteBook(1);
            var notFoundObjectResult = new NotFoundObjectResult(result);

            Assert.That(notFoundObjectResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task DeleteBook_ReturnsNoContent_WhenResourceIsDeleted()
        {
            _bookService.GetBookById(1).Returns(new Book ("Levas Tolstojus", "978-1-60309-502-0", "Anna Karenina", 1878));

            var result = await _bookController.DeleteBook(1);
            var okObjectResult = new OkObjectResult(result);

            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));
        }
    }
}