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

    const editTransaction = async (id: number) => {
        await apiCall(`transaction/${id}`, { method: 'PUT', body: transaction.value });
        router.push({ path: routes.transaction.list });
    };

</script>

<template>
    <ViewContainer :back-action="routes.transaction.list">
        <TransactionForm v-model="transaction" @submit="editTransaction" />
    </ViewContainer>
</template>
