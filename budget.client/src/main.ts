import '@/assets/main.scss';

import { createRouter, createWebHistory } from 'vue-router';
import { faArrowLeft, faFloppyDisk, faMinus, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { routerConfig } from '@/router.ts';
import App from '@/App.vue';
import { createApp } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import vColor from '@/directives/Color';

const router = createRouter({
    history: createWebHistory(),
    routes: routerConfig
});

library.add([faPlus, faMinus, faFloppyDisk, faTrash, faArrowLeft]);

const app = createApp(App)
    .use(router)
    .directive('color', vColor)
    .component('font-awesome-icon', FontAwesomeIcon);

app.mount('#app');
