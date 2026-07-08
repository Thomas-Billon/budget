import '@/assets/main.scss';

import { createRouter, createWebHistory } from 'vue-router';
import { faArrowLeft, faFloppyDisk, faMinus, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { routeAuthGuard, routerConfig } from '@/router.ts';
import App from '@/App.vue';
import { initApiAuth } from '@/utils/ApiAuth';
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { useAuthStore } from '@/stores/useAuthStore';
import vColor from '@/directives/Color';

const pinia = createPinia();

const router = createRouter({
    history: createWebHistory(),
    routes: routerConfig
});
router.beforeEach(routeAuthGuard);

library.add([faPlus, faMinus, faFloppyDisk, faTrash, faArrowLeft]);

const app = createApp(App)
    .use(pinia)
    .use(router)
    .directive('color', vColor)
    .component('font-awesome-icon', FontAwesomeIcon);

const authStore = useAuthStore();

initApiAuth(authStore, router);

app.mount('#app');
