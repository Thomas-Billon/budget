<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ITransactionRequest, getDefaultTransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';

    const router = useRouter();

    const transaction = ref(getDefaultTransactionRequest());

    const onCreateSuccess = () => {
        router.push({ path: routes.transaction.history });
    };

    const { createEntity, createResult } = useCreateEntity<ITransactionRequest>({ endpoint: 'transaction', onCreateSuccess });

</script>

<template>
    <TransactionForm :is-new="true"
                     :save-all-result="createResult"
                     v-model="transaction"
                     @save-all="createEntity" />
</template>
