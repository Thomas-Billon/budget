<script setup lang="ts">

    import './CategoryTag.scss';

    interface Props {
        name: string;
        colorHex?: string;
    };

    const { name, colorHex } = defineProps<Props>();

</script>

<template>
    <div v-color="colorHex" class="category-tag tag">
        <span>{{ name }}</span>
    </div>
</template>
