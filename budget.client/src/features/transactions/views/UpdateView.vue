<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetailsResponse';
    import { type ITransactionRequest, defaultTransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';
    import useUpdateEntity from '@/composables/useUpdateEntity';
    import useDeleteEntity from '@/composables/useDeleteEntity';

    const router = useRouter();

    const onGetDetailsError = () => {
        // TODO: Add error
    };

    const onFullUpdateSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const onDeleteSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const mapResponseToRequest = (response: ITransactionDetailsResponse): ITransactionRequest => {
        return {
            id: response.id,
            type: response.type,
            amount: response.amount,
            reason: response.reason,
            date: response.date,
            paymentMethod: response.paymentMethod,
            comment: response.comment,
            categoryIds: response.categories.map(c => c.id)
        };
    };

    const { entity: transaction, fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult } = useUpdateEntity<ITransactionRequest, ITransactionDetailsResponse>({ endpoint: 'transaction', defaultEntity: defaultTransactionRequest, mapResponseToRequest, onGetDetailsError, onFullUpdateSuccess });
    const { deleteEntity, deleteResult } = useDeleteEntity({ endpoint: 'transaction', onDeleteSuccess });

</script>

<template>
    <TransactionForm :is-new="false"
                     :save-all-result="fullUpdateResult"
                     :save-partial-result="partialUpdateResult"
                     :delete-result="deleteResult"
                     v-model="transaction"
                     @save-all="fullUpdateEntity"
                     @save-partial="partialUpdateEntity"
                     @delete="deleteEntity" />
</template>
