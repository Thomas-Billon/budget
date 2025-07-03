<template>
    <form class="flex flex-col items-center">
        <div class="flex items-center">
            <button>
                <font-awesome-icon icon="fa-solid fa-plus" />
                Income
            </button>
            <button>
                <font-awesome-icon icon="fa-solid fa-minus" />
                Expense
            </button>
        </div>
        <div class="flex items-center">
            <div class="flex items-center text-4xl">
                <input class="text-center" type="text" id="transaction-amount" name="Amount" placeholder="0,00€" />
                <label for="transaction-amount">€</label>
            </div>
        </div>
    </form>
</template>
