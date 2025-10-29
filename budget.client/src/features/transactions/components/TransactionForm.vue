<script setup lang="ts">

    import './TransactionForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import ButtonSwitch from '@/components/button-switch/ButtonSwitch.vue';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts'
    import { type ITransactionRequest } from '@/features/transactions/models/ITransactionRequest';
    import { type IButtonSwitchEvent, type IButtonSwitchOption, isButtonSwitchEvent } from '@/components/button-switch/ButtonSwitchValue';
    import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
    import { type ICategoryOptionsItemResponse, type ICategoryOptionsResponse } from '@/features/categories/models/ICategoryOptionsResponse';

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

    let partialModel: Partial<ITransactionRequest> = {};

    const submitLabel = ref<string>('');
    const deleteLabel = ref<string>('');
    const isSubmitButtonDisabled = ref<boolean>(false);
    const isDeleteButtonDisabled = ref<boolean>(false);

    const typeInput = ref<HTMLInputElement | undefined>();
    const typeOptions: IButtonSwitchOption[] = [{ value: TransactionType.Income, label: 'Income', icon: 'plus' }, { value: TransactionType.Expense, label: 'Expense', icon: 'minus' }];

    const amountPlaceholder: string = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref<string>('');

    const categoryOptions = ref<ICategoryOptionsItemResponse[]>([]);

    // #region Init

    onMounted(() => {
        updateAmountDisplayValue();
        getCategoryOptions();
        setButtonsToDefaultState();
    });

    // #endregion Init

    //

    // #region Actions

    // On form submit
    const onSubmit = (): void => {
        if (!isFormValid()) {
            return;
        }

        disableButtons();
        emitSaveAll(model.value);
    }

    // On delete button click
    const onDelete = (): void => {
        disableButtons();
        emitDelete(model.value.id);
    }

    // #endregion Actions

    // #region API call results

    watch(() => saveAllResult, (result) => {
        if (result !== undefined && !result.isSuccess) {
            // Error on save all -> set submit button to error state for a while
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        // Success on save all -> nothing to do, form will be exited
    });

    watch(() => savePartialResult, (result) => {
        if (result !== undefined && !result.isSuccess || isNew) {
            // Error on save partial -> set submit button to error state for a while
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else {
            // Success on save partial -> set submit button to saved state until next edit but keep delete button enabled
            setSubmitButtonToSavedState();
            enableDeleteButton();
        }
    });

    watch(() => deleteResult, (result) => {
        if (result !== undefined && !result.isSuccess) {
            // Error on delete -> set delete button to error state for a while
            setDeleteButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        // Success on delete -> nothing to do, form will be exited
    });

    // #endregion API call results

    // #region Check form

    const isFormValid = (): boolean => {
        return model.value.type !== undefined && model.value.type !== TransactionType.None
            && model.value.amount !== undefined && model.value.amount > 0
            && model.value.reason !== undefined && model.value.reason.trim().length > 0
            && model.value.date !== undefined;
    };

    // #endregion Check form

    // #region Partial update

    const onFieldChange = <T extends keyof ITransactionRequest>(event: IButtonSwitchEvent | Event | undefined, fieldName: T): void => {
        setButtonsToDefaultState();

        if (isButtonSwitchEvent(event)) {
            fillPartialModel(fieldName, event.value as ITransactionRequest[T]);
            return;
        }

        if (event instanceof Event) {
            const target = event.target as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement | null;
            if (!target) {
                return;
            }

            if (target instanceof HTMLSelectElement && target.multiple) {
                const value = Array.from(target.selectedOptions, opt => opt.value);
                fillPartialModel(fieldName, value as unknown[] as ITransactionRequest[T]);
                return;
            }
            else {
                fillPartialModel(fieldName, target.value as ITransactionRequest[T]);
                return;
            }
        }

    };

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
        disableSubmitButton();
        disableDeleteButton();
    }

    const disableSubmitButton = (): void => {
        isSubmitButtonDisabled.value = true;
    }

    const disableDeleteButton = (): void => {
        isDeleteButtonDisabled.value = true;
    }

    const enableButtons = (): void => {
        enableSubmitButton();
        enableDeleteButton();
    }

    const enableSubmitButton = (): void => {
        isSubmitButtonDisabled.value = false;
    }

    const enableDeleteButton = (): void => {
        isDeleteButtonDisabled.value = false;
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

        disableButtons();
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
            <ButtonSwitch v-model="model.type" :options="typeOptions" class-name="bg-secondary bg-opacity-50" @change="onFieldChange($event, 'type')" />
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
                       @input="onAmountInput($event); onFieldChange($event, 'amount');"
                       @change="onAmountChange($event)" />
                <span class="input-group-text">
                    €
                </span>
            </div>

            <input class="form-control form-control-lg" type="text" id="transaction-reason" name="Reason" v-model="model.reason" placeholder="Reason" required @input="onFieldChange($event, 'reason');" />

            <input class="form-control form-control-lg" type="date" id="transaction-date" name="Date" v-model="model.date" required @input="onFieldChange($event, 'date');" />

            <select class="form-select form-select-lg" id="transaction-payment-method" name="PaymentMethod" v-model="model.paymentMethod" @change="onFieldChange($event, 'paymentMethod');">
                <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
            </select>

            <textarea class="form-control form-control-lg" id="transaction-comment" name="Comment" v-model="model.comment" placeholder="Comment" @input="onFieldChange($event, 'comment');"></textarea>

            <select class="form-select form-select-lg" id="transaction-category-ids" name="CategoryIds" v-model="model.categoryIds" multiple @change="onFieldChange($event, 'categoryIds');">
                <option v-for="option in categoryOptions" :key="option.id" :value="option.id">{{ option.name }}</option>
            </select>

        </div>

        <div class="transaction-form-foot transition-opacity" :class="[ !model.type && 'hidden' ]">
            <button v-if="!isNew" class="transaction-form-button btn btn-outline-danger btn-lg" :disabled="isDeleteButtonDisabled" @click="onDelete">
                <font-awesome-icon icon="fa-solid fa-trash" />
                <span>{{ deleteLabel }}</span>
            </button>
            <button type="submit" class="transaction-form-button btn btn-primary btn-lg" :disabled="isSubmitButtonDisabled || !isFormValid()">
                <font-awesome-icon icon="fa-solid fa-check" />
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
