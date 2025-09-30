<script setup lang="ts">

    import './TransactionForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts'
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import { type ButtonSwitchValue, type IButtonSwitchOption } from '@/components/button-switch/IButtonSwitchOption';
    import { type ApiCallResult } from '@/utils/ApiCall';

    interface Props {
        isNew: boolean;
        saveAllResult?: ApiCallResult;
        savePartialResult?: ApiCallResult;
    }

    type Emits = {
        saveAll: [data: Partial<ITransactionRequest>];
        savePartial: [id: number, data: Partial<ITransactionRequest>];
    };
    
    const { isNew, saveAllResult, savePartialResult } = defineProps<Props>();
    const model = defineModel<Partial<ITransactionRequest>>({ required: true });
    const emit = defineEmits<Emits>();

    const typeInput = ref<HTMLInputElement | undefined>();
    const typeOptions: IButtonSwitchOption[] = [{ value: TransactionType.Income, label: 'Income', icon: 'plus' }, { value: TransactionType.Expense, label: 'Expense', icon: 'minus' }];
    const amountPlaceholder: string = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref<string>('');
    const submitLabel = ref<string>('');
    const isSubmitDisabled = ref<boolean>(false);

    let partialModel: Partial<ITransactionRequest> = {};

    // Init
    onMounted(() => {
        updateAmountDisplayValue();
        setSubmitButtonToDefaultState();
    });

    // On props change
    watch(() => [saveAllResult, savePartialResult], ([all, partial]) => {
        if (all !== undefined && !all.isSuccess) {
            setSubmitButtonToErrorState();
            waitAndResetSubmitButtonToDefaultState();
        }
        else {
            if (partial !== undefined && !partial.isSuccess || isNew) {
                setSubmitButtonToDefaultState();
            }
            else {
                setSubmitButtonToSavedState();
                waitAndResetSubmitButtonToDefaultState();
            }
        }
    });

    // On model change
    watch(() => model.value, (_) => {
        updateAmountDisplayValue();
    });

    // On form submit
    const onSubmit = (): void => {
        saveAll(model.value);
        setSubmitButtonToSavedState();
    }

    // On type field switch event
    const onTypeFieldChange = (value: ButtonSwitchValue | undefined): void => {
        setSubmitButtonToDefaultState();
        fillPartialModel('type', value as ITransactionRequest['type']);
    }

    // On all form fields input event (any text modification)
    const onFormFieldInput = <T extends keyof ITransactionRequest>(event: Event, fieldName: T): void => {
            const target = event.target as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement | null;
            if (target === null) {
                return;
            }

        setSubmitButtonToDefaultState();
        fillPartialModel(fieldName, target.value as ITransactionRequest[T]);
    }

    // On amount field input event (any text modification)
    const onAmountInput = (event: Event): void => {
        updateAmountRealValue(amountDisplayValue.value);
    }

    // On amount field change event (basically after blur)
    const onAmountChange = (event: Event): void => {
        updateAmountDisplayValue();
    }

    const fillPartialModel = <T extends keyof ITransactionRequest>(fieldName: T, value: ITransactionRequest[T]): void => {
        if (isNew) {
            return;
        }

        partialModel[fieldName] = value;
        debounceSavePartial();
    };

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

    const waitAndResetSubmitButtonToDefaultState = debounce(setSubmitButtonToDefaultState, 5000);

    const saveAll = (data: Partial<ITransactionRequest>) => emit('saveAll', data);
    const savePartial = (id: number, data: Partial<ITransactionRequest>) => emit('savePartial', id, data);

    const debounceSavePartial = debounce(() => {
        if (model.value.id === undefined || Object.keys(partialModel).length === 0) {
            return;
        }

        savePartial(model.value.id, partialModel);
        partialModel = {};
    }, 1000);

</script>

<template>
    <form class="transaction-form section-container-grow container" @submit.prevent="onSubmit">

        <input type="hidden" id="transaction-id" name="Id" v-model="model.id" />

        <div class="transaction-form-head">
            <input ref="typeInput" type="hidden" id="transaction-type" name="Type" v-model="model.type" required />
            <ButtonSwitch v-model="model.type" :options="typeOptions" class-name="bg-secondary bg-opacity-50" v-on:update:modelValue="onTypeFieldChange($event)" />
        </div>

        <div class="transaction-form-body transition-opacity" :class="[ !model.type && 'hidden' ]">

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
                <option v-if="model.paymentMethod === undefined" :value="undefined" disabled selected>Select Payment Method</option>
                <option v-if="model.paymentMethod !== undefined" :value="PaymentMethod.None" disabled>Select Payment Method</option>
                <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
            </select>

            <textarea class="form-control form-control-lg" id="transaction-comment" name="Comment" v-model="model.comment" placeholder="Comment" @input="onFormFieldInput($event, 'comment');"></textarea>

        </div>

        <div class="transaction-form-foot transition-opacity" :class="[ !model.type && 'hidden' ]">
            <button type="submit" class="transaction-form-submit btn btn-primary btn-lg" :disabled="isSubmitDisabled || !model.type || !model.amount || !model.reason">
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
