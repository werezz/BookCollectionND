using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCollection.Models
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Auhtor { get; set; }

        [Required]
        public string? ISBN { get; set; }

        [Required]
        public int PublicationYear { get; set; }

    }
}
