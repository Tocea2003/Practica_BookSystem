using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        public int AuthorId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; }

        [MaxLength(100)]
        public string Genre { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public int Pages { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Foreign key pentru Publisher
        public int? PublisherId { get; set; }

        // Navigation properties
        public Author Author { get; set; } = null!;
        public Publisher? Publisher { get; set; } // One-to-Many
        public ICollection<Category> Categories { get; set; } = new List<Category>(); // Many-to-Many
        public ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>(); // Many-to-Many through junction
                                                                                                          // În clasa Book, adaugă această proprietate:
        public ICollection<Review> Reviews { get; set; } = new List<Review>();


    }
}
