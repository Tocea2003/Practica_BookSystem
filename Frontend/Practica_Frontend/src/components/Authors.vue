<template>
  <div class="authors-page">
    <div class="header">
      <h1><i class="pi pi-user"></i> Autori</h1>
      <Button
        label="Adaugă Autor Nou"
        @click="showAddDialog = true"
        class="p-button-success"
      />
    </div>

    <Card>
      <template #content>
        <DataTable
          :value="authors"
          :paginator="true"
          :rows="10"
          :responsive="true"
          :loading="loading"
          filterDisplay="menu"
          :globalFilterFields="['name', 'nationality']"
        >
          <template #header>
            <div class="flex justify-content-between">
              <span class="text-xl text-900 font-bold">Lista Autorilor</span>
              <span class="p-input-icon-left">
                <i class="pi pi-search" />
                <InputText
                  v-model="globalFilter"
                  placeholder="Caută autori..."
                />
              </span>
            </div>
          </template>

          <Column field="name" header="Nume" :sortable="true">
            <template #body="{ data }">
              <strong>{{ data.name }}</strong>
            </template>
          </Column>

          <Column field="nationality" header="Naționalitate" :sortable="true">
            <template #body="{ data }">
              <Tag :value="data.nationality" />
            </template>
          </Column>

          <Column field="birthDate" header="Data Nașterii" :sortable="true">
            <template #body="{ data }">
              {{ formatDate(data.birthDate) }}
            </template>
          </Column>

          <Column field="books" header="Cărți">
            <template #body="{ data }">
              <Badge :value="getAuthorBooksCount(data.id)" severity="info" />
            </template>
          </Column>

          <Column header="Acțiuni">
            <template #body="{ data }">
              <Button
                icon="pi pi-eye"
                @click="viewAuthor(data)"
                class="p-button-rounded p-button-text"
                v-tooltip="'Vezi detalii'"
              />
              <Button
                icon="pi pi-pencil"
                @click="editAuthor(data)"
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
      :header="editingAuthor ? 'Editează Autor' : 'Adaugă Autor Nou'"
      :modal="true"
      :style="{ width: '50vw' }"
    >
      <div class="p-fluid">
        <div class="field">
          <label for="name">Nume *</label>
          <InputText
            id="name"
            v-model="authorForm.name"
            :class="{ 'p-invalid': submitted && !authorForm.name }"
          />
          <small class="p-error" v-if="submitted && !authorForm.name">
            Numele este obligatoriu.
          </small>
        </div>

        <div class="field">
          <label for="nationality">Naționalitate *</label>
          <InputText
            id="nationality"
            v-model="authorForm.nationality"
            :class="{ 'p-invalid': submitted && !authorForm.nationality }"
          />
          <small class="p-error" v-if="submitted && !authorForm.nationality">
            Naționalitatea este obligatorie.
          </small>
        </div>

        <div class="field">
          <label for="birthDate">Data Nașterii *</label>
          <Calendar
            id="birthDate"
            v-model="authorForm.birthDate"
            dateFormat="dd/mm/yy"
            :class="{ 'p-invalid': submitted && !authorForm.birthDate }"
          />
          <small class="p-error" v-if="submitted && !authorForm.birthDate">
            Data nașterii este obligatorie.
          </small>
        </div>

        <div class="field">
          <label for="biography">Biografie</label>
          <Textarea
            id="biography"
            v-model="authorForm.biography"
            rows="4"
            cols="30"
          />
        </div>
      </div>

      <template #footer>
        <Button label="Anulează" @click="hideDialog" class="p-button-text" />
        <Button label="Salvează" @click="saveAuthor" class="p-button-success" />
      </template>
    </Dialog>

    <!-- View Details Dialog -->
    <Dialog
      v-model:visible="showViewDialog"
      header="Detalii Autor"
      :modal="true"
      :style="{ width: '50vw' }"
    >
      <div v-if="selectedAuthor" class="author-details">
        <div class="field">
          <label>Nume:</label>
          <p>{{ selectedAuthor.name }}</p>
        </div>
        <div class="field">
          <label>Naționalitate:</label>
          <p>{{ selectedAuthor.nationality }}</p>
        </div>
        <div class="field">
          <label>Data Nașterii:</label>
          <p>{{ formatDate(selectedAuthor.birthDate) }}</p>
        </div>
        <div class="field">
          <label>Biografie:</label>
          <p>
            {{ selectedAuthor.biography || "Nu există biografie disponibilă." }}
          </p>
        </div>
        <div class="field">
          <label>Cărți scrise:</label>
          <div
            v-if="selectedAuthor && getAuthorBooksCount(selectedAuthor.id) > 0"
          >
            <ul>
              <li v-for="book in authorBooks[selectedAuthor.id]" :key="book.id">
                {{ book.title }} ({{
                  new Date(book.publishedDate).getFullYear()
                }})
              </li>
            </ul>
          </div>
          <p v-else>Nu sunt cărți asociate acestui autor.</p>
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
        <span v-if="selectedAuthor">
          Ești sigur că vrei să ștergi autorul
          <strong>{{ selectedAuthor.name }}</strong
          >?
        </span>
      </div>
      <template #footer>
        <Button
          label="Nu"
          @click="showDeleteDialog = false"
          class="p-button-text"
        />
        <Button label="Da" @click="deleteAuthor" class="p-button-danger" />
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useLibraryStore, type Author } from "../stores/library";
import { useToast } from "primevue/usetoast";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import Card from "primevue/card";
import Dialog from "primevue/dialog";
import InputText from "primevue/inputtext";
import Calendar from "primevue/calendar";
import Textarea from "primevue/textarea";
import Tag from "primevue/tag";
import Badge from "primevue/badge";

