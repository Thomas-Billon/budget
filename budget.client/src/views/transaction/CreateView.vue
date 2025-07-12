<script setup lang="ts">
    import { ref } from 'vue';
    import urls from '@/router.ts';
    import type { Transaction } from '@/models/Transaction.ts';
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import TransactionForm from '@/components/TransactionForm.vue';
    import { apiCall } from '@/utils/apiCall';

    const transactionEdit = {
        id: 99,
        type: TransactionType.Income,
        amount: 3500,
        date: new Date().toISOString().split('T')[0],
        paymentMethod: PaymentMethod.BankTransfer,
        comment: 'Paycheck'
    } as Transaction;

    const transaction = ref(null || DefaultTransaction);

    const create = async () => {
        await apiCall('transaction', { method: 'POST', body: transaction.value });
    };

</script>

<template>
    <div class="page">
        <div class="page-container flex flex-col gap-4">

            <RouterLink :to="urls.transactionList">
                <font-awesome-icon icon="fa-solid fa-arrow-left" size="lg" />
            </RouterLink>

            <TransactionForm v-model="transaction" @submit="create" />

        </div>
    </div>
</template>
