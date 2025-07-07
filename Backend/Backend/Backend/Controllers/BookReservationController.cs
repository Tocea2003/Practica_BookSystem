using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/book-reservations")]
    public class BookReservationsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookReservationsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReservationDto>>> GetBookReservations()
        {
            var reservations = await _context.BookReservations
                .Include(br => br.Book)
                .Include(br => br.User)
                .Select(br => new BookReservationDto
                {
                    Id = br.Id,
                    BookId = br.BookId,
                    BookTitle = br.Book.Title,
                    UserId = br.UserId,
                    UserName = br.User.FirstName + " " + br.User.LastName,
                    ReservationDate = br.ReservationDate.ToString("yyyy-MM-dd"),
                    DueDate = br.DueDate.HasValue ? br.DueDate.Value.ToString("yyyy-MM-dd") : null,
                    ReturnDate = br.ReturnDate.HasValue ? br.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                    Status = br.Status,
                    Fine = br.Fine
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookReservationDto>> GetBookReservation(int id)
        {
            var reservation = await _context.BookReservations
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            var reservationDto = new BookReservationDto
            {
                Id = reservation.Id,
                BookId = reservation.BookId,
                BookTitle = reservation.Book.Title,
                UserId = reservation.UserId,
                UserName = reservation.User.FirstName + " " + reservation.User.LastName,
                ReservationDate = reservation.ReservationDate.ToString("yyyy-MM-dd"),
                DueDate = reservation.DueDate.HasValue ? reservation.DueDate.Value.ToString("yyyy-MM-dd") : null,
                ReturnDate = reservation.ReturnDate.HasValue ? reservation.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                Status = reservation.Status,
                Fine = reservation.Fine
            };

            return Ok(reservationDto);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<BookReservationDto>>> GetReservationsByUser(int userId)
        {
            var reservations = await _context.BookReservations
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.UserId == userId)
                .Select(br => new BookReservationDto
                {
                    Id = br.Id,
                    BookId = br.BookId,
                    BookTitle = br.Book.Title,
                    UserId = br.UserId,
                    UserName = br.User.FirstName + " " + br.User.LastName,
                    ReservationDate = br.ReservationDate.ToString("yyyy-MM-dd"),
                    DueDate = br.DueDate.HasValue ? br.DueDate.Value.ToString("yyyy-MM-dd") : null,
                    ReturnDate = br.ReturnDate.HasValue ? br.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                    Status = br.Status,
                    Fine = br.Fine
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("by-book/{bookId}")]
        public async Task<ActionResult<IEnumerable<BookReservationDto>>> GetReservationsByBook(int bookId)
        {
            var reservations = await _context.BookReservations
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.BookId == bookId)
                .Select(br => new BookReservationDto
                {
                    Id = br.Id,
                    BookId = br.BookId,
                    BookTitle = br.Book.Title,
                    UserId = br.UserId,
                    UserName = br.User.FirstName + " " + br.User.LastName,
                    ReservationDate = br.ReservationDate.ToString("yyyy-MM-dd"),
                    DueDate = br.DueDate.HasValue ? br.DueDate.Value.ToString("yyyy-MM-dd") : null,
                    ReturnDate = br.ReturnDate.HasValue ? br.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                    Status = br.Status,
                    Fine = br.Fine
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpPost]
        public async Task<ActionResult<BookReservationDto>> CreateBookReservation(CreateBookReservationDto createReservationDto)
        {
            var reservation = new BookReservation
            {
                BookId = createReservationDto.BookId,
                UserId = createReservationDto.UserId,
                ReservationDate = DateTime.Now,
                DueDate = !string.IsNullOrEmpty(createReservationDto.DueDate) ? DateTime.Parse(createReservationDto.DueDate) : null,
                Status = createReservationDto.Status
            };

            _context.BookReservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Returnăm rezervarea cu toate datele
            var createdReservation = await _context.BookReservations
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.Id == reservation.Id);

            var reservationDto = new BookReservationDto
            {
                Id = createdReservation.Id,
                BookId = createdReservation.BookId,
                BookTitle = createdReservation.Book.Title,
                UserId = createdReservation.UserId,
                UserName = createdReservation.User.FirstName + " " + createdReservation.User.LastName,
                ReservationDate = createdReservation.ReservationDate.ToString("yyyy-MM-dd"),
                DueDate = createdReservation.DueDate.HasValue ? createdReservation.DueDate.Value.ToString("yyyy-MM-dd") : null,
                ReturnDate = createdReservation.ReturnDate.HasValue ? createdReservation.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                Status = createdReservation.Status,
                Fine = createdReservation.Fine
            };

            return CreatedAtAction(nameof(GetBookReservation), new { id = reservation.Id }, reservationDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookReservation(int id, UpdateBookReservationDto updateReservationDto)
        {
            var reservation = await _context.BookReservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateReservationDto.DueDate))
                reservation.DueDate = DateTime.Parse(updateReservationDto.DueDate);
            if (!string.IsNullOrEmpty(updateReservationDto.ReturnDate))
                reservation.ReturnDate = DateTime.Parse(updateReservationDto.ReturnDate);
            if (!string.IsNullOrEmpty(updateReservationDto.Status))
                reservation.Status = updateReservationDto.Status;
            if (updateReservationDto.Fine.HasValue)
                reservation.Fine = updateReservationDto.Fine.Value;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookReservation(int id)
        {
            var reservation = await _context.BookReservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.BookReservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var reservation = await _context.BookReservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            reservation.ReturnDate = DateTime.Now;
            reservation.Status = "Returned";

            // Calculează amenda dacă e întârziat
            if (reservation.DueDate.HasValue && DateTime.Now > reservation.DueDate.Value)
            {
                var daysLate = (DateTime.Now - reservation.DueDate.Value).Days;
                reservation.Fine = daysLate * 1.0m; // 1 RON per zi întârziere
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

