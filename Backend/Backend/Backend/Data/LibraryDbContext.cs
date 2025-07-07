using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookReservation> BookReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Author-Book relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Publisher-Book relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Book-Category Many-to-Many relationship
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCategory"));

            // Configure BookReservation relationships
            modelBuilder.Entity<BookReservation>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BookReservations)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookReservation>()
                .HasOne(br => br.User)
                .WithMany(u => u.BookReservations)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Review relationships - doar cu Author
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Author)
                .WithMany()
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data pentru Authors
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "J.K. Rowling",
                    Biography = "Autoare britanică, cunoscută pentru seria Harry Potter",
                    BirthDate = new DateTime(1965, 7, 31),
                    Nationality = "Britanică"
                },
                new Author
                {
                    Id = 2,
                    Name = "George R.R. Martin",
                    Biography = "Scriitor american, cunoscut pentru A Song of Ice and Fire",
                    BirthDate = new DateTime(1948, 9, 20),
                    Nationality = "Americană"
                },
                new Author
                {
                    Id = 3,
                    Name = "J.R.R. Tolkien",
                    Biography = "Scriitor britanic, autorul trilogiei The Lord of the Rings",
                    BirthDate = new DateTime(1892, 1, 3),
                    Nationality = "Britanică"
                }
            );

            // Seed data pentru Publishers
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher
                {
                    Id = 1,
                    Name = "Bloomsbury Publishing",
                    Address = "50 Bedford Square, London",
                    Country = "United Kingdom",
                    FoundedDate = new DateTime(1986, 1, 1),
                    Phone = "+44 20 7631 5600",
                    Email = "info@bloomsbury.com"
                },
                new Publisher
                {
                    Id = 2,
                    Name = "Bantam Books",
                    Address = "1745 Broadway, New York",
                    Country = "United States",
                    FoundedDate = new DateTime(1945, 1, 1),
                    Phone = "+1 212 782 9000",
                    Email = "info@bantam.com"
                }
            );

            // Seed data pentru Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fantasy", Description = "Fantasy literature" },
                new Category { Id = 2, Name = "Adventure", Description = "Adventure stories" },
                new Category { Id = 3, Name = "Young Adult", Description = "Books for young adults" },
                new Category { Id = 4, Name = "Epic Fantasy", Description = "Epic fantasy novels" },
                new Category { Id = 5, Name = "Classic Literature", Description = "Classic literary works" }
            );

            // Seed data pentru Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@library.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Phone = "0123456789",
                    JoinDate = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 2,
                    Email = "user1@email.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "0987654321",
                    JoinDate = new DateTime(2024, 6, 1)
                }
            );

            // Seed data pentru Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Harry Potter and the Philosopher's Stone",
                    AuthorId = 1,
                    PublisherId = 1,
                    ISBN = "978-0-7475-3269-9",
                    PublishedDate = new DateTime(1997, 6, 26),
                    Genre = "Fantasy",
                    Description = "Primul roman din seria Harry Potter",
                    Pages = 223,
                    Price = 45.99m
                },
                new Book
                {
                    Id = 2,
                    Title = "A Game of Thrones",
                    AuthorId = 2,
                    PublisherId = 2,
                    ISBN = "978-0-553-10354-0",
                    PublishedDate = new DateTime(1996, 8, 1),
                    Genre = "Fantasy",
                    Description = "Primul roman din seria A Song of Ice and Fire",
                    Pages = 694,
                    Price = 67.5m
                },
                new Book
                {
                    Id = 3,
                    Title = "The Fellowship of the Ring",
                    AuthorId = 3,
                    PublisherId = 1,
                    ISBN = "978-0-547-92822-7",
                    PublishedDate = new DateTime(1954, 7, 29),
                    Genre = "Fantasy",
                    Description = "Primul volum din trilogia The Lord of the Rings",
                    Pages = 423,
                    Price = 55.99m
                }
            );

            // Seed data pentru Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    AuthorId = 1,
                    ReviewerName = "John Doe",
                    Content = "O carte fantastică! Recomandata pentru toate vârstele.",
                    Rating = 5,
                    ReviewDate = new DateTime(2024, 6, 15)
                },
                new Review
                {
                    Id = 2,
                    AuthorId = 2,
                    ReviewerName = "Literary Critic",
                    Content = "Un început excelent pentru seria A Song of Ice and Fire.",
                    Rating = 4,
                    ReviewDate = new DateTime(2024, 6, 20)
                }
            );

            // Seed data pentru BookReservations
            modelBuilder.Entity<BookReservation>().HasData(
                new BookReservation
                {
                    Id = 1,
                    BookId = 1,
                    UserId = 2,
                    ReservationDate = new DateTime(2024, 7, 1),
                    DueDate = new DateTime(2024, 7, 15),
                    Status = "Reserved"
                },
                new BookReservation
                {
                    Id = 2,
                    BookId = 3,
                    UserId = 2,
                    ReservationDate = new DateTime(2024, 6, 25),
                    DueDate = new DateTime(2024, 7, 10),
                    Status = "Borrowed"
                }
            );
        }
    }
}

