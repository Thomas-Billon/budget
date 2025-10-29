<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ITransactionRequest, defaultTransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetailsResponse';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';

    const router = useRouter();

    const onCreateSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const { entity: transaction, createEntity, createResult } = useCreateEntity<ITransactionRequest, ITransactionDetailsResponse>({ endpoint: 'transaction', defaultEntity: defaultTransactionRequest, onCreateSuccess });

</script>

<template>
    <TransactionForm :is-new="true"
                     :save-all-result="createResult"
                     v-model="transaction"
                     @save-all="createEntity" />
</template>
