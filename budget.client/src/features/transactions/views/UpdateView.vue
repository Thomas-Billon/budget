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

    const onUpdateSuccess = () => {
        router.push({ path: routes.transaction.list });
    };

    const { entity: transaction, updateEntity, updateResult, updateEntityPartial, updatePartialResult, } = useUpdateEntity<ITransactionRequest, ITransactionDetailsResponse>({ endpoint: 'transaction', onGetByIdError, onUpdateSuccess });

</script>

<template>
    <TransactionForm :is-new="false"
                     :save-all-result="updateResult"
                     :save-partial-result="updatePartialResult"
                     v-model="transaction"
                     @save-all="updateEntity"
                     @save-partial="updateEntityPartial" />
</template>
