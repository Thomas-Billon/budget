<script setup lang="ts">

    import './TransactionForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts'
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import { type IButtonSwitchOption } from '@/components/button-switch/ButtonSwitchValue';
    import { apiCall } from '@/utils/ApiCall';
    import { type ICategoryOptionsItemResponse, type ICategoryOptionsResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import FormBase from '@/components/form-base/FormBase.vue';
    import { type FormProps, type FormEmits } from '@/utils/Form';

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
        apiCall<undefined, ICategoryOptionsResponse>(`category/options`, { method: 'GET' })
            .then(response => {
                categoryOptions.value = response.items;
            })
            .catch(() => {
                // TODO: Add error
            });
    }

    // #endregion Category options

    // #region Amount

    // On model change
    watch(() => model.value, (_) => {
        updateAmountDisplayValue();
    });

    // On amount field input event (any text modification)
    const onAmountInput = (event: Event): void => {
        updateAmountRealValue(amountDisplayValue.value);
    }

    // On amount field change event (basically after blur)
    const onAmountChange = (event: Event): void => {
        updateAmountDisplayValue();
    }

    const updateAmountDisplayValue = (): void => {
        amountDisplayValue.value = formatAmount(model.value.amount);
    };

    const updateAmountRealValue = (value: string): void => {
        model.value.amount = parseAmount(value);
    }

    // #endregion Amount

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
        :is-new="isNew"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-model="model"
        @save-all="(data) => emit('saveAll', data)"
        @save-partial="(id, data) => emit('savePartial', id, data)"
        @delete="(id) => emit('delete', id)">

        <template #head="{ onChange }">
            <div class="transaction-form-head">
                <input ref="typeInput" type="hidden" name="Type" v-model="model.type" required />
                <ButtonSwitch v-model="model.type" :options="typeOptions" class-name="bg-secondary bg-opacity-50" @change="onChange('type', model.type)" />
            </div>
        </template>

        <template #body="{ onChange }">
            <div class="transaction-form-body transition-opacity" :class="[ !model.type && 'hidden' ]">

                <div class="input-group">
                    <input class="transaction-form-input-amount form-control form-control-lg"
                        type="text"
                        name="Amount"
                        v-model="amountDisplayValue"
                        :placeholder="amountPlaceholder"
                        autocomplete="off"
                        required
                        @input="onAmountInput($event); onChange('amount', model.amount);"
                        @change="onAmountChange($event)" />
                    <span class="input-group-text">
                        €
                    </span>
                </div>

                <input class="form-control form-control-lg" type="text" name="Reason" v-model="model.reason" placeholder="Reason" required @input="onChange('reason', model.reason);" />

                <input class="form-control form-control-lg" type="date" name="Date" v-model="model.date" required @input="onChange('date', model.date);" />

                <select class="form-select form-select-lg" name="PaymentMethod" v-model="model.paymentMethod" @change="onChange('paymentMethod', model.paymentMethod);">
                    <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                    <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                    <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                    <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                    <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                    <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                    <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
                </select>

                <textarea class="form-control form-control-lg" name="Comment" v-model="model.comment" placeholder="Comment" @input="onChange('comment', model.comment);"></textarea>

                <select class="form-select form-select-lg" name="CategoryIds" v-model="model.categoryIds" multiple @change="onChange('categoryIds', model.categoryIds);">
                    <option v-for="option in categoryOptions" :key="option.id" :value="option.id">{{ option.name }}</option>
                </select>

            </div>
        </template>

    </FormBase>
</template>
