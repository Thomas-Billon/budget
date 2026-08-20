import '@/assets/css/main.scss';

import { createRouter, createWebHistory } from 'vue-router';
import {
    faArrowLeft,
    faArrowTrendDown,
    faArrowTrendUp,
    faBriefcase,
    faBuilding,
    faCalendar,
    faCalendarDay,
    faCar,
    faCartShopping,
    faChartLine,
    faCheck,
    faClockRotateLeft,
    faCoins,
    faCreditCard,
    faEnvelope,
    faEye,
    faEyeSlash,
    faFileInvoice,
    faFloppyDisk,
    faGamepad,
    faGift,
    faGraduationCap,
    faHouse,
    faKitMedical,
    faLandmark,
    faLock,
    faMinus,
    faPaw,
    faPiggyBank,
    faPlane,
    faPlus,
    faReceipt,
    faRepeat,
    faRightFromBracket,
    faRuler,
    faScrewdriverWrench,
    faTags,
    faTrash,
    faTriangleExclamation,
    faUtensils,
    faVault,
    faWallet,
    faWandMagicSparkles,
    faXmark
} from '@fortawesome/free-solid-svg-icons';
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

library.add([
    faPlus, faMinus, faFloppyDisk, faTrash, faArrowLeft,
    faHouse, faLandmark, faReceipt, faTags, faRightFromBracket,
    faEnvelope, faEye, faEyeSlash, faLock, faWandMagicSparkles,
    faArrowTrendUp, faBriefcase, faCartShopping, faGamepad, faWallet,
    faCalendarDay, faChartLine, faClockRotateLeft, faRepeat, faXmark,
    faBuilding, faCar, faCheck, faGraduationCap, faKitMedical, faPaw,
    faPiggyBank, faPlane, faRuler, faScrewdriverWrench, faUtensils,
    faArrowTrendDown, faTriangleExclamation, faCalendar, faVault,
    faCoins, faCreditCard, faFileInvoice, faGift
]);

const app = createApp(App)
    .use(pinia)
    .use(router)
    .directive('color', vColor)
    .component('font-awesome-icon', FontAwesomeIcon);

const authStore = useAuthStore();

initApiAuth(authStore, router);

app.mount('#app');
