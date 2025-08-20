<script setup lang="ts">
    import { routes } from '@/router.ts';
    import ViewContainer from '@/components/ViewContainer.vue';
</script>

<template>
    <ViewContainer>
        <RouterLink :to="routes.transaction.list">Transactions</RouterLink>
    </ViewContainer>
</template>
