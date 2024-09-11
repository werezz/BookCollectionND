using BookCollection.Utility;
using System.ComponentModel.DataAnnotations;

namespace BookCollection.DTOs
{
    public record BookDto
    (
        int Id,
        string Title, 
        string Author, 
        string ISBN,   
        int PublicationYear
    );

    public record CreateBookDto
    (
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")] string Title,
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")] string Author,
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")][ISBNValidator] string ISBN,
        [Required(ErrorMessage = "{0} Must Be Filled In")][BookYearValidator] int PublicationYear
    );

    public record UpdateBookDto
  (
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")] string Title,
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")] string Author,
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} Must Be Filled In")][ISBNValidator] string ISBN,
        [Required(ErrorMessage = "{0} Must Be Filled In")][BookYearValidator] int PublicationYear
  );
}
