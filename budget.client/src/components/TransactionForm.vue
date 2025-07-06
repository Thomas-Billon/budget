<script setup lang="ts">
    import { ref } from 'vue';
    import TransactionType from '../enums/TransactionType.ts'
    import PaymentMethod from '../enums/PaymentMethod.ts'

    const onAmountChange = (event): void => {
        const oldValue = event.target.value;
        const newValue = oldValue.replace(/[^0-9 .,]/g, '').replace(',', '.');
        const newValueFormatted = formatAmount(newValue);

        event.target.value = newValueFormatted;
    }

    const formatAmount = (value: string): string => {
        return value ? parseFloat(value).toLocaleString(undefined, { minimumFractionDigits: 2 }) : '';
    }

    const type = ref(TransactionType.None);
    const amountPlaceholder = formatAmount('0.00');
</script>

<template>
    <form class="test-red container h-full min-h-screen flex flex-col items-center justify-center gap-6">
        <div class="flex gap-12">
            <button type="button" class="flex items-center rounded py-2 px-4 gap-1 bg-emerald-400 text-lg text-white cursor-pointer" @click="type = TransactionType.Income">
                <font-awesome-icon icon="fa-solid fa-plus" />
                <span>Income</span>
            </button>
            <button type="button" class="flex items-center rounded py-2 px-4 gap-1 bg-red-400 text-lg text-white cursor-pointer" @click="type = TransactionType.Expense">
                <font-awesome-icon icon="fa-solid fa-minus" />
                <span>Expense</span>
            </button>
        </div>
        <div class="relative flex items-center mx-16">
            <span class="absolute -left-8 -translate-x-1/2 text-3xl">
                <font-awesome-icon v-if="type === TransactionType.Income" icon="fa-solid fa-plus" />
                <font-awesome-icon v-else-if="type === TransactionType.Expense" icon="fa-solid fa-minus" />
            </span>
            <label class="hidden" for="transaction-amount">Amount</label>
            <input class="text-center text-6xl w-full pb-2 outline-0" type="text" id="transaction-amount" name="Amount" :placeholder="amountPlaceholder" autocomplete="off" @change="onAmountChange($event)" />
            <span class="absolute -right-8 translate-x-1/2 text-3xl">
                <font-awesome-icon icon="fa-solid fa-euro" />
            </span>
        </div>
        <div>
            <input type="date" id="transaction-date" name="Date" />
        </div>
        <div>
            <select class="text-center" id="transaction-payment-method" name="PaymentMethod">
                <option :value="PaymentMethod.None" disabled selected>Select Payment Method</option>
                <option :value="PaymentMethod.Cash">{{ PaymentMethod[PaymentMethod.Cash] }}</option>
                <option :value="PaymentMethod.CreditCard">{{ PaymentMethod[PaymentMethod.CreditCard] }}</option>
                <option :value="PaymentMethod.DebitCard">{{ PaymentMethod[PaymentMethod.DebitCard] }}</option>
                <option :value="PaymentMethod.BankTransfer">{{ PaymentMethod[PaymentMethod.BankTransfer] }}</option>
                <option :value="PaymentMethod.Cryptocurrency">{{ PaymentMethod[PaymentMethod.Cryptocurrency] }}</option>
                <option :value="PaymentMethod.Other">{{ PaymentMethod[PaymentMethod.Other] }}</option>
            </select>
        </div>
        <div class="w-full bg-neutral-200">
            <textarea class="w-full outline-0 text-center" id="transaction-comment" name="Comment" placeholder="Comment" rows="3"></textarea>
        </div>
    </form>
</template>
