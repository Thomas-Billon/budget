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
        deleteResult?: ApiCallResult;
    };

    type Emits = {
        saveAll: [data: ITransactionRequest];
        savePartial: [id: number, data: Partial<ITransactionRequest>];
        delete: [id: number];
    };
    
    const { isNew, saveAllResult, savePartialResult, deleteResult } = defineProps<Props>();
    const model = defineModel<ITransactionRequest>({ required: true });
    const emit = defineEmits<Emits>();

    const typeInput = ref<HTMLInputElement | undefined>();
    const typeOptions: IButtonSwitchOption[] = [{ value: TransactionType.Income, label: 'Income', icon: 'plus' }, { value: TransactionType.Expense, label: 'Expense', icon: 'minus' }];
    
    const amountPlaceholder: string = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref<string>('');

    const submitLabel = ref<string>('');
    const deleteLabel = ref<string>('');
    const areButtonsDisabled = ref<boolean>(false);

    let partialModel: Partial<ITransactionRequest> = {};

    // Init
    onMounted(() => {
        updateAmountDisplayValue();
        setButtonsToDefaultState();
    });

    // On props change
    watch(() => [saveAllResult, savePartialResult, deleteResult], ([newSaveAll, newSavePartial, newDelete]) => {
        if (newSaveAll !== undefined && !newSaveAll.isSuccess) {
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else if (newSavePartial !== undefined && !newSavePartial.isSuccess || isNew) {
            setButtonsToDefaultState();
        }
        else if (newDelete !== undefined && !newDelete.isSuccess) {
            setDeleteButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else {
            setSubmitButtonToSavedState();
            waitAndResetButtonsToDefaultState();
        }
    });

    // On form submit
    const onSubmit = (): void => {
        emitSaveAll(model.value);
        disableButtons();
    }

    // On delete button click
    const onDelete = (): void => {
        emitDelete(model.value.id);
        disableButtons();
    }

    // #region Partial update

    // On type field switch event
    const onTypeFieldChange = (value: ButtonSwitchValue | undefined): void => {
        setButtonsToDefaultState();
        fillPartialModel('type', value as ITransactionRequest['type']);
    }

    // On all form fields input event (any text modification)
    const onFormFieldInput = <T extends keyof ITransactionRequest>(event: Event, fieldName: T): void => {
        const target = event.target as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement | null;
        if (target === null) {
            return;
        }

        setButtonsToDefaultState();
        fillPartialModel(fieldName, target.value as ITransactionRequest[T]);
    }

    const fillPartialModel = <T extends keyof ITransactionRequest>(fieldName: T, value: ITransactionRequest[T]): void => {
        if (isNew) {
            return;
        }

        partialModel[fieldName] = value;
        debounceSavePartial();
    };

    // #endregion Partial update

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

    // #region Buttons

    const setButtonsToDefaultState = (): void => {
        submitLabel.value = isNew ? 'Add transaction' : 'Edit transaction';
        deleteLabel.value = 'Delete transaction';
        enableButtons();
    }

    const setSubmitButtonToSavedState = (): void => {
        submitLabel.value = 'Saved';
        disableButtons();
    }

    const setSubmitButtonToErrorState = (): void => {
        submitLabel.value = 'Error';
        enableButtons();
    }

    const setDeleteButtonToErrorState = (): void => {
        deleteLabel.value = 'Error';
        enableButtons();
    }

    const disableButtons = (): void => {
        areButtonsDisabled.value = true;
    }

    const enableButtons = (): void => {
        areButtonsDisabled.value = false;
    }

    const waitAndResetButtonsToDefaultState = debounce(setButtonsToDefaultState, 5000);

    // #endregion Buttons

    // #region Emits

    const emitSaveAll = (data: ITransactionRequest) => emit('saveAll', data);
    const emitSavePartial = (id: number, data: Partial<ITransactionRequest>) => emit('savePartial', id, data);
    const emitDelete = (id: number) => emit('delete', id);

    const debounceSavePartial = debounce(() => {
        if (model.value.id === undefined || Object.keys(partialModel).length === 0) {
            return;
        }

        emitSavePartial(model.value.id, partialModel);
        partialModel = {};
    }, 1000);

    // #endregion Emits

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
            <button v-if="!isNew" class="transaction-form-button btn btn-outline-danger btn-lg" :disabled="areButtonsDisabled" @click="onDelete">
                <font-awesome-icon icon="fa-solid fa-trash" />
                <span>{{ deleteLabel }}</span>
            </button>
            <button type="submit" class="transaction-form-button btn btn-primary btn-lg" :disabled="areButtonsDisabled || !model.type || !model.amount || !model.reason">
                <font-awesome-icon icon="fa-solid fa-check" />
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
