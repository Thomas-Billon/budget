<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type IAccountRequest, getDefaultAccountRequest } from '@/features/accounts/models/IAccountRequest';
    import AccountForm from '@/features/accounts/components/AccountForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';

    const router = useRouter();

    const account = ref(getDefaultAccountRequest());

    const onCreateSuccess = () => {
        router.push({ path: routes.account.list });
    };

    const { createEntity, createResult } = useCreateEntity<IAccountRequest>({ endpoint: 'account', onCreateSuccess });

</script>

<template>
    <AccountForm
        v-model="account"
        :is-new="true"
        :is-loading="false"
        :is-auto-save="false"
        :save-all-result="createResult"
        @save-all="createEntity"
    />
</template>
