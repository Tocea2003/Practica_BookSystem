using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Review
    {
        public int Id { get; set; }

        // Folosim coloanele care există în baza de date
        [Required]
        [MaxLength(100)]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime ReviewDate { get; set; }

        // Foreign key care există în baza de date
        public int AuthorId { get; set; }

        // Navigation property
        public Author Author { get; set; } = null!;
    }
}


