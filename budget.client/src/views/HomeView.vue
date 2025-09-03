<script setup lang="ts">
    import { routes } from '@/router.ts';
</script>

<template>
    <div class="container">
        <RouterLink :to="routes.transaction.list">Transactions</RouterLink>
    </div>
</template>
