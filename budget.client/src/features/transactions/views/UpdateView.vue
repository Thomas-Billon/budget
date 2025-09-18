<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetaillResponse';
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';

    const route = useRoute();
    const router = useRouter();

    const transaction = ref<Partial<ITransactionDetailsResponse>>({});

    const saveAllResult = ref<ApiCallResult>();
    const savePartialResult = ref<ApiCallResult>();

    // Init
    onMounted(() => {
        const transactionId = parseInt(route.params.id as string);
        getTransactionById(transactionId);
    });

    // On route change
    watch(() => route.params.id, (id) => {
        const transactionId = parseInt(id as string);
        getTransactionById(transactionId);
    });

    const getTransactionById = async (id: number) => {
        apiCall<undefined, ITransactionDetailsResponse>(`transaction/${id}`, { method: 'GET' })
            .then(response => {
                transaction.value = response;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

    const updateTransactionAll = async (data: ITransactionRequest) => {
        apiCall(`transaction/${data.id}`, { method: 'PUT', body: data })
            .then(_ => {
                router.push({ path: routes.transaction.list });
            })
            .catch(() => {
                saveAllResult.value = {
                    isSuccess: false,
                    timestamp: Date.now()
                };
            });
    };

    const updateTransactionPartial = async (id: number, data: Partial<ITransactionRequest>) => {
        apiCall(`transaction/${id}`, { method: 'PATCH', body: data })
            .then(_ => {
                savePartialResult.value = {
                    isSuccess: true,
                    timestamp: Date.now()
                };
            })
            .catch(() => {
                savePartialResult.value = {
                    isSuccess: false,
                    timestamp: Date.now()
                };
            });
    };

</script>

<template>
    <TransactionForm :is-new="false"
                        :save-all-result="saveAllResult"
                        :save-partial-result="savePartialResult"
                        v-model="transaction"
                        @save-all="updateTransactionAll"
                        @save-partial="updateTransactionPartial" />
</template>
