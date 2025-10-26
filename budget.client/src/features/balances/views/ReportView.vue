<script setup lang="ts">

    import './ReportView.scss';

    import { onMounted, ref, watch } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type IBalanceReportResponse } from '@/features/balances/models/IBalanceReportResponse.ts';
    import { getUtcDatesFromDateRange } from '@/utils/DateRange.ts';
    import { DateRange } from '@/enums/DateRange.ts';

    const dateRange = ref<DateRange>(DateRange.CurrentMonth);
    const balanceReport = ref<IBalanceReportResponse | undefined>();

    // Init
    onMounted(() => {
        const { startDate, endDate } = getUtcDatesFromDateRange(DateRange.CurrentMonth);
        getBalanceReport(startDate, endDate);
    });

    watch(dateRange, (newDateRange) => {
        const { startDate, endDate } = getUtcDatesFromDateRange(newDateRange);
        getBalanceReport(startDate, endDate);
    });

    const getBalanceReport = async (startDate: Date, endDate: Date) => {
        const startDateStr = startDate.toISOString().split("T")[0];
        const endDateStr = endDate.toISOString().split("T")[0];

        apiCall<undefined, IBalanceReportResponse>(`balance?startDate=${startDateStr}&endDate=${endDateStr}`, { method: 'GET' })
            .then(response => {
                balanceReport.value = response;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

</script>

<template>
    <div class="balance-report section-container container">
        <select class="form-select form-select-lg" id="balance-date-range" name="DateRange" v-model="dateRange">
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

        <div class="balance-summary">
            <p>Total Income: {{ balanceReport?.totalIncome }}</p>
            <p>Total Expense: {{ balanceReport?.totalExpense }}</p>
            <p>Net Balance: {{ balanceReport?.netBalance }}</p>
        </div>
    </div>
</template>
