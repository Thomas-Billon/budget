<script setup lang="ts">

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import type { ITransaction } from '@/features/transactions/ITransaction.ts';
    import { formatAmount } from '@/features/transactions/TransactionService.ts'

    const transactions = ref<ITransaction[]>([]);
    const isLastPage = ref<boolean>(false);

    const itemNumberPerPage = 15;

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
    <div class="container">
        <div class="flex justify-center gap-6">
            <RouterLink :to="routes.transaction.create" class="flex flex-col items-center gap-1">
                <span class="button primary circle text-xl">
                    <font-awesome-icon icon="fa-solid fa-plus" />
                </span>
                <span class="font-semibold">Create</span>
            </RouterLink>
        </div>

        <div class="flex flex-col gap-2">
            <RouterLink :to="routes.transaction.update(transaction.id)" v-for="transaction in transactions" :key="transaction.id" class="flex items-center gap-4">
                <span class="w-2 h-2 bg-neutral-200 rounded-full"></span>
                <div class="grow flex flex-col gap-1">
                    <span class="font-bold">{{ transaction.title }}</span>
                    <span>{{ formatAmount(transaction.amount) }} €</span>
                </div>
            </RouterLink>
        </div>

        <button :class="[ isLastPage ? 'd-none' : '' ]" @click="onSeeMoreClick()">
            See more
        </button>
    </div>
</template>
