<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetailsResponse';
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';
    import useUpdateEntity from '@/composables/useUpdateEntity';

    const router = useRouter();

    const onGetByIdError = () => {
        // TODO: Add error
    };

    const onFullUpdateSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const { entity: transaction, fullUpdate, fullUpdateResult, partialUpdate, partialUpdateResult } = useUpdateEntity<ITransactionRequest, ITransactionDetailsResponse>({ endpoint: 'transaction', onGetByIdError, onFullUpdateSuccess });

</script>

<template>
    <TransactionForm :is-new="false"
                     :save-all-result="fullUpdateResult"
                     :save-partial-result="partialUpdateResult"
                     v-model="transaction"
                     @save-all="fullUpdate"
                     @save-partial="partialUpdate" />
</template>
