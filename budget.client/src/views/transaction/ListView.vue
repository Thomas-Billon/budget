<script setup lang="ts">

    import { ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/apiCall';
    import { formatAmount } from '@/services/TransactionService.ts'
    import ViewContainer from '@/components/ViewContainer.vue';

    const transactions = ref<Transaction[]>([]);

    const list = async () => {
        const response = await apiCall('transaction', { method: 'GET' });

        transactions.value = response.transactions;
    };

    list();

</script>

<template>
    <ViewContainer :back-action="routes.home">
        <div class="section-container">
            <div class="flex justify-center gap-6">
                <RouterLink :to="routes.transaction.create" class="flex flex-col items-center">
                    <span class="flex items-center justify-center w-0 h-0 p-6 bg-neutral-800 rounded-full text-white">
                        <font-awesome-icon icon="fa-solid fa-plus" size="xl" />
                    </span>
                    Create
                </RouterLink>
            </div>

            <div>
                <div v-for="transaction in transactions" :key="transaction.id">
                    {{transaction.id}}
                    {{formatAmount(transaction.amount)}}
                </div>
            </div>
        </div>
    </ViewContainer>
</template>
