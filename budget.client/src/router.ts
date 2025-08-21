import HomeView from '@/views/HomeView.vue';
import TransactionListView from '@/views/transaction/ListView.vue';
import TransactionCreateView from '@/views/transaction/CreateView.vue';
import TransactionDetailsView from '@/views/transaction/DetailsView.vue';
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
        details: (id: number) => `/transaction/details/${getIdParam(id)}`,
        edit: (id: number) => `/transaction/edit/${getIdParam(id)}`,
    },
};

const routerConfig = [
    { path: routes.home, component: HomeView },
    { path: routes.transaction.list, component: TransactionListView },
    { path: routes.transaction.create, component: TransactionCreateView },
    { path: `${routes.transaction.details(0)}:id`, component: TransactionDetailsView },
    { path: `${routes.transaction.edit(0)}:id`, component: TransactionEditView }
];

export { routes, routerConfig };
