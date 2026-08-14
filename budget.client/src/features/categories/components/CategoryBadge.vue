<script setup lang="ts">

    import './CategoryBadge.scss';

    import { computed } from 'vue';
    import { CategoryColor } from '@/enums/CategoryColor';
    import { CategoryIcon } from '@/enums/CategoryIcon';
    import { getCategoryColorHex, getCategoryIconClass } from '@/features/categories/CategoryService';

    interface Props {
        categories: { color: CategoryColor; icon: CategoryIcon }[];
    };

    const { categories } = defineProps<Props>();

    const icon = computed(() => getCategoryIconClass(categories[0]?.icon ?? CategoryIcon.None));
    const color = computed(() => getCategoryColorHex(categories[0]?.color ?? CategoryColor.None));

    const extraCount = computed(() => Math.max(categories.length - 1, 0));

</script>

<template>
    <div v-color="color" class="category-badge">
        <font-awesome-icon :icon="`fa-solid fa-${icon}`" fixed-width />
        <span v-if="extraCount" class="category-badge-count">+{{ extraCount }}</span>
    </div>
</template>
