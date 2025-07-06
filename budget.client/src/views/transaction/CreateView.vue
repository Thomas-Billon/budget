<script setup lang="ts">
    import { ref, defineModel } from 'vue';
    import type { Transaction } from '@/models/Transaction.ts'
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts'
    import { PaymentMethod } from '@/enums/PaymentMethod.ts'
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
</script>

<template>
    <TransactionForm v-model="transaction" />
</template>
