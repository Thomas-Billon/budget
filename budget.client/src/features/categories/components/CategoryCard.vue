<script setup lang="ts">

    import './CategoryCard.scss';

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

    const getCardRouterLink = (): string => {
        if (id === undefined) {
            return routes.category.create(parentCategoryId);
        }
        else {
            return routes.category.update(id);
        }
    };

</script>

<template>
    <RouterLink v-color="colorHex" :to="getCardRouterLink()" class="category-card card">
        <div v-if="name">{{ name }}</div>
        <slot></slot>
        <CategoryCard
            v-for="(child, index) in subCategories"
            :id="child.id"
            :key="index"
            :name="child.name"
            :color-hex="child.colorHex"
            :parent-category-id="child.parentCategoryId"
            :sub-categories="child.subCategories"
            :can-add-sub-categories="true"
        />
        <CategoryCard v-if="canAddSubCategories" :parent-category-id="id">
            <font-awesome-icon icon="fa-solid fa-plus" />
        </CategoryCard>
    </RouterLink>
</template>
