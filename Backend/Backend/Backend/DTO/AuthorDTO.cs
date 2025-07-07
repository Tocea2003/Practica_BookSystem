namespace Backend.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
    }

    public class CreateAuthorDto
    {
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
    }

    public class UpdateAuthorDto
    {
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? BirthDate { get; set; }
        public string? Nationality { get; set; }
    }
}
