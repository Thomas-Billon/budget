<script setup lang="ts">

    import { watch, ref } from 'vue';
    import type { Transaction } from '@/models/Transaction.ts';
    import { TransactionType } from '@/enums/TransactionType.ts';
    import { PaymentMethod } from '@/enums/PaymentMethod.ts';
    import { formatAmount, parseAmount } from '@/services/TransactionService.ts'

    const model = defineModel<Transaction>({ required: true });
    const emit = defineEmits(['submit']);

    const submit = () => emit('submit');

    const amountInputValue = ref(formatAmount(model.value.amount));
    const amountPlaceholder = formatAmount(0, { isFalsyValueAllowed: true });

    // On input event
    watch(amountInputValue, (inputValue) => {
        model.value.amount = parseAmount(inputValue);
    });

    // On change event
    const onChangeAmountInput = (): void => {
        amountInputValue.value = formatAmount(model.value.amount);
    };

</script>

<template>
    <form class="section-container grow" @submit.prevent="submit">

        <input type="hidden" id="transaction-id" name="Id" v-model="model.id" />

        <div class="grow-0 flex p-1 gap-1 rounded bg-neutral-200">
            <button type="button" class="grow flex items-center justify-center py-1 px-2 gap-2 rounded font-medium cursor-pointer" :class="[ model.type !== TransactionType.Expense ? 'bg-white' : 'text-neutral-400' ]" @click="model.type = TransactionType.Income">
                <font-awesome-icon icon="fa-solid fa-plus" size="sm" />
                <span>Income</span>
            </button>
            <button type="button" class="grow flex items-center justify-center py-1 px-2 gap-2 rounded font-medium cursor-pointer" :class="[ model.type !== TransactionType.Income ? 'bg-white' : 'text-neutral-400' ]" @click="model.type = TransactionType.Expense">
                <font-awesome-icon icon="fa-solid fa-minus" size="sm" />
                <span>Expense</span>
            </button>
            <input type="hidden" id="transaction-type" name="Type" v-model="model.type" />
        </div>

        <div class="grow flex flex-col justify-center gap-6 transition-opacity" :class="[ model.type === TransactionType.None ? 'opacity-0' : '' ]">

            <div class="flex items-stretch gap-4 h-24">
                <input class="input text-6xl font-light text-center" type="text" id="transaction-amount" name="Amount" v-model="amountInputValue" :placeholder="amountPlaceholder" autocomplete="off" required @change="onChangeAmountInput()" @keydown.enter.prevent="$event.currentTarget.blur()" />
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
