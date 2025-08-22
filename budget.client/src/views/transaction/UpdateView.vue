<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router'
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/apiCall';
    import type { Transaction } from '@/models/Transaction.ts';
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
    import TransactionForm from '@/components/TransactionForm.vue';

    const route = useRoute();
    const router = useRouter();

    const transaction = ref<Transaction>({ ...DefaultTransaction });

    // Init
    onMounted(() => {
        getTransactionById(route.params.id);
    });

    // On route change
    watch(() => route.params.id, (id) => {
        getTransactionById(id);
    });

    const getTransactionById = async (id: number) => {
        transaction.value = await apiCall(`transaction/${id}`, { method: 'GET' });
    };

    const updateTransaction = async (id: number) => {
        await apiCall(`transaction/${id}`, { method: 'PUT', body: transaction.value });
        router.push({ path: routes.transaction.list });
    };

    const updateTransactionField = (id: number, field: string) => {
        switch (field) {
            case 'type':
                return updateTransactionType(id);

            case 'amount':
                return updateTransactionAmount(id);

            case 'title':
                return updateTransactionTitle(id);

            case 'date':
                return updateTransactionDate(id);

            case 'paymentMethod':
                return updateTransactionPaymentMethod(id);

            case 'comment':
                return updateTransactionComment(id);
        }
    };

    const updateTransactionType = async (id: number) => {
        await apiCall(`transaction/type/${id}`, { method: 'PATCH', body: transaction.value.type });
    };

    const updateTransactionAmount = async (id: number) => {
        await apiCall(`transaction/amount/${id}`, { method: 'PATCH', body: transaction.value.amount });
    };

    const updateTransactionTitle = async (id: number) => {
        await apiCall(`transaction/title/${id}`, { method: 'PATCH', body: transaction.value.title });
    };

    const updateTransactionDate = async (id: number) => {
        await apiCall(`transaction/date/${id}`, { method: 'PATCH', body: transaction.value.date });
    };

    const updateTransactionPaymentMethod = async (id: number) => {
        await apiCall(`transaction/payment-method/${id}`, { method: 'PATCH', body: transaction.value.paymentMethod });
    };

    const updateTransactionComment = async (id: number) => {
        await apiCall(`transaction/comment/${id}`, { method: 'PATCH', body: transaction.value.comment });
    };

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm :is-new="false"
                         v-model="transaction"
                         @save-all="updateTransaction"
                         @save-partial="updateTransactionField" />
    </ViewContainer>
</template>
