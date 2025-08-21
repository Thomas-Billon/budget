<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import type { Transaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import { formatAmount, parseAmount } from '@/services/TransactionService.ts'

    const model = defineModel<Transaction>({ required: true });
    const emit = defineEmits(['submit']);

    const amountPlaceholder = formatAmount(0, { isFalsyValueAllowed: true });
    const amountDisplayValue = ref('');

    // Init
    onMounted(() => {
        updateAmountDisplayValue();
    });

    // On model change
    watch(() => model.value, (transaction) => {
        updateAmountDisplayValue();
    });

    // On amount input change
    watch(amountDisplayValue, (value) => {
        updateAmountRealValue(value);
    });

    const updateAmountDisplayValue = (): void => {
        amountDisplayValue.value = formatAmount(model.value.amount);
    };

    const updateAmountRealValue = (value: string): void => {
        model.value.amount = parseAmount(value);
    }

    const submit = () => emit('submit', model.value.id);

</script>

<template>
    <form class="section-container grow" @submit.prevent="submit">

        <input type="hidden" id="transaction-id" name="Id" v-model="model.id" />

        <div class="grow-0 flex p-1 gap-1 rounded bg-neutral-200">
            <button type="button" class="button !px-2 !py-1 secondary transition-colors" :class="[ model.type !== TransactionType.Expense ? '' : 'disabled' ]" @click="model.type = TransactionType.Income">
                <font-awesome-icon icon="fa-solid fa-plus" size="sm" />
                <span>Income</span>
            </button>
            <button type="button" class="button !px-2 !py-1 secondary transition-colors" :class="[ model.type !== TransactionType.Income ? '' : 'disabled' ]" @click="model.type = TransactionType.Expense">
                <font-awesome-icon icon="fa-solid fa-minus" size="sm" />
                <span>Expense</span>
            </button>
            <input type="hidden" id="transaction-type" name="Type" v-model="model.type" />
        </div>

        <div class="grow flex flex-col justify-center gap-6 transition-opacity" :class="[ model.type === TransactionType.None ? 'opacity-0' : '' ]">

            <div class="flex items-stretch gap-4 h-24">
                <input class="input text-6xl font-light text-center" type="text" id="transaction-amount" name="Amount" v-model="amountDisplayValue" :placeholder="amountPlaceholder" autocomplete="off" required @change="updateAmountDisplayValue()" @keydown.enter.prevent="$event.currentTarget.blur()" />
                <span class="input shrink-0 flex items-center justify-center text-4xl !w-24 text-center">
                    €
                </span>
            </div>

            <input class="input" type="text" id="transaction-title" name="Title" v-model="model.title" placeholder="Title" />

            <input class="input" type="date" id="transaction-date" name="Date" v-model="model.date" />

            <select class="input !ps-3" id="transaction-payment-method" name="PaymentMethod" v-model="model.paymentMethod">
                <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
            </select>

            <textarea class="input" id="transaction-comment" name="Comment" v-model="model.comment" placeholder="Comment"></textarea>

        </div>
        
        <button type="submit" class="button primary transition-opacity" :class="[ model.type === TransactionType.None ? 'opacity-0' : '' ]" :disabled="!model.amount || !model.title">
            <span>Add transaction</span>
        </button>

    </form>
</template>
