<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { useRoute } from 'vue-router'
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/apiCall';
    import type { Transaction } from '@/models/Transaction.ts';
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { formatAmount } from '@/services/TransactionService.ts'
    import ViewContainer from '@/components/ViewContainer.vue';

    const route = useRoute();

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

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <div class="section-container">
            <span>Id: {{ transaction.id }}</span>
            <span>Type: {{ TransactionType[transaction.type] }}</span>
            <span>Amount: {{ formatAmount(transaction.amount) }} €</span>
            <span>Title: {{ transaction.title }}</span>
            <span>Date: {{ transaction.date ?? 'N/A' }}</span>
            <span>Payment method: {{ PaymentMethod[transaction.paymentMethod] }}</span>
            <span>Comment: {{ transaction.comment != '' ? transaction.comment : 'N/A' }}</span>
        </div>
    </ViewContainer>
</template>
