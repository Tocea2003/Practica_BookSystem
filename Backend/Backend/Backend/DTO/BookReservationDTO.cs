namespace Backend.DTOs
{
    public class BookReservationDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ReservationDate { get; set; } = string.Empty;
        public string? DueDate { get; set; }
        public string? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? Fine { get; set; }
    }

    public class CreateBookReservationDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string? DueDate { get; set; }
        public string Status { get; set; } = "Reserved";
    }

    public class UpdateBookReservationDto
    {
        public string? DueDate { get; set; }
        public string? ReturnDate { get; set; }
        public string? Status { get; set; }
        public decimal? Fine { get; set; }
    }
}
