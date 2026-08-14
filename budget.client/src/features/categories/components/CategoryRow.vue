<script setup lang="ts">

    import './CategoryRow.scss';

    import { computed } from 'vue';
    import { type CategoryColor } from '@/enums/CategoryColor';
    import { type CategoryIcon } from '@/enums/CategoryIcon';
    import CategoryBadge from '@/features/categories/components/CategoryBadge.vue';
    import ClickableRow from '@/components/clickable-row/ClickableRow.vue';
    import { routes } from '@/router.ts';

    interface CategoryRowProps {
        id: number;
        name: string;
        color: CategoryColor;
        icon: CategoryIcon;
        transactionCount: number;
    }

    interface Props {
        category: CategoryRowProps;
    };

    const { category } = defineProps<Props>();
    const transactionCount = computed(() => `${category.transactionCount} transaction${category.transactionCount > 1 ? 's' : ''}`);

</script>

<template>
    <ClickableRow :target="routes.category.update(category.id)">
        <CategoryBadge :categories="[category]" />
        <div class="transaction-row-info">
            <div class="transaction-row-text">
                <p class="transaction-row-title">{{ category.name }}</p>
                <p class="transaction-row-count">{{ transactionCount }}</p>
            </div>
        </div>
    </ClickableRow>
</template>
