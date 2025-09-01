<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall';
    import type { ITransaction } from '@/features/transactions/ITransaction.ts';
    import { DefaultTransaction } from '@/features/transactions/ITransaction.ts';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';

    const router = useRouter();

    const transaction = ref<Transaction>({ ...DefaultTransaction });

    const saveAllResult = ref({ isSuccess: undefined, timestamp: Date.now() });

    const createTransaction = async (data: ITransaction) => {
        apiCall('transaction', { method: 'POST', body: data })
            .then(response => {
                router.push({ path: routes.transaction.list });
            })
            .catch(() => {
                saveAllResult.value = {
                    isSuccess: false,
                    timestamp: Date.now
                };
            });
    };

</script>

<template>
    <div class="page section-container container">
        <TransactionForm :is-new="true"
                         :save-all-result="saveAllResult"
                         v-model="transaction"
                         @save-all="createTransaction" />
    </div>
</template>
