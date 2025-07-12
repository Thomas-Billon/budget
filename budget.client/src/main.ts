import '@/assets/main.css';
import App from '@/App.vue';
import { createApp } from 'vue';
import { createWebHistory, createRouter } from 'vue-router';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faPlus, faMinus, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { routes } from '@/router.ts';

library.add([faPlus, faMinus, faArrowLeft]);

const router = createRouter({
    history: createWebHistory(),
    routes
});

createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .use(router)
    .mount('#app');
