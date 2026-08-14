<script setup lang="ts">

    import './ReportView.scss';

    import { computed, onMounted, ref, watch } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type IBalanceReportResponse } from '@/features/balances/models/IBalanceReportResponse.ts';
    import { getUtcDatesFromDateRange } from '@/utils/DateRange.ts';
    import { DateRange } from '@/enums/DateRange.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import { type IButtonSwitchOption } from '@/components/button-switch/ButtonSwitch';
    import WidgetCard from '@/components/widget-card/WidgetCard.vue';
    import TransactionRow from '@/features/transactions/components/TransactionRow.vue';
    import PageHeaderActions from '@/components/page-header/PageHeaderActions.vue';

    const isLoading = ref<boolean>(true);
    const dateRange = ref<DateRange>(DateRange.CurrentMonth);
    const customStartDate = ref<Date | undefined>();
    const customEndDate = ref<Date | undefined>();
    const balanceReport = ref<IBalanceReportResponse | undefined>();

    const dateRangeOptions: IButtonSwitchOption[] = [
        { value: DateRange.Today, label: 'Today' },
        { value: DateRange.Yesterday, label: 'Yesterday' },
        { value: DateRange.CurrentWeek, label: 'This week' },
        { value: DateRange.LastWeek, label: 'Last week' },
        { value: DateRange.CurrentMonth, label: 'This month' },
        { value: DateRange.LastMonth, label: 'Last month' },
        { value: DateRange.CurrentYear, label: 'This year' },
        { value: DateRange.LastYear, label: 'Last year' },
        { value: DateRange.AllTime, label: 'All time' },
        { value: DateRange.Custom, label: 'Custom' }
    ];

    const formatDateForInput = (date?: Date): string => {
        if (!date) {
            return '';
        }

        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');

        return `${year}-${month}-${day}`;
    };

    const parseDateFromInput = (value: string): Date | undefined => {
        if (!value) {
            return undefined;
        }

        const [year, month, day] = value.split('-').map(Number);

        return new Date(year, month - 1, day);
    };

    const customStartDateInput = computed<string>({
        get: () => formatDateForInput(customStartDate.value),
        set: (value: string) => { customStartDate.value = parseDateFromInput(value); }
    });

    const customEndDateInput = computed<string>({
        get: () => formatDateForInput(customEndDate.value),
        set: (value: string) => { customEndDate.value = parseDateFromInput(value); }
    });

    const getBalanceReport = (startDate: Date, endDate: Date) => {
        const startDateStr = startDate.toISOString().split('T')[0];
        const endDateStr = endDate.toISOString().split('T')[0];

        apiCall<undefined, IBalanceReportResponse>(`balance?startDate=${startDateStr}&endDate=${endDateStr}`, { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    balanceReport.value = response.data;
                    isLoading.value = false;
                }
                // TODO: Handle error in else case
            });
    };

    // Init
    onMounted(() => {
        const { startDate, endDate } = getUtcDatesFromDateRange(DateRange.CurrentMonth);
        getBalanceReport(startDate, endDate);
    });

    watch([dateRange, customStartDate, customEndDate], ([newDateRange, newCustomStartDate, newCustomEndDate]) => {
        let startDate: Date | undefined;
        let endDate: Date | undefined;

        if (newDateRange === DateRange.Custom) {
            startDate = newCustomStartDate;
            endDate = newCustomEndDate;
        }
        else {
            const dates = getUtcDatesFromDateRange(newDateRange);
            startDate = dates.startDate;
            endDate = dates.endDate;
        }

        if (startDate && endDate) {
            getBalanceReport(startDate, endDate);
        }
    });

    // #region Category breakdown

    interface CategoryBreakdownItem {
        categoryId: number;
        name: string;
        colorHex: string;
        amount: number;
        share: number;
    }

    const categoryBreakdown = computed<CategoryBreakdownItem[]>(() => {
        if (!balanceReport.value) {
            return [];
        }

        return [];
        /*
        TODO: Fix this
        return balanceReport.value.expenseTransactionsByCategory
            .map(item => {
                const category = balanceReport.value?.categories.find(c => c.id === item.categoryId);
                const amount = item.transactions.reduce((sum, transaction) => sum + transaction.amount, 0);

                return {
                    categoryId: item.categoryId,
                    name: category?.name ?? 'No category',
                    colorHex: category ? getCategoryColorHex(category.color) : '#747878',
                    amount,
                    share: item.categoryShare
                };
            })
            .sort((a, b) => b.amount - a.amount);
        */
    });

    // #endregion Category breakdown

