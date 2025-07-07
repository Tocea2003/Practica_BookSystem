<template>
  <div id="app">
    <header class="app-header">
      <div class="header-content">
        <h1 class="app-title">
          <i class="pi pi-book"></i>
          Library Management System
        </h1>
        <nav class="app-nav">
          <Button
            label="Home"
            @click="$router.push('/')"
            :class="{ 'nav-button-active': $route.name === 'Home' }"
            class="nav-button p-button-text"
          />
          <Button
            label="Cărți"
            @click="$router.push('/books')"
            :class="{ 'nav-button-active': $route.name === 'Books' }"
            class="nav-button p-button-text"
          />
          <Button
            label="Autori"
            @click="$router.push('/authors')"
            :class="{ 'nav-button-active': $route.name === 'Authors' }"
            class="nav-button p-button-text"
          />
        </nav>
      </div>
    </header>

    <main class="app-main">
      <router-view />
    </main>

    <footer class="app-footer">
      <p>&copy; 2025 Library Management System. All rights reserved.</p>
    </footer>

    <Toast />
  </div>
</template>

<script setup lang="ts">
import { useLibraryStore } from "./stores/library";
import { onMounted } from "vue";
import Button from "primevue/button";
import Toast from "primevue/toast";

const store = useLibraryStore();

// Initialize data when app starts
onMounted(() => {
  store.initialize();
});
</script>

<style scoped>
#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.app-header {
  background: #1976d2;
  color: white;
  padding: 1rem 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.header-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.app-title {
  margin: 0;
  font-size: 1.5rem;
  font-weight: bold;
}

.app-title i {
  margin-right: 0.5rem;
}

.app-nav {
  display: flex;
  gap: 0.5rem;
}

.nav-button {
  color: white !important;
  border: 1px solid transparent !important;
}

.nav-button:hover {
  background: rgba(255, 255, 255, 0.1) !important;
  border-color: white !important;
}

.nav-button-active {
  background: rgba(255, 255, 255, 0.2) !important;
  border-color: white !important;
}

.app-main {
  flex: 1;
  background: #f8f9fa;
  min-height: calc(100vh - 140px);
}

.app-footer {
  background: #343a40;
  color: white;
  text-align: center;
  padding: 1rem;
  margin-top: auto;
}

.app-footer p {
  margin: 0;
}

@media (max-width: 768px) {
  .header-content {
    flex-direction: column;
    gap: 1rem;
  }

  .app-nav {
    flex-wrap: wrap;
    justify-content: center;
  }
}
</style>
