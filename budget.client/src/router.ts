import HomeView from '@/views/HomeView.vue';
import TransactionListView from '@/views/transaction/ListView.vue';
import TransactionCreateView from '@/views/transaction/CreateView.vue';
import TransactionEditView from '@/views/transaction/EditView.vue';

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
        edit: (id: number) => `/transaction/edit/${getIdParam(id)}`,
    },
};

const routerConfig = [
    { path: routes.home, component: HomeView },
    { path: routes.transaction.list, component: TransactionListView },
    { path: routes.transaction.create, component: TransactionCreateView },
    { path: `${routes.transaction.edit(0)}:id`, component: TransactionEditView }
];

export { routes, routerConfig };
