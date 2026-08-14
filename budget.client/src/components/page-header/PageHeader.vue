<script setup lang="ts">

    import './PageHeader.scss';

    interface Props {
        title: string;
        subtitle?: string;
    };

    const { title, subtitle } = defineProps<Props>();

</script>

<template>
    <header class="page-header">
        <div class="page-header-text d-none d-md-flex">
            <h1 class="page-header-title">{{ title }}</h1>
            <p v-if="subtitle" class="page-header-subtitle">{{ subtitle }}</p>
        </div>
        <div class="page-header-actions"></div>
    </header>
</template>
