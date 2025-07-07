// stores/library.ts
import { defineStore } from "pinia";
import { ref, computed } from "vue";
import {
  apiService,
  type Author,
  type Book,
  type Category,
  type Publisher,
  type User,
  type Review,
  type BookReservation,
  type CreateAuthorDto,
  type CreateBookDto,
  type CreateCategoryDto,
  type CreatePublisherDto,
  type CreateUserDto,
  type CreateReviewDto,
  type CreateBookReservationDto,
} from "../services/api";

export const useLibraryStore = defineStore("library", () => {
  // State
  const authors = ref<Author[]>([]);
  const books = ref<Book[]>([]);
  const categories = ref<Category[]>([]);
  const publishers = ref<Publisher[]>([]);
  const users = ref<User[]>([]);
  const reviews = ref<Review[]>([]);
  const bookReservations = ref<BookReservation[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  // Utility function to handle async operations
  const handleAsync = async <T>(
    operation: () => Promise<T>
  ): Promise<T | null> => {
    try {
      loading.value = true;
      error.value = null;
      return await operation();
    } catch (err) {
      error.value = err instanceof Error ? err.message : "An error occurred";
      console.error("Store operation failed:", err);
      return null;
    } finally {
      loading.value = false;
    }
  };

  // Fetch methods
  const fetchAuthors = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getAuthors();
      authors.value = data;
      return data;
    });
  };

  const fetchBooks = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getBooks();
      books.value = data;
      return data;
    });
  };

  const fetchCategories = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getCategories();
      categories.value = data;
      return data;
    });
  };

  const fetchPublishers = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getPublishers();
      publishers.value = data;
      return data;
    });
  };

  const fetchUsers = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getUsers();
      users.value = data;
      return data;
    });
  };

  const fetchReviews = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getReviews();
      reviews.value = data;
      return data;
    });
  };

  const fetchBookReservations = async (): Promise<void> => {
    await handleAsync(async () => {
      const data = await apiService.getBookReservations();
      bookReservations.value = data;
      return data;
    });
  };

  // Computed properties
  const getBooksWithAuthors = computed(() => {
    return books.value; // API already includes authorName
  });

  const getBooksByAuthor = async (authorId: number): Promise<Book[]> => {
    const result = await handleAsync(async () => {
      return await apiService.getBooksByAuthor(authorId);
    });
    return result || [];
  };

  const getBooksByCategory = async (categoryId: number): Promise<Book[]> => {
    const result = await handleAsync(async () => {
      return await apiService.getBooksByCategory(categoryId);
    });
    return result || [];
  };

  // Author actions
  const addAuthor = async (author: CreateAuthorDto): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newAuthor = await apiService.createAuthor(author);
      authors.value.push(newAuthor);
      return newAuthor;
    });
    return result !== null;
  };

  const updateAuthor = async (
    id: number,
    author: Partial<CreateAuthorDto>
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.updateAuthor(id, author);
      await fetchAuthors(); // Refresh the list
      return true;
    });
    return result !== null;
  };

  const deleteAuthor = async (id: number): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.deleteAuthor(id);
      authors.value = authors.value.filter((a) => a.id !== id);
      return true;
    });
    return result !== null;
  };

  // Book actions
  const addBook = async (book: CreateBookDto): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newBook = await apiService.createBook(book);
      books.value.push(newBook);
      return newBook;
    });
    return result !== null;
  };

  const updateBook = async (
    id: number,
    book: Partial<CreateBookDto>
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.updateBook(id, book);
      await fetchBooks(); // Refresh the list
      return true;
    });
    return result !== null;
  };

  const deleteBook = async (id: number): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.deleteBook(id);
      books.value = books.value.filter((b) => b.id !== id);
      return true;
    });
    return result !== null;
  };

  // Category actions
  const addCategory = async (category: CreateCategoryDto): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newCategory = await apiService.createCategory(category);
      categories.value.push(newCategory);
      return newCategory;
    });
    return result !== null;
  };

  const updateCategory = async (
    id: number,
    category: Partial<CreateCategoryDto>
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.updateCategory(id, category);
      await fetchCategories(); // Refresh the list
      return true;
    });
    return result !== null;
  };

  const deleteCategory = async (id: number): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.deleteCategory(id);
      categories.value = categories.value.filter((c) => c.id !== id);
      return true;
    });
    return result !== null;
  };

  // Publisher actions
  const addPublisher = async (
    publisher: CreatePublisherDto
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newPublisher = await apiService.createPublisher(publisher);
      publishers.value.push(newPublisher);
      return newPublisher;
    });
    return result !== null;
  };

  const updatePublisher = async (
    id: number,
    publisher: Partial<CreatePublisherDto>
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.updatePublisher(id, publisher);
      await fetchPublishers(); // Refresh the list
      return true;
    });
    return result !== null;
  };

  const deletePublisher = async (id: number): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.deletePublisher(id);
      publishers.value = publishers.value.filter((p) => p.id !== id);
      return true;
    });
    return result !== null;
  };

  // User actions
  const addUser = async (user: CreateUserDto): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newUser = await apiService.createUser(user);
      users.value.push(newUser);
      return newUser;
    });
    return result !== null;
  };

  const updateUser = async (
    id: number,
    user: Partial<CreateUserDto>
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.updateUser(id, user);
      await fetchUsers(); // Refresh the list
      return true;
    });
    return result !== null;
  };

  const deleteUser = async (id: number): Promise<boolean> => {
    const result = await handleAsync(async () => {
      await apiService.deleteUser(id);
      users.value = users.value.filter((u) => u.id !== id);
      return true;
    });
    return result !== null;
  };

  // Review actions
  const addReview = async (review: CreateReviewDto): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newReview = await apiService.createReview(review);
      reviews.value.push(newReview);
      return newReview;
    });
    return result !== null;
  };

  // BookReservation actions
  const addBookReservation = async (
    reservation: CreateBookReservationDto
  ): Promise<boolean> => {
    const result = await handleAsync(async () => {
      const newReservation = await apiService.createBookReservation(
        reservation
      );
      bookReservations.value.push(newReservation);
      return newReservation;
    });
    return result !== null;
  };

  // Initialize all data
  const initialize = async (): Promise<void> => {
    await Promise.all([
      fetchAuthors(),
      fetchBooks(),
      fetchCategories(),
      fetchPublishers(),
      fetchUsers(),
      fetchReviews(),
      fetchBookReservations(),
    ]);
  };

  return {
    // State
    authors,
    books,
    categories,
    publishers,
    users,
    reviews,
    bookReservations,
    loading,
    error,

    // Getters
    getBooksWithAuthors,

    // Fetch methods
    fetchAuthors,
    fetchBooks,
    fetchCategories,
    fetchPublishers,
    fetchUsers,
    fetchReviews,
    fetchBookReservations,

    // Query methods
    getBooksByAuthor,
    getBooksByCategory,

    // Author actions
    addAuthor,
    updateAuthor,
    deleteAuthor,

    // Book actions
    addBook,
    updateBook,
    deleteBook,

    // Category actions
    addCategory,
    updateCategory,
    deleteCategory,

    // Publisher actions
    addPublisher,
    updatePublisher,
    deletePublisher,

    // User actions
    addUser,
    updateUser,
    deleteUser,

    // Review actions
    addReview,

    // BookReservation actions
    addBookReservation,

    // Initialize
    initialize,
  };
});

// Re-export types for convenience
export type {
  Author,
  Book,
  Category,
  Publisher,
  User,
  Review,
  BookReservation,
  CreateAuthorDto,
  CreateBookDto,
  CreateCategoryDto,
  CreatePublisherDto,
  CreateUserDto,
  CreateReviewDto,
  CreateBookReservationDto,
} from "../services/api";
