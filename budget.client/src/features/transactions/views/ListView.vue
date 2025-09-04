<script setup lang="ts">

    import './ListView.scss';

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import type { ITransaction } from '@/features/transactions/ITransaction.ts';
    import { formatAmount } from '@/features/transactions/TransactionService.ts'

    const transactions = ref<ITransaction[]>([]);
    const isLastPage = ref<boolean>(false);

    const itemNumberPerPage = 5;

    // Init
    onMounted(() => {
        getListTransaction(0, itemNumberPerPage);
    });

    const onSeeMoreClick = () => {
        getListTransaction(transactions?.value.length, itemNumberPerPage);
    };

    const getListTransaction = async (skip: number, take: number) => {
        apiCall(`transaction?skip=${skip}&take=${take}`, { method: 'GET' })
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
    <div class="transaction-list container">
        <div class="transaction-list-actions">
            <RouterLink :to="routes.transaction.create" class="transaction-list-action-link">
                <span class="transaction-list-action-icon btn btn-primary btn-circle">
                    <font-awesome-icon icon="fa-solid fa-plus" />
                </span>
                <span class="transaction-list-action-label">Create</span>
            </RouterLink>
        </div>

        <div class="transaction-list-items">
            <RouterLink :to="routes.transaction.update(transaction.id)" v-for="transaction in transactions" :key="transaction.id" class="transaction-list-item">
                <span class="transaction-list-item-icon"></span>
                <div class="transaction-list-item-details">
                    <span class="transaction-list-item-title">{{ transaction.title }}</span>
                    <span>{{ formatAmount(transaction.amount) }} €</span>
                </div>
            </RouterLink>
        </div>

        <button :class="[ 'btn btn-outline-secondary', isLastPage ? 'd-none' : '' ]" @click="onSeeMoreClick()">
            See more
        </button>
    </div>
</template>
