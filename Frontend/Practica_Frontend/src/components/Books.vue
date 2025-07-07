<template>
  <div class="books-page">
    <div class="header">
      <h1><i class="pi pi-book"></i> Cărți</h1>
      <Button
        label="Adaugă Carte Nouă"
        @click="showAddDialog = true"
        class="p-button-success"
      />
    </div>

    <Card>
      <template #content>
        <DataTable
          :value="booksWithAuthors"
          :paginator="true"
          :rows="10"
          :responsive="true"
          :loading="loading"
          filterDisplay="menu"
          :globalFilterFields="['title', 'authorName', 'genre']"
        >
          <template #header>
            <div class="flex justify-content-between">
              <span class="text-xl text-900 font-bold">Lista Cărților</span>
              <span class="p-input-icon-left">
                <i class="pi pi-search" />
                <InputText
                  v-model="globalFilter"
                  placeholder="Caută cărți..."
                />
              </span>
            </div>
          </template>

          <Column field="title" header="Titlu" :sortable="true">
            <template #body="{ data }">
              <strong>{{ data.title }}</strong>
            </template>
          </Column>

          <Column field="authorName" header="Autor" :sortable="true">
            <template #body="{ data }">
              <Tag :value="data.authorName" severity="info" />
            </template>
          </Column>

          <Column field="genre" header="Gen" :sortable="true">
            <template #body="{ data }">
              <Tag :value="data.genre" severity="success" />
            </template>
          </Column>

          <Column
            field="publishedDate"
            header="Data Publicării"
            :sortable="true"
          >
            <template #body="{ data }">
              {{ formatDate(data.publishedDate) }}
            </template>
          </Column>

          <Column field="price" header="Preț" :sortable="true">
            <template #body="{ data }">
              <span class="price">{{ data.price }} RON</span>
            </template>
          </Column>

          <Column header="Acțiuni">
            <template #body="{ data }">
              <Button
                icon="pi pi-eye"
                @click="viewBook(data)"
                class="p-button-rounded p-button-text"
                v-tooltip="'Vezi detalii'"
              />
              <Button
                icon="pi pi-pencil"
                @click="editBook(data)"
                class="p-button-rounded p-button-text p-button-warning"
                v-tooltip="'Editează'"
              />
              <Button
                icon="pi pi-trash"
                @click="confirmDelete(data)"
                class="p-button-rounded p-button-text p-button-danger"
                v-tooltip="'Șterge'"
              />
            </template>
          </Column>
        </DataTable>
      </template>
    </Card>

    <!-- Add/Edit Dialog -->
    <Dialog
      v-model:visible="showAddDialog"
      :header="editingBook ? 'Editează Carte' : 'Adaugă Carte Nouă'"
      :modal="true"
      :style="{ width: '60vw' }"
    >
      <div class="p-fluid">
        <div class="formgrid grid">
          <div class="field col-12 md:col-6">
            <label for="title">Titlu *</label>
            <InputText
              id="title"
              v-model="bookForm.title"
              :class="{ 'p-invalid': submitted && !bookForm.title }"
            />
            <small class="p-error" v-if="submitted && !bookForm.title">
              Titlul este obligatoriu.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="author">Autor *</label>
            <Select
              id="author"
              v-model="bookForm.authorId"
              :options="authors"
              optionLabel="name"
              optionValue="id"
              placeholder="Selectează autor"
              :class="{ 'p-invalid': submitted && !bookForm.authorId }"
            />
            <small class="p-error" v-if="submitted && !bookForm.authorId">
              Autorul este obligatoriu.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="isbn">ISBN *</label>
            <InputText
              id="isbn"
              v-model="bookForm.isbn"
              :class="{ 'p-invalid': submitted && !bookForm.isbn }"
            />
            <small class="p-error" v-if="submitted && !bookForm.isbn">
              ISBN-ul este obligatoriu.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="genre">Gen *</label>
            <Select
              id="genre"
              v-model="bookForm.genre"
              :options="genres"
              placeholder="Selectează gen"
              :class="{ 'p-invalid': submitted && !bookForm.genre }"
            />
            <small class="p-error" v-if="submitted && !bookForm.genre">
              Genul este obligatoriu.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="publishedDate">Data Publicării *</label>
            <DatePicker
              id="publishedDate"
              v-model="bookForm.publishedDate"
              dateFormat="dd/mm/yy"
              :class="{ 'p-invalid': submitted && !bookForm.publishedDate }"
            />
            <small class="p-error" v-if="submitted && !bookForm.publishedDate">
              Data publicării este obligatorie.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="pages">Numărul de pagini *</label>
            <InputNumber
              id="pages"
              v-model="bookForm.pages"
              :min="1"
              :class="{ 'p-invalid': submitted && !bookForm.pages }"
            />
            <small class="p-error" v-if="submitted && !bookForm.pages">
              Numărul de pagini este obligatoriu.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="price">Preț (RON) *</label>
            <InputNumber
              id="price"
              v-model="bookForm.price"
              :min="0"
              :minFractionDigits="2"
              :maxFractionDigits="2"
              :class="{ 'p-invalid': submitted && !bookForm.price }"
            />
            <small class="p-error" v-if="submitted && !bookForm.price">
              Prețul este obligatoriu.
            </small>
          </div>

          <div class="field col-12 md:col-6">
            <label for="publisher">Editura</label>
            <Select
              id="publisher"
              v-model="bookForm.publisherId"
              :options="publishers"
              optionLabel="name"
              optionValue="id"
              placeholder="Selectează editura"
              showClear
            />
          </div>

          <div class="field col-12">
            <label for="categories">Categorii</label>
            <MultiSelect
              id="categories"
              v-model="bookForm.categoryIds"
              :options="categories"
              optionLabel="name"
              optionValue="id"
              placeholder="Selectează categorii"
              :maxSelectedLabels="3"
              selectedItemsLabel="{0} categorii selectate"
            />
          </div>

          <div class="field col-12">
            <label for="description">Descriere</label>
            <Textarea
              id="description"
              v-model="bookForm.description"
              rows="4"
              cols="30"
            />
          </div>
        </div>
      </div>

      <template #footer>
        <Button label="Anulează" @click="hideDialog" class="p-button-text" />
        <Button label="Salvează" @click="saveBook" class="p-button-success" />
      </template>
    </Dialog>

    <!-- View Details Dialog -->
    <Dialog
      v-model:visible="showViewDialog"
      header="Detalii Carte"
      :modal="true"
      :style="{ width: '60vw' }"
    >
      <div v-if="selectedBook" class="book-details">
        <div class="formgrid grid">
          <div class="field col-12 md:col-6">
            <label>Titlu:</label>
            <p>{{ selectedBook.title }}</p>
          </div>
          <div class="field col-12 md:col-6">
            <label>Autor:</label>
            <p>{{ selectedBook.authorName }}</p>
          </div>
          <div class="field col-12 md:col-6">
            <label>ISBN:</label>
            <p>{{ selectedBook.isbn }}</p>
          </div>
          <div class="field col-12 md:col-6">
            <label>Gen:</label>
            <p>{{ selectedBook.genre }}</p>
          </div>
          <div class="field col-12 md:col-6">
            <label>Data Publicării:</label>
            <p>{{ formatDate(selectedBook.publishedDate) }}</p>
          </div>
          <div class="field col-12 md:col-6">
            <label>Numărul de pagini:</label>
            <p>{{ selectedBook.pages }}</p>
          </div>
          <div class="field col-12 md:col-6">
            <label>Preț:</label>
            <p class="price">{{ selectedBook.price }} RON</p>
          </div>
          <div class="field col-12">
            <label>Descriere:</label>
            <p>
              {{
                selectedBook.description || "Nu există descriere disponibilă."
              }}
            </p>
          </div>
        </div>
      </div>
    </Dialog>

    <!-- Delete Confirmation Dialog -->
    <Dialog
      v-model:visible="showDeleteDialog"
      header="Confirmă Ștergerea"
      :modal="true"
      :style="{ width: '350px' }"
    >
      <div class="confirmation-content">
        <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
        <span v-if="selectedBook">
          Ești sigur că vrei să ștergi cartea
          <strong>{{ selectedBook.title }}</strong
          >?
        </span>
      </div>
      <template #footer>
        <Button
          label="Nu"
          @click="showDeleteDialog = false"
          class="p-button-text"
        />
        <Button label="Da" @click="deleteBook" class="p-button-danger" />
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useLibraryStore, type Book } from "../stores/library";
import { useToast } from "primevue/usetoast";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import Card from "primevue/card";
import Dialog from "primevue/dialog";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import DatePicker from "primevue/datepicker";
import Textarea from "primevue/textarea";
import Select from "primevue/select";
import MultiSelect from "primevue/multiselect";
import Tag from "primevue/tag";

