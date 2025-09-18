import HomeView from '@/views/HomeView.vue';
import TransactionListView from '@/features/transactions/views/ListView.vue';
import TransactionCreateView from '@/features/transactions/views/CreateView.vue';
import TransactionUpdateView from '@/features/transactions/views/UpdateView.vue';

const getIdParam = (id: number): string => {
    if (id > 0) {
        return id.toString();
    }

    return '';
};

const routes = {
    home: '/',
    transaction: {
        list: '/transaction/list',
        create: '/transaction/create',
        update: (id: number) => `/transaction/update/${getIdParam(id)}`,
    },
};

const routerConfig = [
    { path: routes.home, component: HomeView },
    { path: routes.transaction.list, component: TransactionListView, meta: { back: routes.home } },
    { path: routes.transaction.create, component: TransactionCreateView, meta: { back: routes.transaction.list } },
    { path: `${routes.transaction.update(0)}:id`, component: TransactionUpdateView, meta: { back: routes.transaction.list } },
];

export { routes, routerConfig };
