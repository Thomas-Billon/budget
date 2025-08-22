<script setup lang="ts">
    const { backAction } = defineProps(['backAction']);
</script>

<template>
    <div class="w-full h-full min-h-screen bg-neutral-100">
        <div class="w-full h-full min-h-screen container py-8 flex flex-col">

            <div v-if="backAction" class="mb-8">
                <RouterLink :to="backAction">
                    <font-awesome-icon icon="fa-solid fa-arrow-left" size="lg" />
                </RouterLink>
            </div>

            <slot />

        </div>
    </div>
</template>
