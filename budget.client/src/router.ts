import AccountCreateView from '@/features/accounts/views/CreateView.vue';
import AccountListView from '@/features/accounts/views/ListView.vue';
import AccountUpdateView from '@/features/accounts/views/UpdateView.vue';
import BalanceReportView from '@/features/balances/views/ReportView.vue';
import CategoryCreateView from '@/features/categories/views/CreateView.vue';
import CategoryListView from '@/features/categories/views/ListView.vue';
import CategoryUpdateView from '@/features/categories/views/UpdateView.vue';
import ConfirmEmailView from '@/features/auth/views/ConfirmEmailView.vue';
import ForgotPasswordView from '@/features/auth/views/ForgotPasswordView.vue';
import HomeView from '@/views/HomeView.vue';
import LoginView from '@/features/auth/views/LoginView.vue';
import RegisterView from '@/features/auth/views/RegisterView.vue';
import ResendEmailConfirmationView from '@/features/auth/views/ResendEmailConfirmationView.vue';
import ResetPasswordView from '@/features/auth/views/ResetPasswordView.vue';
import { type RouteLocationNormalized } from 'vue-router';
import TransactionCreateView from '@/features/transactions/views/CreateView.vue';
import TransactionHistoryView from '@/features/transactions/views/HistoryView.vue';
import TransactionUpdateView from '@/features/transactions/views/UpdateView.vue';
import { useAuthStore } from '@/stores/useAuthStore';

const getIdParam = (id?: number): string => {
    if (id !== undefined && id !== null) {
        return `/${id}`;
    }
    return '';
};

const routes = {
    home: '/',
    auth: {
        login: '/login',
        register: '/register',
        forgotPassword: '/forgot-password',
        resetPassword: '/reset-password',
        confirmEmail: '/confirm-email',
        resendEmailConfirmation: '/resend-email-confirmation'
    },
    balance: {
        report: '/balance/report'
    },
    transaction: {
        history: '/transaction/history',
        create: '/transaction/create',
        update: (id?: number) => `/transaction/update${getIdParam(id)}`
    },
    category: {
        list: '/category/list',
        create: '/category/create',
        update: (id?: number) => `/category/update${getIdParam(id)}`
    },
    account: {
        list: '/account/list',
        create: '/account/create',
        update: (id?: number) => `/account/update${getIdParam(id)}`
    }
};

const routerConfig = [
    { path: routes.home, component: HomeView },
    { path: routes.auth.login, component: LoginView, meta: { public: true }},
    { path: routes.auth.register, component: RegisterView, meta: { public: true }},
    { path: routes.auth.forgotPassword, component: ForgotPasswordView, meta: { public: true }},
    { path: routes.auth.resetPassword, component: ResetPasswordView, meta: { public: true }},
    { path: routes.auth.confirmEmail, component: ConfirmEmailView, meta: { public: true }},
    { path: routes.auth.resendEmailConfirmation, component: ResendEmailConfirmationView, meta: { public: true }},
    { path: routes.balance.report, component: BalanceReportView, meta: { back: routes.home, title: 'Balance Report', subtitle: 'Your financial overview for the selected period.' }},
    { path: routes.transaction.history, component: TransactionHistoryView, meta: { back: routes.home, title: 'Transaction History', subtitle: "Every transaction you've logged, most recent first." }},
    { path: routes.transaction.create, component: TransactionCreateView, meta: { back: routes.transaction.history }},
    { path: `${routes.transaction.update()}/:id`, component: TransactionUpdateView, meta: { back: routes.transaction.history }},
    { path: routes.category.list, component: CategoryListView, meta: { back: routes.home, title: 'Categories', subtitle: 'Organize your transactions into meaningful groups.' }},
    { path: routes.category.create, component: CategoryCreateView, meta: { back: routes.category.list }},
    { path: `${routes.category.update()}/:id`, component: CategoryUpdateView, meta: { back: routes.category.list }},
    { path: routes.account.list, component: AccountListView, meta: { back: routes.home, title: 'Accounts', subtitle: 'Manage the accounts your transactions belong to.' }},
    { path: routes.account.create, component: AccountCreateView, meta: { back: routes.account.list }},
    { path: `${routes.account.update()}/:id`, component: AccountUpdateView, meta: { back: routes.account.list }},
    { path: '/:pathMatch(.*)*', redirect: routes.home }
];

const routeAuthGuard = async (route: RouteLocationNormalized) => {
    const authStore = useAuthStore();

    const isPublic = route.meta.public === true;

    const isRouteAuth = Object.values(routes.auth).includes(route.path);

    // If route is an auth route and user is already logged in → redirect to home
    if (isRouteAuth && authStore.accessToken) {
        return { path: routes.home };
    }

    // If route is public → allow through
    if (isPublic) {
        return true;
    }

    // If token is already stored → allow through
    if (authStore.accessToken) {
        return true;
    }

    // If silent refresh from HttpOnly cookie succeeds → allow through
    const refreshed = await authStore.refresh();
    if (refreshed) {
        return true;
    }

    // No valid session → redirect to login, preserve intended destination
    return { path: routes.auth.login, query: { redirect: route.fullPath }};
};

export { routes, routerConfig, routeAuthGuard };
