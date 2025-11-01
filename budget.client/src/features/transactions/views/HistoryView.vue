<script setup lang="ts">

    import './HistoryView.scss';

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ITransactionHistoryResponse, type ITransactionHistoryItemResponse } from '@/features/transactions/models/ITransactionHistoryResponse';
    import { formatAmount } from '@/features/transactions/TransactionService.ts';
    import CategoryTag from '@/features/categories/components/CategoryTag.vue';

    const transactions = ref<ITransactionHistoryItemResponse[]>([]);
    const isLastPage = ref<boolean>(false);

    const itemNumberPerPage = 5;

    // Init
    onMounted(() => {
        getTransactionHistory(0, itemNumberPerPage);
    });

    const onSeeMoreClick = () => {
        getTransactionHistory(transactions?.value.length, itemNumberPerPage);
    };

    const getTransactionHistory = async (skip: number, take: number) => {
        apiCall<undefined, ITransactionHistoryResponse>(`transaction/history?skip=${skip}&take=${take}`, { method: 'GET' })
            .then(response => {
                transactions.value.push(...response.page);
                isLastPage.value = response.isLastPage;

                const ids = new Set(); // temp variable to keep track of duplicates
                transactions.value = transactions.value.filter(({ id }) => !ids.has(id) && ids.add(id));
            })
            .catch(() => {
                // TODO: Add error
            });
    };

</script>

<template>
    <div class="transaction-history section-container container">
        <div class="transaction-history-actions">
            <RouterLink :to="routes.transaction.create" class="transaction-history-action-link">
                <span class="transaction-history-action-icon btn btn-primary btn-circle">
                    <font-awesome-icon icon="fa-solid fa-plus" />
                </span>
                <span class="transaction-history-action-label">Create</span>
            </RouterLink>
        </div>

        <div class="transaction-history-items">
            <RouterLink v-for="transaction in transactions" :key="transaction.id" :to="routes.transaction.update(transaction.id)" class="transaction-history-item">
                <span class="transaction-history-item-icon"></span>
                <div class="transaction-history-item-details">
                    <span class="transaction-history-item-reason">{{ transaction.reason }}</span>
                    <span>{{ formatAmount(transaction.amount) }} €</span>
                </div>
                <div class="transaction-history-item-categories">
                    <CategoryTag v-for="category in transaction.categories" :key="category.id" v-bind="category"/>
                </div>
            </RouterLink>
        </div>

        <button :class="[ 'btn btn-outline-secondary', isLastPage ? 'disabled' : '' ]" @click="onSeeMoreClick()">
            <span>See more</span>
        </button>
    </div>
</template>
