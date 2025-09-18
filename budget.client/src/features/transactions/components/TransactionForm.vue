<script setup lang="ts">

    import './TransactionForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts'
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';

    const { isNew, saveAllResult, savePartialResult } = defineProps(['isNew', 'saveAllResult', 'savePartialResult']);
    const model = defineModel<ITransactionRequest>({ required: true });
    const emit = defineEmits(['saveAll', 'savePartial']);

    const typeOptions = [{ value: TransactionType.Income, label: 'Income', icon: 'plus' }, { value: TransactionType.Expense, label: 'Expense', icon: 'minus' }];
    const amountPlaceholder = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref('');
    const submitLabel = ref('');
    const isSubmitDisabled = ref(false);

    let partialModel: Partial<ITransactionRequest> = {};

    // Init
    onMounted(() => {
        updateAmountDisplayValue();
        setSubmitButtonToDefaultState();
    });

    // On props change
    watch(() => [saveAllResult, savePartialResult], ([all, partial]) => {
        if (all.isSuccess == false) {
            setSubmitButtonToErrorState();
            resetSubmitButtonToDefaultState();
        }
        else {
            if (partial.isSuccess == false || isNew) {
                setSubmitButtonToDefaultState();
            }
            else {
                setSubmitButtonToSavedState();
                resetSubmitButtonToDefaultState();
            }
        }
    });

    // On model change
    watch(() => model.value, (_) => {
        updateAmountDisplayValue();
    });

    // On form submit
    const onSubmit = (): void => {
        setSubmitButtonToSavedState();
        saveAll();
    }

    // On all form fields input event (any text modification)
    const onFormFieldInput = <T extends keyof ITransactionRequest>(event: Event, fieldName: T): void => {
        setSubmitButtonToDefaultState();

        // Edit only for partial update
        if (!isNew) {
            partialModel[fieldName] = model.value[fieldName];
            debounceSavePartial();
        }
    }

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

    const setSubmitButtonToDefaultState = (): void => {
        submitLabel.value = isNew ? 'Add transaction' : 'Edit transaction';
        isSubmitDisabled.value = false;
    }

    const setSubmitButtonToSavedState = (): void => {
        submitLabel.value = 'Saved';
        isSubmitDisabled.value = true;
    }

    const setSubmitButtonToErrorState = (): void => {
        submitLabel.value = 'Error';
        isSubmitDisabled.value = false;
    }

    const resetSubmitButtonToDefaultState = debounce(setSubmitButtonToDefaultState, 5000);

    const saveAll = () => emit('saveAll', model.value);
    const savePartial = () => emit('savePartial', model.value.id, partialModel);

    const debounceSavePartial = debounce(() => { savePartial(); partialModel = {}; }, 1000);

</script>

<template>
    <form class="transaction-form section-container-grow container" @submit.prevent="onSubmit">

        <input type="hidden" id="transaction-id" name="Id" v-model="model.id" />

        <div class="transaction-form-head">
            <input type="hidden" id="transaction-type" name="Type" v-model="model.type" />
            <ButtonSwitch v-model="model.type" :options="typeOptions" class-name="bg-secondary bg-opacity-50" />
        </div>

        <div class="transaction-form-body transition-opacity" :class="[ model.type === TransactionType.None && 'hidden' ]">

            <div class="input-group">
                <input class="transaction-form-input-amount form-control form-control-lg"
                       type="text"
                       id="transaction-amount"
                       name="Amount"
                       v-model="amountDisplayValue"
                       :placeholder="amountPlaceholder"
                       autocomplete="off"
                       required
                       @input="onAmountInput($event); onFormFieldInput($event, 'amount');"
                       @change="onAmountChange($event)" />
                <span class="input-group-text">
                    €
                </span>
            </div>

            <input class="form-control form-control-lg" type="text" id="transaction-reason" name="Reason" v-model="model.reason" placeholder="Reason" required @input="onFormFieldInput($event, 'reason');" />

            <input class="form-control form-control-lg" type="date" id="transaction-date" name="Date" v-model="model.date" @input="onFormFieldInput($event, 'date');" />

            <select class="form-select form-select-lg" id="transaction-payment-method" name="PaymentMethod" v-model="model.paymentMethod" @input="onFormFieldInput($event, 'paymentMethod');">
                <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
            </select>

            <textarea class="form-control form-control-lg" id="transaction-comment" name="Comment" v-model="model.comment" placeholder="Comment" @input="onFormFieldInput($event, 'comment');"></textarea>

        </div>

        <div class="transaction-form-foot transition-opacity" :class="[ model.type === TransactionType.None && 'hidden' ]">
            <button type="submit" class="transaction-form-submit btn btn-primary btn-lg" :disabled="isSubmitDisabled || model.type === TransactionType.None || !model.amount || !model.reason">
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
