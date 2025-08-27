<script setup lang="ts">

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
    import { formatAmount } from '@/features/transactions/TransactionService.ts'

    const transactions = ref<Transaction[]>([]);

    // Init
    onMounted(() => {
        getListTransaction();
    });

    const getListTransaction = async () => {
        const response = await apiCall('transaction', { method: 'GET' });
        transactions.value = response.page;
    };

</script>

<template>
    <ViewContainer :back-action="routes.home">
        <div class="section-container">
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
        </div>
    </ViewContainer>
</template>
