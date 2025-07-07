using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class BookReservation
    {
        public int Id { get; set; }

        // Foreign keys
        public int BookId { get; set; }
        public int UserId { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Reserved"; // Reserved, Borrowed, Returned

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Fine { get; set; }

        // Navigation properties
        public Book Book { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
