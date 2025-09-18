<script setup lang="ts">

    import './ListView.scss';

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ITransactionListResponse, type ITransactionListItemResponse } from '@/features/transactions/models/ITransactionListResponse';
    import { formatAmount } from '@/features/transactions/TransactionService.ts';

    const transactions = ref<ITransactionListItemResponse[]>([]);
    const isLastPage = ref<boolean>(false);

    const itemNumberPerPage = 5;

    // Init
    onMounted(() => {
        getTransactionList(0, itemNumberPerPage);
    });

    const onSeeMoreClick = () => {
        getTransactionList(transactions?.value.length, itemNumberPerPage);
    };

    const getTransactionList = async (skip: number, take: number) => {
        apiCall<undefined, ITransactionListResponse>(`transaction?skip=${skip}&take=${take}`, { method: 'GET' })
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
    <div class="transaction-list section-container container">
        <div class="transaction-list-actions">
            <RouterLink :to="routes.transaction.create" class="transaction-list-action-link">
                <span class="transaction-list-action-icon btn btn-primary btn-circle">
                    <font-awesome-icon icon="fa-solid fa-plus" />
                </span>
                <span class="transaction-list-action-label">Create</span>
            </RouterLink>
        </div>

        <div class="transaction-list-items">
            <RouterLink v-for="transaction in transactions" :key="transaction.id" :to="routes.transaction.update(transaction.id)" class="transaction-list-item">
                <span class="transaction-list-item-icon"></span>
                <div class="transaction-list-item-details">
                    <span class="transaction-list-item-reason">{{ transaction.reason }}</span>
                    <span>{{ formatAmount(transaction.amount) }} €</span>
                </div>
            </RouterLink>
        </div>

        <button :class="[ 'btn btn-outline-secondary', isLastPage ? 'disabled' : '' ]" @click="onSeeMoreClick()">
            See more
        </button>
    </div>
</template>