const libraryStore = useLibraryStore();
const toast = useToast();

const authors = ref<Author[]>([]);
const loading = ref(false);
const globalFilter = ref("");
const showAddDialog = ref(false);
const showViewDialog = ref(false);
const showDeleteDialog = ref(false);
const selectedAuthor = ref<Author | null>(null);
const editingAuthor = ref(false);
const submitted = ref(false);

const authorForm = ref({
  name: "",
  nationality: "",
  birthDate: null as Date | null,
  biography: "",
});

const loadAuthors = async () => {
  loading.value = true;
  await libraryStore.fetchAuthors();
  authors.value = [...libraryStore.authors];

  // Încarcă cărțile pentru fiecare autor pentru a afișa badge-ul
  for (const author of authors.value) {
    await getAuthorBooks(author.id);
  }

  loading.value = false;
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString("ro-RO");
};

const { getBooksByAuthor } = libraryStore;

// Cache pentru cărțile autorului
const authorBooks = ref<Record<number, Book[]>>({});

const getAuthorBooks = async (authorId: number) => {
  if (!authorBooks.value[authorId]) {
    const books = await getBooksByAuthor(authorId);
    authorBooks.value[authorId] = books;
  }
  return authorBooks.value[authorId] || [];
};

const getAuthorBooksCount = (authorId: number) => {
  return authorBooks.value[authorId]?.length || 0;
};

const viewAuthor = async (author: Author) => {
  selectedAuthor.value = author;
  // Încarcă cărțile autorului
  await getAuthorBooks(author.id);
  showViewDialog.value = true;
};

const editAuthor = (author: Author) => {
  selectedAuthor.value = author;
  editingAuthor.value = true;
  authorForm.value = {
    name: author.name,
    nationality: author.nationality,
    birthDate: new Date(author.birthDate),
    biography: author.biography,
  };
  showAddDialog.value = true;
};

const confirmDelete = (author: Author) => {
  selectedAuthor.value = author;
  showDeleteDialog.value = true;
};

const hideDialog = () => {
  showAddDialog.value = false;
  editingAuthor.value = false;
  submitted.value = false;
  resetForm();
};

const resetForm = () => {
  authorForm.value = {
    name: "",
    nationality: "",
    birthDate: null,
    biography: "",
  };
};

const saveAuthor = async () => {
  submitted.value = true;

  if (
    !authorForm.value.name ||
    !authorForm.value.nationality ||
    !authorForm.value.birthDate
  ) {
    return;
  }

  try {
    const authorData = {
      name: authorForm.value.name,
      nationality: authorForm.value.nationality,
      birthDate: authorForm.value.birthDate.toISOString().split("T")[0],
      biography: authorForm.value.biography,
    };

    let success = false;
    if (editingAuthor.value && selectedAuthor.value) {
      success = await libraryStore.updateAuthor(
        selectedAuthor.value.id,
        authorData
      );
      if (success) {
        toast.add({
          severity: "success",
          summary: "Succes",
          detail: "Autor actualizat cu succes",
          life: 3000,
        });
      }
    } else {
      success = await libraryStore.addAuthor(authorData);
      if (success) {
        toast.add({
          severity: "success",
          summary: "Succes",
          detail: "Autor adăugat cu succes",
          life: 3000,
        });
      }
    }

    if (success) {
      hideDialog();
      await loadAuthors();
    } else {
      toast.add({
        severity: "error",
        summary: "Eroare",
        detail: libraryStore.error || "A apărut o eroare la salvarea autorului",
        life: 3000,
      });
    }
  } catch (error) {
    toast.add({
      severity: "error",
      summary: "Eroare",
      detail: "A apărut o eroare la salvarea autorului",
      life: 3000,
    });
  }
};

const deleteAuthor = async () => {
  if (!selectedAuthor.value) return;

  try {
    const success = await libraryStore.deleteAuthor(selectedAuthor.value.id);
    if (success) {
      toast.add({
        severity: "success",
        summary: "Succes",
        detail: "Autor șters cu succes",
        life: 3000,
      });
      showDeleteDialog.value = false;
      await loadAuthors();
    } else {
      toast.add({
        severity: "error",
        summary: "Eroare",
        detail:
          libraryStore.error || "A apărut o eroare la ștergerea autorului",
        life: 3000,
      });
      showDeleteDialog.value = false;
    }
  } catch (error) {
    toast.add({
      severity: "error",
      summary: "Eroare",
      detail: (error as Error).message,
      life: 3000,
    });
    showDeleteDialog.value = false;
  }
};

onMounted(async () => {
  await loadAuthors();
});
</script>

<style scoped>
.authors-page {
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

.author-details .field p {
  margin: 0;
  padding: 0.5rem 0;
}

.confirmation-content {
  display: flex;
  align-items: center;
}
</style>
