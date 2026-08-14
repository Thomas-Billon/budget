<script setup lang="ts">

    import './WidgetCard.scss';

    interface Props {
        title: string;
        icon?: string;
    };

    const { title, icon } = defineProps<Props>();

</script>

<template>
    <div class="widget-card card">
        <div class="widget-card-head">
            <font-awesome-icon v-if="icon" :icon="`fa-solid fa-${icon}`" fixed-width />
            <span class="widget-card-title">{{ title }}</span>
        </div>
        <div class="widget-card-body">
            <slot></slot>
        </div>
    </div>
</template>
