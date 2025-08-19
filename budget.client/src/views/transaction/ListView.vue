<script setup lang="ts">
    import { ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/apiCall';

    const transactions = ref<Transaction[]>([]);

    const list = async () => {
        const response = await apiCall('transaction', { method: 'GET' });

        transactions.value = response.transactions;
    };

    list();
</script>

<template>
    <RouterLink :to="routes.home">Home</RouterLink>
    <RouterLink :to="routes.transaction.create">New</RouterLink>

    <div v-for="transaction in transactions" :key="transaction.id">
        {{transaction.id}}
        {{transaction.amount}}
    </div>
</template>
