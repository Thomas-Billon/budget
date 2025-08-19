<script setup lang="ts">
    import { routes } from '@/router.ts';
</script>

<template>
    <RouterLink :to="routes.transaction.list">Transactions</RouterLink>
</template>
