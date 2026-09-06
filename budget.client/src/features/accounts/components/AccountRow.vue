<script setup lang="ts">

    import './AccountRow.scss';

    import { computed } from 'vue';
    import { Bank } from '@/enums/Bank';
    import { Currency } from '@/enums/Currency';
    import ClickableRow from '@/components/clickable-row/ClickableRow.vue';
    import { routes } from '@/router.ts';

    interface AccountRowProps {
        id: number;
        name: string;
        bank: Bank;
        currency: Currency;
        transactionCount: number;
    }

    interface Props {
        account: AccountRowProps;
    };

    const { account } = defineProps<Props>();
    const subtitle = computed(() => `${Bank[account.bank]} · ${Currency[account.currency]}`);

</script>

<template>
    <ClickableRow :target="routes.account.update(account.id)">
        <div class="account-row-info">
            <div class="account-row-text">
                <p class="account-row-title">{{ account.name }}</p>
                <p class="account-row-subtitle">{{ subtitle }}</p>
            </div>
        </div>
    </ClickableRow>
</template>
