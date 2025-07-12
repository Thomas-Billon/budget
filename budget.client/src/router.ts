import HomeView from '@/views/HomeView.vue';
import TransactionListView from '@/views/transaction/ListView.vue';
import TransactionCreateView from '@/views/transaction/CreateView.vue';

const urls = {
    home: '/',
    transactionList: '/transaction/list',
    transactionCreate: '/transaction/create'
};

const routes = [
    { path: urls.home, component: HomeView },
    { path: urls.transactionList, component: TransactionListView },
    { path: urls.transactionCreate, component: TransactionCreateView }
];

export default urls;
export { urls, routes };
