// services/api.ts
const API_BASE_URL = "http://localhost:5219/api";

export interface Author {
  id: number;
  name: string;
  biography: string;
  birthDate: string;
  nationality: string;
}
export interface Review {
  id: number;
  bookId: number;
  bookTitle: string;
  authorId: number;
  authorName: string;
  userId: number;
  userName: string;
  rating: number;
  comment: string;
  reviewDate: string;
}

export interface CreateReviewDto {
  bookId: number;
  authorId: number;
  userId: number;
  rating: number;
  comment: string;
}

export interface CreateBookReservationDto {
  bookId: number;
  userId: number;
  reservationDate: string;
  dueDate?: string;
}
export interface Book {
  id: number;
  title: string;
  authorId: number;
  authorName?: string;
  isbn: string;
  publishedDate: string;
  genre: string;
  description: string;
  pages: number;
  price: number;
  publisherId?: number;
  publisherName?: string;
  categories: Category[];
}

export interface Category {
  id: number;
  name: string;
  description: string;
}

export interface Publisher {
  id: number;
  name: string;
  address: string;
  country: string;
  foundedDate: string;
  phone: string;
  email: string;
}

export interface User {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  address: string;
  registrationDate: string;
  role: string;
}

export interface BookReservation {
  id: number;
  bookId: number;
  bookTitle: string;
  userId: number;
  userName: string;
  reservationDate: string;
  dueDate?: string;
  returnDate?: string;
  status: string;
  fine?: number;
}

// Create DTOs
export interface CreateAuthorDto {
  name: string;
  biography: string;
  birthDate: string;
  nationality: string;
}

export interface CreateBookDto {
  title: string;
  authorId: number;
  isbn: string;
  publishedDate: string;
  genre: string;
  description: string;
  pages: number;
  price: number;
  publisherId?: number;
  categoryIds: number[];
}

export interface CreateCategoryDto {
  name: string;
  description: string;
}

export interface CreatePublisherDto {
  name: string;
  address: string;
  country: string;
  foundedDate: string;
  phone: string;
  email: string;
}

export interface CreateUserDto {
  username: string;
  password: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  address: string;
  role: string;
}

class ApiService {
  private async request<T>(url: string, options?: RequestInit): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${url}`, {
      headers: {
        "Content-Type": "application/json",
        ...options?.headers,
      },
      ...options,
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || `HTTP error! status: ${response.status}`);
    }

    // Handle empty responses (like NoContent 204)
    if (
      response.status === 204 ||
      response.headers.get("content-length") === "0"
    ) {
      return {} as T;
    }

    const text = await response.text();
    return text ? JSON.parse(text) : ({} as T);
  }

  // Authors API
  async getAuthors(): Promise<Author[]> {
    return this.request<Author[]>("/authors");
  }

  async getAuthor(id: number): Promise<Author> {
    return this.request<Author>(`/authors/${id}`);
  }

  async createAuthor(author: CreateAuthorDto): Promise<Author> {
    return this.request<Author>("/authors", {
      method: "POST",
      body: JSON.stringify(author),
    });
  }

  async updateAuthor(
    id: number,
    author: Partial<CreateAuthorDto>
  ): Promise<void> {
    await this.request<void>(`/authors/${id}`, {
      method: "PUT",
      body: JSON.stringify(author),
    });
  }

  async deleteAuthor(id: number): Promise<void> {
    await this.request<void>(`/authors/${id}`, {
      method: "DELETE",
    });
  }

  // Books API
  async getBooks(): Promise<Book[]> {
    return this.request<Book[]>("/books");
  }

  async getBook(id: number): Promise<Book> {
    return this.request<Book>(`/books/${id}`);
  }

  async getBooksByAuthor(authorId: number): Promise<Book[]> {
    return this.request<Book[]>(`/books/by-author/${authorId}`);
  }

  async getBooksByCategory(categoryId: number): Promise<Book[]> {
    return this.request<Book[]>(`/books/by-category/${categoryId}`);
  }

  async createBook(book: CreateBookDto): Promise<Book> {
    return this.request<Book>("/books", {
      method: "POST",
      body: JSON.stringify(book),
    });
  }

  async updateBook(id: number, book: Partial<CreateBookDto>): Promise<void> {
    await this.request<void>(`/books/${id}`, {
      method: "PUT",
      body: JSON.stringify(book),
    });
  }

  async deleteBook(id: number): Promise<void> {
    await this.request<void>(`/books/${id}`, {
      method: "DELETE",
    });
  }

  // Categories API
  async getCategories(): Promise<Category[]> {
    return this.request<Category[]>("/categories");
  }

  async getCategory(id: number): Promise<Category> {
    return this.request<Category>(`/categories/${id}`);
  }

  async createCategory(category: CreateCategoryDto): Promise<Category> {
    return this.request<Category>("/categories", {
      method: "POST",
      body: JSON.stringify(category),
    });
  }

  async updateCategory(
    id: number,
    category: Partial<CreateCategoryDto>
  ): Promise<void> {
    await this.request<void>(`/categories/${id}`, {
      method: "PUT",
      body: JSON.stringify(category),
    });
  }

  async deleteCategory(id: number): Promise<void> {
    await this.request<void>(`/categories/${id}`, {
      method: "DELETE",
    });
  }

  // Publishers API
  async getPublishers(): Promise<Publisher[]> {
    return this.request<Publisher[]>("/publishers");
  }

  async getPublisher(id: number): Promise<Publisher> {
    return this.request<Publisher>(`/publishers/${id}`);
  }

  async createPublisher(publisher: CreatePublisherDto): Promise<Publisher> {
    return this.request<Publisher>("/publishers", {
      method: "POST",
      body: JSON.stringify(publisher),
    });
  }

  async updatePublisher(
    id: number,
    publisher: Partial<CreatePublisherDto>
  ): Promise<void> {
    await this.request<void>(`/publishers/${id}`, {
      method: "PUT",
      body: JSON.stringify(publisher),
    });
  }

  async deletePublisher(id: number): Promise<void> {
    await this.request<void>(`/publishers/${id}`, {
      method: "DELETE",
    });
  }

  // Users API
  async getUsers(): Promise<User[]> {
    return this.request<User[]>("/users");
  }

  async getUser(id: number): Promise<User> {
    return this.request<User>(`/users/${id}`);
  }

  async createUser(user: CreateUserDto): Promise<User> {
    return this.request<User>("/users", {
      method: "POST",
      body: JSON.stringify(user),
    });
  }

  async updateUser(id: number, user: Partial<CreateUserDto>): Promise<void> {
    await this.request<void>(`/users/${id}`, {
      method: "PUT",
      body: JSON.stringify(user),
    });
  }

  async deleteUser(id: number): Promise<void> {
    await this.request<void>(`/users/${id}`, {
      method: "DELETE",
    });
  }

  // Reviews API
  async getReviews(): Promise<Review[]> {
    return this.request<Review[]>("/reviews");
  }

  async createReview(review: CreateReviewDto): Promise<Review> {
    return this.request<Review>("/reviews", {
      method: "POST",
      body: JSON.stringify(review),
    });
  }

  // Book Reservations API
  async getBookReservations(): Promise<BookReservation[]> {
    return this.request<BookReservation[]>("/book-reservations");
  }

  async createBookReservation(
    reservation: CreateBookReservationDto
  ): Promise<BookReservation> {
    return this.request<BookReservation>("/book-reservations", {
      method: "POST",
      body: JSON.stringify(reservation),
    });
  }
}

export const apiService = new ApiService();
