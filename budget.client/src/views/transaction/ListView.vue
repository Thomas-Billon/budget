<script setup lang="ts">
    import urls from '@/router.ts';
</script>

<template>
    <RouterLink :to="urls.home">Home</RouterLink>
    <RouterLink :to="urls.transactionCreate">New</RouterLink>
</template>
