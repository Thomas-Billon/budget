<script setup lang="ts">

    import './NavBar.scss';

    import { useRoute, useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { useAuthStore } from '@/stores/useAuthStore';
    import useNavigation from '@/composables/useNavigation';
    import BrandLogo from '@/components/brand-logo/BrandLogo.vue';

    const route = useRoute();
    const router = useRouter();
    const authStore = useAuthStore();
    const { canGoBack, goBack } = useNavigation();

    const navItems = [
        { target: routes.home, label: 'Home', icon: 'house' },
        { target: routes.balance.report, label: 'Balance', icon: 'landmark' },
        { target: routes.transaction.history, label: 'Transactions', icon: 'receipt' },
        { target: routes.category.list, label: 'Categories', icon: 'tags' }
    ];

    const isActive = (target: string): boolean => {
        const targetRoot = target.split('/')[1];
        return route.path === target || (!!targetRoot && route.path.startsWith(`/${targetRoot}`));
    };

    const logout = async (): Promise<void> => {
        await authStore.logout();
        await router.push(routes.auth.login);
    };

</script>

<template>
    <!-- Desktop sidebar -->
    <nav class="nav-bar-sidebar d-none d-md-flex flex-column">
        <div class="nav-bar-sidebar-head">
            <BrandLogo class="inline" />
        </div>

        <div class="nav-bar-sidebar-body">
            <RouterLink v-for="(item, index) in navItems" :key="index" :to="item.target" class="nav-bar-sidebar-item" :class="{ 'active': isActive(item.target) }">
                <font-awesome-icon :icon="`fa-solid fa-${item.icon}`" fixed-width />
                <span class="nav-bar-sidebar-item-text">{{ item.label }}</span>
            </RouterLink>
        </div>

        <div class="nav-bar-sidebar-foot">
            <button type="button" class="nav-bar-sidebar-item" @click="logout">
                <font-awesome-icon icon="fa-solid fa-right-from-bracket" fixed-width />
                <span class="nav-bar-sidebar-item-text">Sign out</span>
            </button>
        </div>
    </nav>

    <!-- Mobile top bar -->
    <div class="nav-bar-topbar d-flex d-md-none">
        <div class="nav-bar-topbar-container container">
            <button v-if="canGoBack()" type="button" class="nav-bar-topbar-back" @click="goBack()">
                <font-awesome-icon icon="fa-solid fa-arrow-left" fixed-width />
            </button>
            <div class="nav-bar-topbar-title">{{ route.meta.title }}</div>
        </div>
    </div>

    <!-- Mobile bottom bar -->
    <nav class="nav-bar-bottombar d-flex d-md-none">
        <div class="nav-bar-bottombar-container container">
            <RouterLink v-for="(item, index) in navItems" :key="index" :to="item.target" class="nav-bar-bottombar-item" :class="{ 'active': isActive(item.target) }">
                <font-awesome-icon :icon="`fa-solid fa-${item.icon}`" fixed-width />
                <span class="nav-bar-bottombar-item-text">{{ item.label }}</span>
            </RouterLink>
        </div>
    </nav>
</template>
