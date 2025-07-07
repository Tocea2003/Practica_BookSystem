using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ReviewsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.Author)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        BookId = 1, // Valoare default - nu avem BookId în noua structură
                        BookTitle = "Unknown Book", // Valoare default
                        AuthorId = r.AuthorId,
                        AuthorName = r.Author.Name,
                        UserId = 1, // Valoare default - nu avem UserId în noua structură
                        UserName = r.ReviewerName,
                        Rating = r.Rating,
                        Comment = r.Content,
                        ReviewDate = r.ReviewDate.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            try
            {
                var review = await _context.Reviews
                    .Include(r => r.Author)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (review == null)
                {
                    return NotFound();
                }

                var reviewDto = new ReviewDto
                {
                    Id = review.Id,
                    BookId = 1, // Valoare default
                    BookTitle = "Unknown Book", // Valoare default
                    AuthorId = review.AuthorId,
                    AuthorName = review.Author.Name,
                    UserId = 1, // Valoare default
                    UserName = review.ReviewerName,
                    Rating = review.Rating,
                    Comment = review.Content,
                    ReviewDate = review.ReviewDate.ToString("yyyy-MM-dd")
                };

                return Ok(reviewDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("by-author/{authorId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByAuthor(int authorId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.Author)
                    .Where(r => r.AuthorId == authorId)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        BookId = 1, // Valoare default
                        BookTitle = "Unknown Book", // Valoare default
                        AuthorId = r.AuthorId,
                        AuthorName = r.Author.Name,
                        UserId = 1, // Valoare default
                        UserName = r.ReviewerName,
                        Rating = r.Rating,
                        Comment = r.Content,
                        ReviewDate = r.ReviewDate.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto createReviewDto)
        {
            try
            {
                var review = new Review
                {
                    AuthorId = createReviewDto.AuthorId,
                    ReviewerName = "Anonymous", // Valoare default
                    Content = createReviewDto.Comment,
                    Rating = createReviewDto.Rating,
                    ReviewDate = DateTime.Now
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Returnăm review-ul cu toate datele
                var createdReview = await _context.Reviews
                    .Include(r => r.Author)
                    .FirstOrDefaultAsync(r => r.Id == review.Id);

                if (createdReview == null)
                {
                    return StatusCode(500, "Error creating review");
                }

                var reviewDto = new ReviewDto
                {
                    Id = createdReview.Id,
                    BookId = 1, // Valoare default
                    BookTitle = "Unknown Book", // Valoare default
                    AuthorId = createdReview.AuthorId,
                    AuthorName = createdReview.Author.Name,
                    UserId = 1, // Valoare default
                    UserName = createdReview.ReviewerName,
                    Rating = createdReview.Rating,
                    Comment = createdReview.Content,
                    ReviewDate = createdReview.ReviewDate.ToString("yyyy-MM-dd")
                };

                return CreatedAtAction(nameof(GetReview), new { id = review.Id }, reviewDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, UpdateReviewDto updateReviewDto)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);

                if (review == null)
                {
                    return NotFound();
                }

                if (updateReviewDto.Rating.HasValue)
                    review.Rating = updateReviewDto.Rating.Value;
                if (!string.IsNullOrEmpty(updateReviewDto.Comment))
                    review.Content = updateReviewDto.Comment;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);

                if (review == null)
                {
                    return NotFound();
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
        }
    }
}

