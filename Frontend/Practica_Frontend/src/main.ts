import { createApp } from "vue";
import { createPinia } from "pinia";
import { createRouter, createWebHistory } from "vue-router";
import PrimeVue from "primevue/config";
import Aura from "@primevue/themes/aura";
import ToastService from "primevue/toastservice";
import Tooltip from "primevue/tooltip";

import "./style.css";
import "primeicons/primeicons.css";

import App from "./App.vue";

const routes = [
  {
    path: "/",
    component: () => import("./components/Home.vue"),
    name: "Home",
  },
  {
    path: "/books",
    component: () => import("./components/Books.vue"),
    name: "Books",
  },
  {
    path: "/authors",
    component: () => import("./components/Authors.vue"),
    name: "Authors",
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

const pinia = createPinia();

const app = createApp(App);

app.use(PrimeVue, {
  theme: {
    preset: Aura,
  },
});

app.use(ToastService);
app.directive("tooltip", Tooltip);
app.use(router);
app.use(pinia);

app.mount("#app");