const libraryStore = useLibraryStore();
const toast = useToast();

const booksWithAuthors = computed(() => libraryStore.getBooksWithAuthors);
const loading = computed(() => libraryStore.loading);
const globalFilter = ref("");
const showAddDialog = ref(false);
const showViewDialog = ref(false);
const showDeleteDialog = ref(false);
const selectedBook = ref<(Book & { authorName: string }) | null>(null);
const editingBook = ref(false);
const submitted = ref(false);

const authors = computed(() => libraryStore.authors);
const publishers = computed(() => libraryStore.publishers);
const categories = computed(() => libraryStore.categories);

const genres = [
  "Fantasy",
  "Science Fiction",
  "Mystery",
  "Romance",
  "Thriller",
  "Horror",
  "Biography",
  "History",
  "Self-Help",
  "Education",
  "Children",
  "Poetry",
  "Drama",
  "Comedy",
  "Adventure",
];

const bookForm = ref({
  title: "",
  authorId: null as number | null,
  isbn: "",
  genre: "",
  publishedDate: null as Date | null,
  pages: null as number | null,
  price: null as number | null,
  description: "",
  publisherId: null as number | null,
  categoryIds: [] as number[],
});

const loadBooks = async () => {
  await libraryStore.fetchBooks();
  await libraryStore.fetchAuthors(); // Ensure authors are loaded for name mapping
  await libraryStore.fetchPublishers(); // Ensure publishers are loaded for dropdown
  await libraryStore.fetchCategories(); // Ensure categories are loaded for dropdown
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString("ro-RO");
};

