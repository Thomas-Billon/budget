<script setup lang="ts">

    import './ReportView.scss';

    import { onMounted, ref, watch } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type IBalanceReportResponse } from '@/features/balances/models/IBalanceReportResponse.ts';
    import { getUtcDatesFromDateRange } from '@/utils/DateRange.ts';
    import { DateRange } from '@/enums/DateRange.ts';

    const dateRange = ref<DateRange>(DateRange.CurrentMonth);
    const balanceReport = ref<IBalanceReportResponse | undefined>();

    const getBalanceReport = (startDate: Date, endDate: Date) => {
        const startDateStr = startDate.toISOString().split('T')[0];
        const endDateStr = endDate.toISOString().split('T')[0];

        apiCall<undefined, IBalanceReportResponse>(`balance?startDate=${startDateStr}&endDate=${endDateStr}`, { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    balanceReport.value = response.data;
                }
                // TODO: Handle error in else case
            });
    };

    // Init
    onMounted(() => {
        const { startDate, endDate } = getUtcDatesFromDateRange(DateRange.CurrentMonth);
        getBalanceReport(startDate, endDate);
    });

    watch(dateRange, (newDateRange) => {
        const { startDate, endDate } = getUtcDatesFromDateRange(newDateRange);
        getBalanceReport(startDate, endDate);
    });

</script>

<template>
    <div class="balance-report section-container container">
        <select v-model="dateRange" name="DateRange" class="form-select form-select-lg">
            <option :value="DateRange.Today">Today</option>
            <option :value="DateRange.Yesterday">Yesterday</option>
            <option :value="DateRange.CurrentWeek">Current week</option>
            <option :value="DateRange.LastWeek">Last week</option>
            <option :value="DateRange.CurrentMonth">Current month</option>
            <option :value="DateRange.LastMonth">Last month</option>
            <option :value="DateRange.CurrentYear">Current year</option>
            <option :value="DateRange.LastYear">Last year</option>
            <option :value="DateRange.AllTime">All time</option>
        </select>

        <div v-if="balanceReport" class="balance-summary">
            <p>Total Income: {{ balanceReport.totalIncome }}</p>
            <p>Total Expense: {{ balanceReport.totalExpense }}</p>
            <p>Net Balance: {{ balanceReport.netBalance }}</p>
            <br />
            <div>
                Top 3 Income:
                <ul>
                    <li v-for="(transaction, index) in balanceReport.mostLucrativeTransactions" :key="index">
                        {{ transaction.reason }} - {{ transaction.amount }}
                    </li>
                </ul>
            </div>
            <div>
                Top 3 Expense:
                <ul>
                    <li v-for="(transaction, index) in balanceReport.mostExpensiveTransactions" :key="index">
                        {{ transaction.reason }} - {{ transaction.amount }}
                    </li>
                </ul>
            </div>
            <div>
                Income by categories:
                <ul>
                    <li v-for="(transactionsByCategory, index) in balanceReport.incomeTransactionsByCategory" :key="index">
                        {{ balanceReport.categories.find((item) => item.id === transactionsByCategory.categoryId)?.name ?? 'No category' }}
                        -
                        {{ parseFloat(transactionsByCategory.categoryShare.toFixed(2)) }}%
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>
