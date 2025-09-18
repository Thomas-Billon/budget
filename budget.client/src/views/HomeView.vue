<script setup lang="ts">
    import { routes } from '@/router.ts';
</script>

<template>
    <div class="container">
        <RouterLink :to="routes.transaction.list">Transactions</RouterLink>
        <RouterLink :to="routes.category.list">Categories</RouterLink>
    </div>
</template>
