<script setup lang="ts">
    import { routes } from '@/router.ts';
</script>

<template>
    <div class="container">
        <RouterLink :to="routes.balance.report">Balance</RouterLink>
        <RouterLink :to="routes.transaction.history">Transactions</RouterLink>
        <RouterLink :to="routes.category.hierarchy">Categories</RouterLink>
    </div>
</template>
