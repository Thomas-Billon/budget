<script setup lang="ts">

    import './DetailsView.scss';

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type IBalanceDetailsResponse } from '@/features/balances/models/IBalanceDetailsResponse.ts';
    import { getDatesFromDateRange } from '@/utils/DateRange.ts';
    import { DateRange } from '@/enums/DateRange.ts';

    const balanceDetails = ref<IBalanceDetailsResponse | undefined>();

    // Init
    onMounted(() => {
        const { startDate, endDate } = getDatesFromDateRange(DateRange.CurrentMonth);
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
        <div class="balance-summary">
            <p>Total Income: {{ balanceDetails?.totalIncome }}</p>
            <p>Total Expense: {{ balanceDetails?.totalExpense }}</p>
            <p>Net Balance: {{ balanceDetails?.netBalance }}</p>
        </div>
    </div>
</template>
