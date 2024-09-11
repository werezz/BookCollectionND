using BookCollection.DTOs;
using BookCollection.Models;

namespace BookCollection.Utility
{
    public static class ModelExtensions
    {
        public static BookDto ToBookDto(this Book book)
        {
            return new BookDto(
                book.Id,
                book.Title,
                book.Author,
                book.ISBN,
                book.PublicationYear
                );
        }

        public static Book ToBookEntity(this CreateBookDto bookDto)
        {
            return new Book(bookDto.Title, bookDto.Author, bookDto.ISBN, bookDto.PublicationYear);
        }

        public static Book ToBookEntity(this UpdateBookDto bookDto, int id)
        {
            return new Book(id, bookDto.Title, bookDto.Author, bookDto.ISBN, bookDto.PublicationYear);
        }
    }
}
