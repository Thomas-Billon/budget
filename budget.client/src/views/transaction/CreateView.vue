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

    const createTransaction = async () => {
        await apiCall('transaction', { method: 'POST', body: transaction.value });
        router.push({ path: routes.transaction.list });
    };

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm :is-new="true"
                         v-model="transaction"
                         @save-all="createTransaction" />
    </ViewContainer>
</template>
