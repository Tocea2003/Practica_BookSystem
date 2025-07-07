using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Biography = a.Biography,
                    BirthDate = a.BirthDate.ToString("yyyy-MM-dd"),
                    Nationality = a.Nationality
                })
                .ToListAsync();

            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate.ToString("yyyy-MM-dd"),
                Nationality = author.Nationality
            };

            return Ok(authorDto);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            var author = new Author
            {
                Name = createAuthorDto.Name,
                Biography = createAuthorDto.Biography,
                BirthDate = DateTime.Parse(createAuthorDto.BirthDate),
                Nationality = createAuthorDto.Nationality
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate.ToString("yyyy-MM-dd"),
                Nationality = author.Nationality
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateAuthorDto.Name))
                author.Name = updateAuthorDto.Name;
            if (!string.IsNullOrEmpty(updateAuthorDto.Biography))
                author.Biography = updateAuthorDto.Biography;
            if (!string.IsNullOrEmpty(updateAuthorDto.BirthDate))
                author.BirthDate = DateTime.Parse(updateAuthorDto.BirthDate);
            if (!string.IsNullOrEmpty(updateAuthorDto.Nationality))
                author.Nationality = updateAuthorDto.Nationality;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            if (author.Books.Any())
            {
                return BadRequest("Nu se poate șterge autorul. Are cărți asociate.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