</script>

<template>
    <div class="balance-report">

        <PageHeaderActions>
            <div class="balance-report-date-range">
                <ButtonSwitch v-model="dateRange" :options="dateRangeOptions" class="sm" />
                <div v-if="dateRange === DateRange.Custom" class="balance-report-date-range-custom">
                    <div class="form-group">
                        <label class="field-label" for="custom-start-date">Start date</label>
                        <input id="custom-start-date" v-model="customStartDateInput" name="CustomStartDate" type="date" class="field-input sm" />
                    </div>
                    <div class="form-group">
                        <label class="field-label" for="custom-end-date">End date</label>
                        <input id="custom-end-date" v-model="customEndDateInput" name="CustomEndDate" type="date" class="field-input sm" />
                    </div>
                </div>
            </div>
        </PageHeaderActions>

        <div v-if="!isLoading">
            <div class="row">
                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="arrow-trend-up" title="Total Income">
                        <span class="balance-report-amount income">{{ balanceReport.totalIncome.toFixed(2) }} €</span>
                    </WidgetCard>
                </div>
                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="arrow-trend-down" title="Total Expense">
                        <span class="balance-report-amount expense">{{ balanceReport.totalExpense.toFixed(2) }} €</span>
                    </WidgetCard>
                </div>
                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="wallet" title="Net Balance" class="dark">
                        <span class="balance-report-amount">{{ balanceReport.netBalance.toFixed(2) }} €</span>
                    </WidgetCard>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-with-navbar-8">
                    <WidgetCard title="Spending by Category">
                        <!-- Treemap -->
                        <div v-if="categoryBreakdown.length > 0" class="treemap-grid">
                            <div v-for="item in categoryBreakdown" :key="item.categoryId" v-color="item.colorHex" class="treemap-item">
                                <div class="treemap-accent" :style="{ backgroundColor: item.colorHex }"></div>
                                <span class="widget-card-title">{{ item.name }}</span>
                                <span class="treemap-amount">{{ item.amount.toFixed(2) }} € · {{ parseFloat(item.share.toFixed(1)) }}%</span>
                            </div>
                        </div>
                        <p v-else class="balance-report-empty">No expenses for this period.</p>
                    </WidgetCard>
                </div>
                <div class="col-12 col-md-with-navbar-4">
                    <div class="row">
                        <div class="col-12">
                            <WidgetCard icon="arrow-trend-down" title="Top Expenses" class="balance-report-list">
                                <div v-if="balanceReport.mostExpensiveTransactions.length > 0">
                                    <TransactionRow
                                        v-for="transaction in balanceReport.mostExpensiveTransactions"
                                        :key="transaction.id"
                                        :transaction="transaction"
                                        class="in-card"
                                    />
                                </div>
                                <p v-else class="balance-report-empty">No expenses for this period.</p>
                            </WidgetCard>
                        </div>
                        <div class="col-12">
                            <WidgetCard icon="arrow-trend-up" title="Top Income" class="balance-report-list">
                                <div v-if="balanceReport.mostLucrativeTransactions.length > 0">
                                    <TransactionRow
                                        v-for="transaction in balanceReport.mostLucrativeTransactions"
                                        :key="transaction.id"
                                        :transaction="transaction"
                                        class="in-card"
                                    />
                                </div>
                                <p v-else class="balance-report-empty">No income for this period.</p>
                            </WidgetCard>
                        </div>
                    </div>
                </div>
            </div>

            <section class="row">
                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="triangle-exclamation" title="Overspend Alert" class="expense">
                        <p>You overspent <strong>20,00 €</strong> for 'Coffee/Breakfast' this month.</p>
                    </WidgetCard>
                </div>
                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="piggy-bank" title="Saved" class="income">
                        <p>You saved <strong>8%</strong> more than last month on 'Transport'.</p>
                    </WidgetCard>
                </div>
                <div class="col-12 col-md-with-navbar-4">
                    <WidgetCard icon="arrow-trend-up" title="Trend" class="info">
                        <p>'Housing' is your fastest-growing category this quarter.</p>
                    </WidgetCard>
                </div>
            </section>
        </div>

        <div v-else>Loading…</div>

    </div>
</template>
