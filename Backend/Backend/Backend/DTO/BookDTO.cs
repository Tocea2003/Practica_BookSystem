namespace Backend.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Pages { get; set; }
        public decimal Price { get; set; }

        // Proprietăți noi
        public int? PublisherId { get; set; }
        public string? PublisherName { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }

    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Pages { get; set; }
        public decimal Price { get; set; }

        // Proprietăți noi
        public int? PublisherId { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }

    public class UpdateBookDto
    {
        public string? Title { get; set; }
        public int? AuthorId { get; set; }
        public string? ISBN { get; set; }
        public string? PublishedDate { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public int? Pages { get; set; }
        public decimal? Price { get; set; }

        // Proprietăți noi
        public int? PublisherId { get; set; }
        public List<int>? CategoryIds { get; set; }
    }

    // DTO pentru Category
  
}

