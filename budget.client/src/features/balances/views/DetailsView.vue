<script setup lang="ts">

    import './DetailsView.scss';

    import { onMounted, ref, watch } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type IBalanceDetailsResponse } from '@/features/balances/models/IBalanceDetailsResponse.ts';
    import { getDatesFromDateRange } from '@/utils/DateRange.ts';
    import { DateRange } from '@/enums/DateRange.ts';

    const dateRange = ref<DateRange>(DateRange.CurrentMonth);
    const balanceDetails = ref<IBalanceDetailsResponse | undefined>();

    // Init
    onMounted(() => {
        const { startDate, endDate } = getDatesFromDateRange(DateRange.CurrentMonth);
        getBalanceDetails(startDate, endDate);
    });

    watch(dateRange, (newDateRange) => {
        const { startDate, endDate } = getDatesFromDateRange(newDateRange);
        getBalanceDetails(startDate, endDate);
    });

    const getBalanceDetails = async (startDate: Date, endDate: Date) => {
        const startDateStr = startDate.toISOString().split("T")[0];
        const endDateStr = endDate.toISOString().split("T")[0];

        apiCall<undefined, IBalanceDetailsResponse>(`balance?startDate=${startDateStr}&endDate=${endDateStr}`, { method: 'GET' })
            .then(response => {
                balanceDetails.value = response;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

</script>

<template>
    <div class="balance-details section-container container">
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
            <p>Total Income: {{ balanceDetails?.totalIncome }}</p>
            <p>Total Expense: {{ balanceDetails?.totalExpense }}</p>
            <p>Net Balance: {{ balanceDetails?.netBalance }}</p>
        </div>
    </div>
</template>
