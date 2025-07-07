using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Categories)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.Name,
                    ISBN = b.ISBN,
                    PublishedDate = b.PublishedDate.ToString("yyyy-MM-dd"),
                    Genre = b.Genre,
                    Description = b.Description,
                    Pages = b.Pages,
                    Price = b.Price,
                    PublisherId = b.PublisherId,
                    PublisherName = b.Publisher != null ? b.Publisher.Name : null,
                    Categories = b.Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate.ToString("yyyy-MM-dd"),
                Genre = book.Genre,
                Description = book.Description,
                Pages = book.Pages,
                Price = book.Price,
                PublisherId = book.PublisherId,
                PublisherName = book.Publisher?.Name,
                Categories = book.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList()
            };

            return Ok(bookDto);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            var book = new Book
            {
                Title = createBookDto.Title,
                AuthorId = createBookDto.AuthorId,
                ISBN = createBookDto.ISBN,
                PublishedDate = DateTime.Parse(createBookDto.PublishedDate),
                Genre = createBookDto.Genre,
                Description = createBookDto.Description,
                Pages = createBookDto.Pages,
                Price = createBookDto.Price,
                PublisherId = createBookDto.PublisherId
            };

            // Adăugăm categoriile
            if (createBookDto.CategoryIds != null && createBookDto.CategoryIds.Any())
            {
                var categories = await _context.Categories
                    .Where(c => createBookDto.CategoryIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var category in categories)
                {
                    book.Categories.Add(category);
                }
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Returnăm cartea cu toate datele
            var createdBook = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == book.Id);

            if (createdBook == null)
            {
                return StatusCode(500, "Failed to retrieve created book");
            }

            var bookDto = new BookDto
            {
                Id = createdBook.Id,
                Title = createdBook.Title,
                AuthorId = createdBook.AuthorId,
                AuthorName = createdBook.Author.Name,
                ISBN = createdBook.ISBN,
                PublishedDate = createdBook.PublishedDate.ToString("yyyy-MM-dd"),
                Genre = createdBook.Genre,
                Description = createdBook.Description,
                Pages = createdBook.Pages,
                Price = createdBook.Price,
                PublisherId = createdBook.PublisherId,
                PublisherName = createdBook.Publisher?.Name,
                Categories = createdBook.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList()
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            var book = await _context.Books.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateBookDto.Title))
                book.Title = updateBookDto.Title;
            if (updateBookDto.AuthorId.HasValue)
                book.AuthorId = updateBookDto.AuthorId.Value;
            if (!string.IsNullOrEmpty(updateBookDto.ISBN))
                book.ISBN = updateBookDto.ISBN;
            if (!string.IsNullOrEmpty(updateBookDto.PublishedDate))
                book.PublishedDate = DateTime.Parse(updateBookDto.PublishedDate);
            if (!string.IsNullOrEmpty(updateBookDto.Genre))
                book.Genre = updateBookDto.Genre;
            if (!string.IsNullOrEmpty(updateBookDto.Description))
                book.Description = updateBookDto.Description;
            if (updateBookDto.Pages.HasValue)
                book.Pages = updateBookDto.Pages.Value;
            if (updateBookDto.Price.HasValue)
                book.Price = updateBookDto.Price.Value;
            if (updateBookDto.PublisherId.HasValue)
                book.PublisherId = updateBookDto.PublisherId.Value;

            // Actualizăm categoriile
            if (updateBookDto.CategoryIds != null)
            {
                book.Categories.Clear();
                if (updateBookDto.CategoryIds.Any())
                {
                    var categories = await _context.Categories
                        .Where(c => updateBookDto.CategoryIds.Contains(c.Id))
                        .ToListAsync();

                    foreach (var category in categories)
                    {
                        book.Categories.Add(category);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookReservations)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // Check if the book has any reservations
            if (book.BookReservations.Any())
            {
                return BadRequest("Nu se poate șterge cartea. Există rezervări asociate cu această carte.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("by-author/{authorId}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByAuthor(int authorId)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Categories)
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.Name,
                    ISBN = b.ISBN,
                    PublishedDate = b.PublishedDate.ToString("yyyy-MM-dd"),
                    Genre = b.Genre,
                    Description = b.Description,
                    Pages = b.Pages,
                    Price = b.Price,
                    PublisherId = b.PublisherId,
                    PublisherName = b.Publisher != null ? b.Publisher.Name : null,
                    Categories = b.Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByCategory(int categoryId)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Categories)
                .Where(b => b.Categories.Any(c => c.Id == categoryId))
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.Name,
                    ISBN = b.ISBN,
                    PublishedDate = b.PublishedDate.ToString("yyyy-MM-dd"),
                    Genre = b.Genre,
                    Description = b.Description,
                    Pages = b.Pages,
                    Price = b.Price,
                    PublisherId = b.PublisherId,
                    PublisherName = b.Publisher != null ? b.Publisher.Name : null,
                    Categories = b.Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }
    }
}
