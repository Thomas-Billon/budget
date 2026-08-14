<script setup lang="ts">

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ITransactionHistoryResponse, type ITransactionHistoryItemResponse } from '@/features/transactions/models/ITransactionHistoryResponse';
    import TransactionRow from '@/features/transactions/components/TransactionRow.vue';
    import PageHeaderActions from '@/components/page-header/PageHeaderActions.vue';

    const isLoading = ref<boolean>(true);
    const transactions = ref<ITransactionHistoryItemResponse[]>([]);
    const isLastPage = ref<boolean>(false);

    const itemNumberPerPage = 10;

    const getTransactionHistory = (skip: number, take: number) => {
        apiCall<undefined, ITransactionHistoryResponse>(`transaction/history?skip=${skip}&take=${take}`, { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    transactions.value.push(...response.data.page);
                    isLastPage.value = response.data.isLastPage;

                    const ids = new Set(); // temp variable to keep track of duplicates
                    transactions.value = transactions.value.filter(({ id }) => !ids.has(id) && ids.add(id));
                }
                // TODO: Handle error in else case
                isLoading.value = false;
            });
    };

    // Init
    onMounted(() => {
        getTransactionHistory(0, itemNumberPerPage);
    });

    const onSeeMoreClick = () => {
        getTransactionHistory(transactions?.value.length, itemNumberPerPage);
    };

</script>

<template>
    <div class="transaction-history">
        <PageHeaderActions>
            <RouterLink :to="routes.transaction.create" class="btn btn-primary btn-lg w-100">
                <font-awesome-icon icon="fa-solid fa-plus" />
                <span>Add transaction</span>
            </RouterLink>
        </PageHeaderActions>

        <div v-if="!isLoading" class="card">
            <div class="d-flex flex-column">
                <TransactionRow
                    v-for="(transaction, index) in transactions"
                    :key="index"
                    :transaction="transaction"
                    class="in-card"
                />
            </div>

            <button class="btn btn-outline-secondary" :class="{ 'disabled': isLastPage }" @click="onSeeMoreClick()">
                <span>See more</span>
            </button>
        </div>

        <div v-else>Loading…</div>
    </div>
</template>
