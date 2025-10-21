import HomeView from '@/views/HomeView.vue';
import BalanceDetailsView from '@/features/balances/views/DetailsView.vue';
import TransactionHistoryView from '@/features/transactions/views/HistoryView.vue';
import TransactionCreateView from '@/features/transactions/views/CreateView.vue';
import TransactionUpdateView from '@/features/transactions/views/UpdateView.vue';
import CategoryListView from '@/features/categories/views/ListView.vue';
import CategoryCreateView from '@/features/categories/views/CreateView.vue';
import CategoryUpdateView from '@/features/categories/views/UpdateView.vue';

const getIdParam = (id: number): string => {
    if (id > 0) {
        return id.toString();
    }

    return '';
};

const routes = {
    home: '/',
    balance: {
        details: '/balance/details',
    },
    transaction: {
        history: '/transaction/history',
        create: '/transaction/create',
        update: (id: number) => `/transaction/update/${getIdParam(id)}`,
    },
    category: {
        list: '/category/list',
        create: '/category/create',
        update: (id: number) => `/category/update/${getIdParam(id)}`,
    }
};

const routerConfig = [
    { path: routes.home, component: HomeView },
    { path: routes.balance.details, component: BalanceDetailsView, meta: { back: routes.home } },
    { path: routes.transaction.history, component: TransactionHistoryView, meta: { back: routes.home } },
    { path: routes.transaction.create, component: TransactionCreateView, meta: { back: routes.transaction.history } },
    { path: `${routes.transaction.update(0)}:id`, component: TransactionUpdateView, meta: { back: routes.transaction.history } },
    { path: routes.category.list, component: CategoryListView, meta: { back: routes.home } },
    { path: routes.category.create, component: CategoryCreateView, meta: { back: routes.category.list } },
    { path: `${routes.category.update(0)}:id`, component: CategoryUpdateView, meta: { back: routes.category.list } },
];

export { routes, routerConfig };
