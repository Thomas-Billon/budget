<script setup lang="ts">

    import './CategoryCard.scss';

    import type { RouteLocationAsRelativeGeneric, RouteLocationAsPathGeneric } from 'vue-router';
    import { routes } from '@/router';

    interface Props {
        id?: number;
        name?: string;
        colorHex?: string;
        parentCategoryId?: number;
        subCategories?: Props[];

        canAddSubCategories?: boolean;
    };

    const { id, name, colorHex, parentCategoryId, subCategories, canAddSubCategories } = defineProps<Props>();

    const getCardRouterLink = (): string | RouteLocationAsRelativeGeneric | RouteLocationAsPathGeneric => {
        if (id !== undefined) {
            return routes.category.update(id);
        }
        else {
            return { path: routes.category.create(parentCategoryId) };
        }
    };

</script>

<template>
    <RouterLink :to="getCardRouterLink()" class="category-card" v-color="colorHex">
        <div v-if="name">{{ name }}</div>
        <slot />
        <CategoryCard v-for="(child, index) in subCategories" :key="index" v-bind="child" :can-add-sub-categories="true" />
        <CategoryCard v-if="canAddSubCategories" :parent-category-id="id">
            <font-awesome-icon icon="fa-solid fa-plus" />
        </CategoryCard>
    </RouterLink>
</template>
