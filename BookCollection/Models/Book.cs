using BookCollection.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCollection.Models
{
    public class Book
    {
        public Book() { }

        public Book(int id, string title, string author, string isbn, int publicationYear)
        {
            Id = id;
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
        }
        public Book(string title, string author, string isbn, int publicationYear)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")]
        public string Author { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")]
        [ISBNValidator]
        public string ISBN { get; set; }

        [Required]
        [BookYearValidator]
        public int PublicationYear { get; set; }
    }
}
