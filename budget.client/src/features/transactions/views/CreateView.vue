<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetaillResponse';
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';

    const router = useRouter();

    const transaction = ref<ITransactionDetailsResponse>();

    const saveAllResult = ref<ApiCallResult>();

    const createTransaction = async (data: ITransactionRequest) => {
        apiCall<ITransactionRequest, undefined>('transaction', { method: 'POST', body: data })
            .then(_ => {
                router.push({ path: routes.transaction.list });
            })
            .catch(() => {
                saveAllResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    };

</script>

<template>
    <TransactionForm :is-new="true"
                        :save-all-result="saveAllResult"
                        v-model="transaction"
                        @save-all="createTransaction" />
</template>
