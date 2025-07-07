using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        public DateTime FoundedDate { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        // Navigation property - One-to-Many
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
