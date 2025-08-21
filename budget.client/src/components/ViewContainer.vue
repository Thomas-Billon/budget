<script setup lang="ts">
    const props = defineProps(['backAction']);
</script>

<template>
    <div class="w-full h-full min-h-screen bg-neutral-100">
        <div class="w-full h-full min-h-screen container py-8 flex flex-col">

            <div v-if="props.backAction" class="mb-8">
                <RouterLink :to="props.backAction">
                    <font-awesome-icon icon="fa-solid fa-arrow-left" size="lg" />
                </RouterLink>
            </div>

            <slot />

        </div>
    </div>
</template>
