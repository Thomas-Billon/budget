import HomeView from '@/views/HomeView.vue';
import TransactionListView from '@/views/transaction/ListView.vue';
import TransactionCreateView from '@/views/transaction/CreateView.vue';

const routes = {
    home: '/',
    transaction: {
        list: '/transaction/list',
        create: '/transaction/create'
    },
};

const routerConfig = [
    { path: routes.home, component: HomeView },
    { path: routes.transaction.list, component: TransactionListView },
    { path: routes.transaction.create, component: TransactionCreateView }
];

export { routes, routerConfig };
