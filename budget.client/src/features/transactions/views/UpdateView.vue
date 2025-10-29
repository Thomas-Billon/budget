<script setup lang="ts">

    import { ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetailsResponse';
    import { type ITransactionRequest, getDefaultTransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';
    import useGetEntity from '@/composables/useGetEntity';
    import useUpdateEntity from '@/composables/useUpdateEntity';
    import useDeleteEntity from '@/composables/useDeleteEntity';
    import useMountedOrRouteParamUpdate from '@/composables/useMountedOrRouteParamUpdate';
    import { getIdFromRoute } from '@/utils/Route';

    const route = useRoute();
    const router = useRouter();

    const transaction = ref(getDefaultTransactionRequest());

    useMountedOrRouteParamUpdate((params) => {
        const id = getIdFromRoute(params?.id);

        if (id) {
            getEntity(id);
        }
    });

    const onGetError = () => {
        // TODO: Add error
    };

    const onFullUpdateSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const onDeleteSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const endpoint = 'transaction';

    const { entity, getEntity } = useGetEntity<ITransactionDetailsResponse>({ endpoint, onGetError });
    const { fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult } = useUpdateEntity<ITransactionRequest>({ endpoint, onFullUpdateSuccess });
    const { deleteEntity, deleteResult } = useDeleteEntity({ endpoint, onDeleteSuccess });

    // Convert entity from db to request object
    watch(entity, (result) => {
        if (result) {
            transaction.value = {
                id: result.id,
                type: result.type,
                amount: result.amount,
                reason: result.reason,
                date: result.date,
                paymentMethod: result.paymentMethod,
                comment: result.comment,
                categoryIds: result.categories.map(c => c.id),
            };
        }
    });

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
