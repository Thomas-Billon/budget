<script setup lang="ts">
    import './App.scss';

    import { computed, onMounted, ref } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import NavBar from '@/components/nav-bar/NavBar.vue';
    import PageHeader from '@/components/page-header/PageHeader.vue';

    const route = useRoute();
    const router = useRouter();

    const isReady = ref<boolean>(false);
    const isPublicRoute = computed<boolean>(() => route.meta.public === true);

    onMounted(async () => {
        await router.isReady();
        isReady.value = true;
    });
</script>

<template>
    <div class="page">
        <div v-if="!isReady" class="page-loader">
            <div class="spinner-border text-secondary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
        <template v-else>
            <NavBar v-if="!isPublicRoute" />
            <div :class="['page-content', isPublicRoute ? 'public' : '']">
                <PageHeader v-if="!isPublicRoute" :class="['container-with-navbar', {'no-text': !route.meta.title && !route.meta.subtitle}]" :title="route.meta.title ?? ''" :subtitle="route.meta.subtitle" />
                <main :class="['page-body', isPublicRoute ? 'container' : 'container-with-navbar']">
                    <RouterView />
                </main>
            </div>
        </template>
    </div>
</template>
