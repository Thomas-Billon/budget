<script setup lang="ts">

    import './HomeView.scss';

    import { onMounted, ref } from 'vue';
    import WidgetCard from '@/components/widget-card/WidgetCard.vue';
    import TransactionRow from '@/features/transactions/components/TransactionRow.vue';
    import { CategoryColor } from '@/enums/CategoryColor';
    import { CategoryIcon } from '@/enums/CategoryIcon';
    import { TransactionType } from '@/enums/TransactionType.ts';

    interface SpendingCategory {
        label: string;
        amount: string;
        heightPercent: number;
        color: string;
    }

    interface ActivityCategory {
        id: number;
        name: string;
        color: CategoryColor;
        icon: CategoryIcon;
    }

    interface ActivityItem {
        id: number;
        type: TransactionType;
        amount: number;
        reason: string;
        date: string;
        categories: ActivityCategory[];
    }

    const isLoading = ref<boolean>(true);

    const totalBalance = '12 456,08 €';
    const balanceChangePercent = 4.2;
    const balanceChangeLabel = `${balanceChangePercent > 0 ? '+' : ''}${balanceChangePercent}% vs last month`;
    const balanceSparklinePath = 'M0,80 Q20,62 40,72 T78,42 T100,18';

    const monthlySpending = '3 240,15 €';
    const monthlySpendingPercent = 65;
    const monthlySpendingLimitLabel = '65% of your 5 000 € limit';

    const spendingCategories: SpendingCategory[] = [
        { label: 'Dining', amount: '840 €', heightPercent: 80, color: '#3A7CA5' },
        { label: 'Grocery', amount: '630 €', heightPercent: 60, color: '#E67E22' },
        { label: 'Travel', amount: '472 €', heightPercent: 45, color: '#8E44AD' },
        { label: 'Rent', amount: '320 €', heightPercent: 30, color: '#27AE60' }
    ];

    const recentActivity: ActivityItem[] = [
        { id: 1, type: TransactionType.Expense, amount: 84.20, reason: 'Organic Market', date: 'Today, 2:45 PM', categories: [{ id: 1, name: 'Groceries', color: CategoryColor.Yellow, icon: CategoryIcon.Cart }] },
        { id: 2, type: TransactionType.Income, amount: 6400, reason: 'Salary Deposit', date: 'Yesterday', categories: [{ id: 2, name: 'Income', color: CategoryColor.Green, icon: CategoryIcon.Wallet }] },
        { id: 3, type: TransactionType.Expense, amount: 142.30, reason: 'Costco', date: 'Aug 20, 2024', categories: [{ id: 3, name: 'Housing', color: CategoryColor.Orange, icon: CategoryIcon.House }, { id: 4, name: 'Transport', color: CategoryColor.Blue, icon: CategoryIcon.Car }] },
        { id: 4, type: TransactionType.Expense, amount: 32.00, reason: 'Cineplex Ent.', date: 'Aug 18, 2024', categories: [] }
    ];

    // Init
    onMounted(() => {
        isLoading.value = false;
    });

</script>

<template>
    <div class="home">

        <div v-if="!isLoading">
            <div class="row">
                <div class="col-12 col-md-with-navbar-8">
                    <WidgetCard icon="wallet" title="Total Balance">
                        <div class="widget-balance">
                            <span class="widget-balance-amount">{{ totalBalance }}</span>
                            <div class="widget-balance-update" :class="balanceChangePercent > 0 ? 'income' : 'expense'">
                                <font-awesome-icon :icon="`fa-solid fa-arrow-trend-${ balanceChangePercent > 0 ? 'up' : 'down' }`" />
                                <span>{{ balanceChangeLabel }}</span>
                            </div>
                            <!-- Chart -->
                            <div class="widget-balance-chart">
                                <svg viewBox="0 0 100 100" preserveAspectRatio="none">
                                    <defs>
                                        <linearGradient id="widget-balance-gradient" x1="0%" y1="0%" x2="0%" y2="100%">
                                            <stop offset="0%" style="stop-color:rgba(58,124,165,0.25);" />
                                            <stop offset="100%" style="stop-color:rgba(58,124,165,0);" />
                                        </linearGradient>
                                    </defs>
                                    <path :d="`${balanceSparklinePath} L100,100 L0,100 Z`" fill="url(#widget-balance-gradient)" />
                                    <path :d="balanceSparklinePath" fill="none" stroke="#3A7CA5" stroke-width="2" vector-effect="non-scaling-stroke" />
                                </svg>
                            </div>
                        </div>
                    </WidgetCard>
                </div>

                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="wallet" title="This Month's Spending" class="dark">
                        <div class="widget-spending">
                            <span class="widget-spending-amount">{{ monthlySpending }}</span>
                            <div class="widget-spending-progress">
                                <!-- Progress bar -->
                                <div class="widget-spending-progress-bar">
                                    <div class="widget-spending-progress-filler" :style="{ '--progressValue': `${monthlySpendingPercent}%` }"></div>
                                </div>
                                <p class="widget-spending-progress-limit">{{ monthlySpendingLimitLabel }}</p>
                            </div>
                            <!-- Chart -->
                            <div class="widget-spending-chart">
                                <div v-for="(category, index) in spendingCategories" :key="index" class="widget-spending-chart-item" :style="{ '--categoryColor': category.color, '--categoryValue': `${category.heightPercent}%` }">
                                    <span class="widget-spending-chart-amount">{{ category.amount }}</span>
                                    <div class="widget-spending-chart-bar"></div>
                                    <span class="widget-spending-chart-label">{{ category.label }}</span>
                                </div>
                            </div>
                        </div>
                    </WidgetCard>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <WidgetCard icon="receipt" title="Recent Activity">
                        <TransactionRow
                            v-for="(item, index) in recentActivity"
                            :key="index"
                            :transaction="item"
                            class="in-card"
                        />
                    </WidgetCard>
                </div>
            </div>
        </div>

        <div v-else>Loading…</div>

    </div>
</template>
