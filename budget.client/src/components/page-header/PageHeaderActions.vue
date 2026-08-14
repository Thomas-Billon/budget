<script setup lang="ts">

    import './PageHeaderActions.scss';

</script>

<template>
    <Teleport defer to=".page-header-actions">
        <slot></slot>
    </Teleport>
</template>
