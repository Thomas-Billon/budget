<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router'
    import { routes } from '@/router.ts';
    import { apiCall } from '@/helpers/ApiCall';
    import type { Transaction } from '@/features/transactions/ITransaction.ts';
    import { DefaultTransaction } from '@/features/transactions/ITransaction.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';

    const route = useRoute();
    const router = useRouter();

    const transaction = ref<Transaction>({ ...DefaultTransaction });

    const saveAllResult = ref({ isSuccess: undefined, timestamp: Date.now() });
    const savePartialResult = ref({ isSuccess: undefined, timestamp: Date.now() });

    // Init
    onMounted(() => {
        getTransactionById(route.params.id);
    });

    // On route change
    watch(() => route.params.id, (id) => {
        getTransactionById(id);
    });

    const getTransactionById = async (id: number) => {
        transaction.value = await apiCall(`transaction/${id}`, { method: 'GET' });
    };

    const updateTransactionAll = async (id: number) => {
        const response = await apiCall(`transaction/${id}`, { method: 'PUT', body: transaction.value });

        if (response.isSuccess) {
            router.push({ path: routes.transaction.list });
        }
        else {
            saveAllResult.value = {
                isSuccess: response.isSuccess,
                timestamp: Date.now
            };
        }
    };

    const updateTransactionPartial = async (id: number) => {
        const response = await apiCall(`transaction/${id}`, { method: 'PATCH', body: transaction.value });

        savePartialResult.value = {
            isSuccess: response.isSuccess,
            timestamp: Date.now
        };
    };

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm :is-new="false"
                         :save-all-result="saveAllResult"
                         :save-partial-result="savePartialResult"
                         v-model="transaction"
                         @save-all="updateTransactionAll"
                         @save-partial="updateTransactionPartial" />
    </ViewContainer>
</template>
