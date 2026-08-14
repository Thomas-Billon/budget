<script setup lang="ts">

    import './CategoryTag.scss';

    import { computed } from 'vue';
    import { CategoryColor } from '@/enums/CategoryColor';
    import { CategoryIcon } from '@/enums/CategoryIcon';
    import { getCategoryColorHex, getCategoryIconClass } from '@/features/categories/CategoryService';

    interface Props {
        categories: { name: string; color: CategoryColor; icon: CategoryIcon }[];
    };

    const { categories } = defineProps<Props>();

    const icon = computed(() => getCategoryIconClass(categories[0]?.icon ?? CategoryIcon.None));
    const color = computed(() => getCategoryColorHex(categories[0]?.color ?? CategoryColor.None));

</script>

<template>
    <div class="category-tag">
        <template v-if="categories.length > 0">
            <div
                v-for="(category, index) in categories"
                :key="index"
                v-color="color"
                class="category-tag-item"
            >
                <font-awesome-icon :icon="`fa-solid fa-${icon}`" fixed-width class="category-tag-icon" />
                <span>{{ category.name }}</span>
            </div>
        </template>
        <span v-else class="category-tag-empty">No category</span>
    </div>
</template>
