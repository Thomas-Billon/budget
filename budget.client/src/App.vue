<script setup lang="ts">

    import './App.scss';

    import { ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router';

    const route = useRoute();
    const router = useRouter();

    const canGoBack = ref(false);

    // On route change
    watch(() => route.path, (_) => {
        canGoBack.value = !!router.options.history.state.back;
    });

    const goBack = (): void => {
        if (canGoBack.value) {
            router.back();
        }
    }

</script>

<template>
    <div class="page">
        <header>
            <div v-if="canGoBack">
                <button @click="goBack()" class="btn btn-link">
                    <font-awesome-icon icon="fa-solid fa-arrow-left" size="lg" />
                </button>
            </div>
        </header>
        <main class="section-container-grow">
            <RouterView />
        </main>
    </div>
</template>
