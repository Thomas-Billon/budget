<script setup lang="ts">

    import './ClickableRow.scss';

    interface Props {
        target: string;
    };

    defineProps<Props>();

</script>

<template>
    <div class="clickable-row">
        <RouterLink type="button" class="clickable-row-link" :to="target">
            <span class="clickable-row-decorator"></span>
            <div class="clickable-row-content">
                <slot></slot>
            </div>
        </RouterLink>
    </div>
</template>