const viewBook = (book: Book & { authorName: string }) => {
  selectedBook.value = book;
  showViewDialog.value = true;
};

const editBook = (book: Book & { authorName: string }) => {
  selectedBook.value = book;
  editingBook.value = true;
  bookForm.value = {
    title: book.title,
    authorId: book.authorId,
    isbn: book.isbn,
    genre: book.genre,
    publishedDate: new Date(book.publishedDate),
    pages: book.pages,
    price: book.price,
    description: book.description,
    publisherId: book.publisherId || null,
    categoryIds: book.categories?.map((c: { id: number }) => c.id) || [],
  };
  showAddDialog.value = true;
};

const confirmDelete = (book: Book & { authorName: string }) => {
  selectedBook.value = book;
  showDeleteDialog.value = true;
};

const hideDialog = () => {
  showAddDialog.value = false;
  editingBook.value = false;
  submitted.value = false;
  resetForm();
};

const resetForm = () => {
  bookForm.value = {
    title: "",
    authorId: null,
    isbn: "",
    genre: "",
    publishedDate: null,
    pages: null,
    price: null,
    description: "",
    publisherId: null,
    categoryIds: [],
  };
};

const saveBook = async () => {
  submitted.value = true;

  if (
    !bookForm.value.title ||
    !bookForm.value.authorId ||
    !bookForm.value.isbn ||
    !bookForm.value.genre ||
    !bookForm.value.publishedDate ||
    !bookForm.value.pages ||
    !bookForm.value.price
  ) {
    return;
  }

  try {
    const bookData = {
      title: bookForm.value.title,
      authorId: bookForm.value.authorId,
      isbn: bookForm.value.isbn,
      genre: bookForm.value.genre,
      publishedDate: bookForm.value.publishedDate.toISOString().split("T")[0],
      pages: bookForm.value.pages,
      price: bookForm.value.price,
      description: bookForm.value.description,
      publisherId:
        bookForm.value.publisherId !== null
          ? bookForm.value.publisherId
          : undefined,
      categoryIds: bookForm.value.categoryIds,
    };

    let success = false;
    if (editingBook.value && selectedBook.value) {
      success = await libraryStore.updateBook(selectedBook.value.id, bookData);
      if (success) {
        toast.add({
          severity: "success",
          summary: "Succes",
          detail: "Carte actualizată cu succes",
          life: 3000,
        });
      }
    } else {
      success = await libraryStore.addBook(bookData);
      if (success) {
        toast.add({
          severity: "success",
          summary: "Succes",
          detail: "Carte adăugată cu succes",
          life: 3000,
        });
      }
    }

    if (success) {
      hideDialog();
      await loadBooks();
    } else {
      toast.add({
        severity: "error",
        summary: "Eroare",
        detail: libraryStore.error || "A apărut o eroare la salvarea cărții",
        life: 3000,
      });
    }
  } catch (error) {
    toast.add({
      severity: "error",
      summary: "Eroare",
      detail: "A apărut o eroare la salvarea cărții",
      life: 3000,
    });
  }
};

const deleteBook = async () => {
  if (!selectedBook.value) return;

  try {
    const success = await libraryStore.deleteBook(selectedBook.value.id);
    if (success) {
      toast.add({
        severity: "success",
        summary: "Succes",
        detail: "Carte ștearsă cu succes",
        life: 3000,
      });
      showDeleteDialog.value = false;
      await loadBooks();
    } else {
      toast.add({
        severity: "error",
        summary: "Eroare",
        detail: libraryStore.error || "A apărut o eroare la ștergerea cărții",
        life: 3000,
      });
      showDeleteDialog.value = false;
    }
  } catch (error) {
    toast.add({
      severity: "error",
      summary: "Eroare",
      detail: "A apărut o eroare la ștergerea cărții",
      life: 3000,
    });
    showDeleteDialog.value = false;
  }
};

onMounted(async () => {
  await libraryStore.fetchAuthors(); // Ensure authors are loaded for dropdown
  await libraryStore.fetchPublishers(); // Ensure publishers are loaded for dropdown
  await libraryStore.fetchCategories(); // Ensure categories are loaded for dropdown
  await loadBooks();
});
</script>

<style scoped>
.books-page {
  padding: 2rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.field {
  margin-bottom: 1rem;
}

.field label {
  font-weight: bold;
  margin-bottom: 0.5rem;
  display: block;
}

.book-details .field p {
  margin: 0;
  padding: 0.5rem 0;
}

.price {
  font-weight: bold;
  color: var(--primary-color);
}

.confirmation-content {
  display: flex;
  align-items: center;
}

.formgrid {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  gap: 1rem;
}

.col-12 {
  grid-column: span 12;
}

@media (min-width: 768px) {
  .md\:col-6 {
    grid-column: span 6;
  }
}
</style>
