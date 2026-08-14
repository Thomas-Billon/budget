<script setup lang="ts">

    import './TransactionRow.scss';

    import { computed } from 'vue';
    import { type CategoryColor } from '@/enums/CategoryColor';
    import { type CategoryIcon } from '@/enums/CategoryIcon';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import CategoryBadge from '@/features/categories/components/CategoryBadge.vue';
    import CategoryTag from '@/features/categories/components/CategoryTag.vue';
    import { formatAmount } from '@/features/transactions/TransactionService.ts';
    import ClickableRow from '@/components/clickable-row/ClickableRow.vue';
    import { routes } from '@/router.ts';

    interface TransactionRowCategoryProps {
        id: number;
        name: string;
        color: CategoryColor;
        icon: CategoryIcon;
    }

    interface TransactionRowProps {
        id: number;
        type: TransactionType;
        amount: number;
        reason: string;
        date: string;
        categories: TransactionRowCategoryProps[];
    }

    interface Props {
        transaction: TransactionRowProps;
    };

    const { transaction } = defineProps<Props>();

    const amount = computed(() => `${transaction.type === TransactionType.Income ? '+' : ''}${formatAmount(transaction.amount)} €`);

</script>

<template>
    <ClickableRow :target="routes.transaction.update(transaction.id)">
        <CategoryBadge :categories="transaction.categories" />
        <div class="transaction-row-info">
            <div class="transaction-row-text">
                <p class="transaction-row-title">{{ transaction.reason }}</p>
                <p class="transaction-row-date">{{ transaction.date }}</p>
            </div>
            <CategoryTag :categories="transaction.categories" class="sm no-icon" />
        </div>
        <span :class="['transaction-row-amount', TransactionType[transaction.type].toLowerCase()]">{{ amount }}</span>
    </ClickableRow>
</template>
