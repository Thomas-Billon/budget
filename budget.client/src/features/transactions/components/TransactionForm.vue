<script setup lang="ts">

    import './TransactionForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts';
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import { type IButtonSwitchOption } from '@/components/button-switch/ButtonSwitch';
    import { apiCall } from '@/utils/ApiCall';
    import { type ICategoryOptionsItemResponse, type ICategoryOptionsResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import FormBase from '@/components/form-base/FormBase.vue';
    import { type FormProps, type FormEmits } from '@/components/form-base/FormBase';

    const { isNew, saveAllResult, savePartialResult, deleteResult } = defineProps<FormProps>();
    const model = defineModel<ITransactionRequest>({ required: true });
    const emit = defineEmits<FormEmits<ITransactionRequest>>();

    const typeInput = ref<HTMLInputElement | undefined>();
    const typeOptions: IButtonSwitchOption[] = [{ value: TransactionType.Income, label: 'Income', icon: 'plus' }, { value: TransactionType.Expense, label: 'Expense', icon: 'minus' }];

    const amountPlaceholder: string = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref<string>('');

    const categoryOptions = ref<ICategoryOptionsItemResponse[]>([]);

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
        v-model="model"
        :is-new="isNew"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-bind="formEvents"
    >

        <template #head="{ onChange }">
            <div class="form-head">
                <input ref="typeInput" v-model="model.type" name="Type" type="hidden" required />
                <ButtonSwitch v-model="model.type" :options="typeOptions" class-name="bg-secondary bg-opacity-50" @change="onChange('type', model.type)" />
            </div>
        </template>

        <template #body="{ onChange }">
            <div class="form-body transition-opacity" :class="[ !model.type && 'hidden' ]">

                <div class="input-group">
                    <input
                        v-model="amountDisplayValue"
                        name="Amount"
                        type="text"
                        class="transaction-form-input-amount form-control form-control-lg"
                        :placeholder="amountPlaceholder"
                        autocomplete="off"
                        required
                        @input="onAmountInput(); onChange('amount', model.amount);"
                        @change="onAmountChange()"
                    />
                    <span class="input-group-text">
                        €
                    </span>
                </div>

                <input v-model="model.reason" name="Reason" type="text" class="form-control form-control-lg" placeholder="Reason" required @input="onChange('reason', model.reason);" />

                <input v-model="model.date" name="Date" type="date" class="form-control form-control-lg" required @input="onChange('date', model.date);" />

                <select v-model="model.paymentMethod" name="PaymentMethod" class="form-select form-select-lg" @change="onChange('paymentMethod', model.paymentMethod);">
                    <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                    <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                    <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                    <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                    <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                    <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                    <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
                </select>

                <textarea v-model="model.comment" name="Comment" class="form-control form-control-lg" placeholder="Comment" @input="onChange('comment', model.comment);"></textarea>

                <select v-model="model.categoryIds" name="CategoryIds" class="form-select form-select-lg" multiple @change="onChange('categoryIds', model.categoryIds);">
                    <option v-for="option in categoryOptions" :key="option.id" :value="option.id">{{ option.name }}</option>
                </select>

            </div>
        </template>

    </FormBase>
</template>
