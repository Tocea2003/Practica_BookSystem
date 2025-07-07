using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public PublishersController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers()
        {
            var publishers = await _context.Publishers
                .Select(p => new PublisherDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    Country = p.Country,
                    FoundedDate = p.FoundedDate.ToString("yyyy-MM-dd"),
                    Phone = p.Phone,
                    Email = p.Email
                })
                .ToListAsync();

            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            var publisherDto = new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Country = publisher.Country,
                FoundedDate = publisher.FoundedDate.ToString("yyyy-MM-dd"),
                Phone = publisher.Phone,
                Email = publisher.Email
            };

            return Ok(publisherDto);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher(CreatePublisherDto createPublisherDto)
        {
            var publisher = new Publisher
            {
                Name = createPublisherDto.Name,
                Address = createPublisherDto.Address,
                Country = createPublisherDto.Country,
                FoundedDate = DateTime.Parse(createPublisherDto.FoundedDate),
                Phone = createPublisherDto.Phone,
                Email = createPublisherDto.Email
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            var publisherDto = new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Country = publisher.Country,
                FoundedDate = publisher.FoundedDate.ToString("yyyy-MM-dd"),
                Phone = publisher.Phone,
                Email = publisher.Email
            };

            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, publisherDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, UpdatePublisherDto updatePublisherDto)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updatePublisherDto.Name))
                publisher.Name = updatePublisherDto.Name;
            if (!string.IsNullOrEmpty(updatePublisherDto.Address))
                publisher.Address = updatePublisherDto.Address;
            if (!string.IsNullOrEmpty(updatePublisherDto.Country))
                publisher.Country = updatePublisherDto.Country;
            if (!string.IsNullOrEmpty(updatePublisherDto.FoundedDate))
                publisher.FoundedDate = DateTime.Parse(updatePublisherDto.FoundedDate);
            if (!string.IsNullOrEmpty(updatePublisherDto.Phone))
                publisher.Phone = updatePublisherDto.Phone;
            if (!string.IsNullOrEmpty(updatePublisherDto.Email))
                publisher.Email = updatePublisherDto.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.Include(p => p.Books).FirstOrDefaultAsync(p => p.Id == id);

            if (publisher == null)
            {
                return NotFound();
            }

            if (publisher.Books.Any())
            {
                return BadRequest("Nu se poate șterge editorul. Are cărți asociate.");
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
