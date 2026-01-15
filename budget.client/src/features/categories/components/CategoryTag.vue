<script setup lang="ts">

    import './CategoryTag.scss';

    interface Props {
        name: string;
        colorHex?: string;
    };

    const { name, colorHex } = defineProps<Props>();

</script>

<template>
    <div class="category-tag tag" v-color="colorHex">
        <span>{{ name }}</span>
    </div>
</template>
