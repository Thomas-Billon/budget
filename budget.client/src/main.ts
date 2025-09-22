import '@/assets/main.scss';

import App from '@/App.vue';
import { createApp } from 'vue';
import { createWebHistory, createRouter } from 'vue-router';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faPlus, faMinus, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { routerConfig } from '@/router.ts';
import vColor from './directives/color';

const router = createRouter({
    history: createWebHistory(),
    routes: routerConfig
});

library.add([faPlus, faMinus, faArrowLeft]);

createApp(App)
    .use(router)
    .directive('color', vColor)
    .component('font-awesome-icon', FontAwesomeIcon)
    .mount('#app');
