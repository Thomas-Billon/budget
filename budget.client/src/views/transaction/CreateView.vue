<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router'
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/apiCall';
    import type { Transaction } from '@/models/Transaction.ts';
    import { DefaultTransaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
    import TransactionForm from '@/components/TransactionForm.vue';

    const router = useRouter();

    // TEMP
    let transactionEdit: Transaction = {
        id: 99,
        type: TransactionType.Income,
        amount: 3500,
        title: 'Hello world!',
        date: new Date().toISOString().split('T')[0],
        paymentMethod: PaymentMethod.BankTransfer,
        comment: 'Paycheck'
    };

    // TEMP
    const transaction = ref<Transaction>(null || { ...DefaultTransaction });
    //const transaction = ref<Transaction>({ ...DefaultTransaction });

    const create = async () => {
        await apiCall('transaction', { method: 'POST', body: transaction.value });
        router.push({ path: routes.transaction.list });
    };

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm v-model="transaction" @submit="create" />
    </ViewContainer>
</template>
