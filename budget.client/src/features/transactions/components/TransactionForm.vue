<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import type { Transaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import { formatAmount, parseAmount } from '@/features/transactions/TransactionService.ts'

    const { isNew, saveAllResult, savePartialResult } = defineProps(['isNew', 'saveAllResult', 'savePartialResult']);
    const model = defineModel<Transaction>({ required: true });
    const emit = defineEmits(['saveAll', 'savePartial']);

    const amountPlaceholder = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref('');
    const submitLabel = ref('');
    const isSubmitDisabled = ref(false);

    let partialModel = {};

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
    watch(() => model.value, (transaction) => {
        updateAmountDisplayValue();
    });

    // On form submit
    const onSubmit = (): void => {
        setSubmitButtonToSavedState();
        saveAll();
    }

    // On all form fields input event (any text modification)
    const onFormFieldInput = (event: Event, fieldName: string): void => {
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
    <form class="section-container grow" @submit.prevent="onSubmit">

        <input type="hidden" id="transaction-id" name="Id" v-model="model.id" />

        <div class="grow-0 flex p-1 gap-1 rounded bg-neutral-200">
            <button type="button" class="button !px-2 !py-1 secondary transition-colors" :class="[ model.type !== TransactionType.Expense ? '' : 'disabled' ]" @click="model.type = TransactionType.Income; onFormFieldInput($event, 'type');">
                <font-awesome-icon icon="fa-solid fa-plus" size="sm" />
                <span>Income</span>
            </button>
            <button type="button" class="button !px-2 !py-1 secondary transition-colors" :class="[ model.type !== TransactionType.Income ? '' : 'disabled' ]" @click="model.type = TransactionType.Expense; onFormFieldInput($event, 'type');">
                <font-awesome-icon icon="fa-solid fa-minus" size="sm" />
                <span>Expense</span>
            </button>
            <input type="hidden" id="transaction-type" name="Type" v-model="model.type" />
        </div>

        <div class="grow flex flex-col justify-center gap-6 transition-opacity" :class="[ model.type === TransactionType.None ? 'opacity-0' : '' ]">

            <div class="flex items-stretch gap-4 h-24">
                <input class="input text-6xl font-light text-center"
                       type="text"
                       id="transaction-amount"
                       name="Amount"
                       v-model="amountDisplayValue"
                       :placeholder="amountPlaceholder"
                       autocomplete="off"
                       required
                       @input="onAmountInput($event); onFormFieldInput($event, 'amount');"
                       @change="onAmountChange($event)" />
                <span class="input shrink-0 flex items-center justify-center text-4xl !w-24 text-center">
                    €
                </span>
            </div>

            <input class="input" type="text" id="transaction-title" name="Title" v-model="model.title" placeholder="Title" required @input="onFormFieldInput($event, 'title');" />

            <input class="input" type="date" id="transaction-date" name="Date" v-model="model.date" @input="onFormFieldInput($event, 'date');" />

            <select class="input !ps-3" id="transaction-payment-method" name="PaymentMethod" v-model="model.paymentMethod" @input="onFormFieldInput($event, 'paymentMethod');">
                <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
            </select>

            <textarea class="input" id="transaction-comment" name="Comment" v-model="model.comment" placeholder="Comment" @input="onFormFieldInput($event, 'comment');"></textarea>

        </div>

        <div class="transition-opacity" :class="[ model.type === TransactionType.None ? 'opacity-0' : '' ]">
            <button type="submit" class="button primary" :disabled="isSubmitDisabled || model.type === TransactionType.None || !model.amount || !model.title">
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
