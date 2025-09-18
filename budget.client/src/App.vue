<script setup lang="ts">

    import './App.scss';

    import { ref, watch } from 'vue';
    import { useRoute } from 'vue-router';

    const route = useRoute();

    const backLink = ref<string | undefined>();

    // On route change
    watch(() => route.path, (_) => {
        backLink.value = route.meta.back as string | undefined;
    });

</script>

<template>
    <div class="page">
        <header class="header container">
            <RouterLink v-if="backLink" :to="backLink" class="header-item btn btn-link">
                <font-awesome-icon icon="fa-solid fa-arrow-left" size="lg" />
            </RouterLink>
        </header>
        <main class="section-container-grow">
            <RouterView />
        </main>
    </div>
</template>
