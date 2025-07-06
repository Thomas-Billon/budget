import './assets/main.css';

import { createApp } from 'vue';
import App from './App.vue';

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faPlus, faMinus, faEuro } from '@fortawesome/free-solid-svg-icons';

library.add([faPlus, faMinus, faEuro]);

createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .mount('#app');
