<script setup lang="ts">
    import { ref } from 'vue';
    import urls from '@/router.ts';
    import type { Transaction } from '@/models/Transaction.ts';
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import TransactionForm from '@/components/TransactionForm.vue';

    const transactionEdit = {
        id: 99,
        type: TransactionType.Income,
        amount: 3500,
        date: new Date().toISOString().split('T')[0],
        paymentMethod: PaymentMethod.BankTransfer,
        comment: 'Paycheck',
    } as Transaction;

    const transaction = ref(null || DefaultTransaction);
    
    const pince = async () => {
        const response = await fetch('https://localhost:7177/transaction', {
            method: 'POST',
            body: JSON.stringify(transaction.value),
        });
        if (response.ok) {
            console.log('Response is OK');
        }
    }

</script>

<template>
    <div class="page">
        <div class="page-container flex flex-col gap-4">

            <RouterLink :to="urls.transactionList">
                <font-awesome-icon icon="fa-solid fa-arrow-left" size="lg" />
            </RouterLink>

            <TransactionForm v-model="transaction" @submit="pince" />

        </div>
    </div>
</template>
