<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router'
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/apiCall';
    import type { Transaction } from '@/models/Transaction.ts';
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
    import TransactionForm from '@/components/TransactionForm.vue';

    const router = useRouter();

    const transaction = ref<Transaction>({ ...DefaultTransaction });

    const saveAllResult = ref({ isSuccess: undefined, timestamp: Date.now() });

    const createTransaction = async () => {
        const response = await apiCall('transaction', { method: 'POST', body: transaction.value });

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

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm :is-new="true"
                         :save-all-result="saveAllResult"
                         v-model="transaction"
                         @save-all="createTransaction" />
    </ViewContainer>
</template>
