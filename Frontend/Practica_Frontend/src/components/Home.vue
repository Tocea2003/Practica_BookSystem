<template>
  <div class="home">
    <div class="welcome-card">
      <h1><i class="pi pi-book"></i> Biblioteca Digitală</h1>
      <p>Bine ai venit în aplicația de management a cărților și autorilor!</p>
      <p>Conectată cu backend-ul prin API.</p>
    </div>

    <div v-if="loading" class="loading-section">
      <div class="loading-content">
        <i class="pi pi-spin pi-spinner" style="font-size: 2rem"></i>
        <p>Se încarcă datele...</p>
      </div>
    </div>

    <div v-if="error" class="error-section">
      <div class="error-content">
        <i
          class="pi pi-exclamation-triangle"
          style="font-size: 2rem; color: #dc3545"
        ></i>
        <p class="error-message">{{ error }}</p>
        <Button label="Încearcă din nou" @click="store.initialize()" />
      </div>
    </div>

    <div v-if="!loading && !error" class="data-section">
      <div class="grid">
        <div class="data-card">
          <h2><i class="pi pi-user"></i> Autori ({{ authors.length }})</h2>
          <div v-if="authors.length > 0">
            <div
              v-for="author in authors.slice(0, 3)"
              :key="author.id"
              class="author-item"
            >
              <strong>{{ author.name }}</strong> ({{ author.nationality }})
              <p class="author-bio">{{ author.biography }}</p>
            </div>
            <div v-if="authors.length > 3" class="more-items">
              <small>... și încă {{ authors.length - 3 }} autori</small>
            </div>
          </div>
          <p v-else class="no-data">Nu sunt autori disponibili</p>
          <Button label="Vezi toți autorii" @click="$router.push('/authors')" />
        </div>

        <div class="data-card">
          <h2><i class="pi pi-book"></i> Cărți ({{ books.length }})</h2>
          <div v-if="books.length > 0">
            <div
              v-for="book in books.slice(0, 3)"
              :key="book.id"
              class="book-item"
            >
              <strong>{{ book.title }}</strong>
              <p class="book-author">de {{ book.authorName }}</p>
              <p class="book-price">{{ book.price }} RON</p>
            </div>
            <div v-if="books.length > 3" class="more-items">
              <small>... și încă {{ books.length - 3 }} cărți</small>
            </div>
          </div>
          <p v-else class="no-data">Nu sunt cărți disponibile</p>
          <Button label="Vezi toate cărțile" @click="$router.push('/books')" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useLibraryStore } from "../stores/library";
import Button from "primevue/button";

const store = useLibraryStore();
const { authors, books, loading, error } = store;

onMounted(() => {
  store.initialize();
});
</script>

<style scoped>
.home {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.welcome-card {
  margin-bottom: 2rem;
  text-align: center;
  padding: 2rem;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e9ecef;
}

.welcome-card h1 {
  color: var(--primary-color);
  margin-bottom: 1rem;
}

.loading-section,
.error-section {
  margin-bottom: 2rem;
}

.loading-content,
.error-content {
  text-align: center;
  padding: 2rem;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e9ecef;
}

.error-message {
  color: #dc3545;
  font-weight: 500;
  margin: 1rem 0;
}

.data-section {
  margin-top: 2rem;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 2rem;
}

.data-card {
  padding: 2rem;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e9ecef;
}

.data-card h2 {
  color: var(--primary-color);
  margin-bottom: 1.5rem;
  font-size: 1.5rem;
}

.author-item,
.book-item {
  margin-bottom: 1rem;
  padding: 1rem;
  border-left: 4px solid var(--primary-color);
  background: white;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.author-bio {
  font-size: 0.9rem;
  color: #6c757d;
  margin: 0.5rem 0 0 0;
  line-height: 1.4;
}

.book-author {
  font-size: 0.9rem;
  color: #6c757d;
  margin: 0.5rem 0;
}

.book-price {
  font-weight: bold;
  color: var(--primary-color);
  margin: 0.5rem 0 0 0;
}

.more-items {
  text-align: center;
  margin: 1.5rem 0;
  color: #6c757d;
  font-style: italic;
}

.no-data {
  text-align: center;
  color: #6c757d;
  font-style: italic;
  margin: 2rem 0;
  font-size: 1.1rem;
}

.data-card .p-button {
  margin-top: 1rem;
  width: 100%;
}

@media (max-width: 768px) {
  .home {
    padding: 1rem;
  }

  .grid {
    grid-template-columns: 1fr;
  }

  .data-card {
    padding: 1.5rem;
  }
}
</style>
