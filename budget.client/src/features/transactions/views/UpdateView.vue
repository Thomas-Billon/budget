<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router'
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall';
    import type { ITransaction } from '@/features/transactions/ITransaction.ts';
    import { DefaultTransaction } from '@/features/transactions/ITransaction.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
    import TransactionForm from '@/features/transactions/components/TransactionForm.vue';

    const route = useRoute();
    const router = useRouter();

    const transaction = ref<Transaction>({ ...DefaultTransaction });

    const saveAllResult = ref({ isSuccess: undefined, timestamp: Date.now() });
    const savePartialResult = ref({ isSuccess: undefined, timestamp: Date.now() });

    // Init
    onMounted(() => {
        getTransactionById(route.params.id);
    });

    // On route change
    watch(() => route.params.id, (id) => {
        getTransactionById(id);
    });

    const getTransactionById = async (id: number) => {
        apiCall(`transaction/${id}`, { method: 'GET' })
            .then(response => {
                transaction.value = response;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

    const updateTransactionAll = async (data: ITransaction) => {
        apiCall(`transaction/${data.id}`, { method: 'PUT', body: data })
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

    const updateTransactionPartial = async (id: number, data: { [P in keyof Transaction]: Transaction[P] }) => {
        apiCall(`transaction/${id}`, { method: 'PATCH', body: data })
            .then(response => {
                savePartialResult.value = {
                    isSuccess: true,
                    timestamp: Date.now
                };
            })
            .catch(() => {
                savePartialResult.value = {
                    isSuccess: false,
                    timestamp: Date.now
                };
            });
    };

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm :is-new="false"
                         :save-all-result="saveAllResult"
                         :save-partial-result="savePartialResult"
                         v-model="transaction"
                         @save-all="updateTransactionAll"
                         @save-partial="updateTransactionPartial" />
    </ViewContainer>
</template>
