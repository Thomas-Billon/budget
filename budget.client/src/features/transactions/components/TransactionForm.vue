<script setup lang="ts">

    import './TransactionForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import CategoryPicker from '@/features/categories/components/CategoryPicker.vue';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts';
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import { type IButtonSwitchOption } from '@/components/button-switch/ButtonSwitch';
    import { apiCall } from '@/utils/ApiCall';
    import { type ICategoryOptionsItemResponse, type ICategoryOptionsResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import FormBase from '@/components/form-base/FormBase.vue';
    import { type FormProps, type FormEmits } from '@/components/form-base/FormBase';
    import WidgetCard from '@/components/widget-card/WidgetCard.vue';
    import PageHeaderActions from '@/components/page-header/PageHeaderActions.vue';

    const { isNew, isAutoSave, isLoading, saveAllResult, savePartialResult, deleteResult } = defineProps<FormProps>();
    const model = defineModel<ITransactionRequest>({ required: true });
    const emit = defineEmits<FormEmits<ITransactionRequest>>();

    const typeOptions: IButtonSwitchOption[] = [
        { value: TransactionType.Income, label: 'Income' },
        { value: TransactionType.Expense, label: 'Expense' }
    ];

    const amountPlaceholder: string = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref<string>('');

    const categoryOptions = ref<ICategoryOptionsItemResponse[]>([]);

    // Recurring toggle — UI only, not part of ITransactionRequest, not sent to the API yet.
    const isRecurring = ref<boolean>(false);
    const recurringFrequency = ref<'day' | 'week' | 'month' | 'year'>('month');

    // #region Init

    onMounted(() => {
        updateAmountDisplayValue();
        getCategoryOptions();
    });

    // #endregion Init

    // #region Category options

    const getCategoryOptions = (): void => {
        apiCall<undefined, ICategoryOptionsResponse>('category/options', { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    categoryOptions.value = response.data.items;
                }
                // TODO: Handle error in else case
            });
    };

    // #endregion Category options

    // #region Amount

    // On model change
    watch(() => model.value, (_) => {
        updateAmountDisplayValue();
    });

    // On amount field input event (any text modification)
    const onAmountInput = (): void => {
        updateAmountRealValue(amountDisplayValue.value);
    };

    // On amount field change event (basically after blur)
    const onAmountChange = (): void => {
        updateAmountDisplayValue();
    };

    const updateAmountDisplayValue = (): void => {
        amountDisplayValue.value = formatAmount(model.value.amount);
    };

    const updateAmountRealValue = (value: string): void => {
        model.value.amount = parseAmount(value);
    };

    // #endregion Amount

    // #region Events

    const formEvents = {
        onSaveAll: (data: ITransactionRequest) => emit('saveAll', data),
        onSavePartial: (id: number, data: Partial<ITransactionRequest>) => emit('savePartial', id, data),
        onDelete: (id: number) => emit('delete', id)
    };

    // #endregion Events

    // #region Form validation

    const isFormValid = (): boolean => {
        return model.value.type !== TransactionType.None
            && model.value.amount > 0
            && model.value.reason.trim().length > 0
            && model.value.date.trim().length > 0;
    };

    // #endregion Form validation

</script>

<template>
    <FormBase
        v-slot="{ onChange }"
        v-model="model"
        :is-new="isNew"
        :is-auto-save="isAutoSave"
        :is-loading="isLoading"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-bind="formEvents"
    >

        <PageHeaderActions>
            <input v-model="model.type" name="Type" type="hidden" required />
            <ButtonSwitch v-model="model.type" :options="typeOptions" @change="onChange('type', model.type)" />
        </PageHeaderActions>

        <div class="row">
            <div :class="['col-12', { 'mt-0': model.type === TransactionType.None }]">
                <div :class="['collapsible', { 'hidden': model.type === TransactionType.None }]">
                    <div>
                        <div class="form-body card">

                            <div class="row">
                                <div class="col-12">
                                    <div class="transaction-form-amount">
                                        <input
                                            id="amount-input"
                                            v-model="amountDisplayValue"
                                            name="Amount"
                                            type="text"
                                            class="transaction-form-amount-input"
                                            :placeholder="amountPlaceholder"
                                            autocomplete="off"
                                            required
                                            @input="onAmountInput(); onChange('amount', model.amount);"
                                            @change="onAmountChange()"
                                        />
                                        <span class="transaction-form-amount-currency">€</span>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <label class="form-label" for="reason">Reason</label>
                                    <input id="reason" v-model="model.reason" name="Reason" type="text" class="form-control" placeholder="What was this for?" required @input="onChange('reason', model.reason);" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12 col-md-with-navbar-6">
                                    <label class="form-label" for="date">Date</label>
                                    <input id="date" v-model="model.date" name="Date" type="date" class="form-control" required @input="onChange('date', model.date);" />
                                </div>
                                <div class="col-12 col-md-with-navbar-6">
                                    <label class="form-label" for="payment-method">Payment Method</label>
                                    <select id="payment-method" v-model="model.paymentMethod" name="PaymentMethod" class="form-select" @change="onChange('paymentMethod', model.paymentMethod);">
                                        <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                                        <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                                        <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                                        <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                                        <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                                        <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                                        <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
                                    </select>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <label class="form-label" for="comment">Comment</label>
                                    <textarea id="comment" v-model="model.comment" name="Comment" class="form-control" placeholder="Comment" @input="onChange('comment', model.comment);"></textarea>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <label class="form-label">Category</label>
                                    <CategoryPicker v-model="model.categoryIds" :options="categoryOptions" @change="onChange('categoryIds', model.categoryIds)" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <div class="transaction-form-recurring">
                                        <font-awesome-icon icon="fa-solid fa-repeat" class="transaction-form-recurring-icon" fixed-width />
                                        <div class="transaction-form-recurring-text">
                                            <p class="transaction-form-recurring-title">Set as recurring</p>
                                            <div class="transaction-form-recurring-frequency">
                                                <span>Repeat every</span>
                                                <select v-model="recurringFrequency" class="transaction-form-recurring-select">
                                                    <option value="day">Day</option>
                                                    <option value="week">Week</option>
                                                    <option value="month">Month</option>
                                                    <option value="year">Year</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="form-check form-switch text-xl">
                                            <input v-model="isRecurring" class="form-check-input" type="checkbox" role="switch" aria-label="Set as recurring" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12 col-md-with-navbar-4">
                <WidgetCard icon="chart-line" title="Budget Status" class="info">
                    <p class="widget-desc">You have <strong>1 240,00 €</strong> left for 'Dining Out' this month.</p>
                </WidgetCard>
            </div>
            <div class="col-12 col-md-with-navbar-4">
                <WidgetCard icon="clock-rotate-left" title="Recent Similar" class="info">
                    <p class="widget-desc">Last 'Dining Out' was <strong>28,50 €</strong> at Le Bistrot yesterday.</p>
                </WidgetCard>
            </div>
            <div class="col-12 col-md-with-navbar-4">
                <WidgetCard icon="calendar-day" title="Prediction" class="info">
                    <p class="widget-desc">This matches your recurring weekly spending pattern.</p>
                </WidgetCard>
            </div>
        </div>

    </FormBase>
</template>
