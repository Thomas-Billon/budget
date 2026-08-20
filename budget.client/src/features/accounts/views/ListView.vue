<script setup lang="ts">

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type IAccountListResponse, type IAccountListItemResponse } from '@/features/accounts/models/IAccountListResponse';
    import { routes } from '@/router';
    import AccountRow from '@/features/accounts/components/AccountRow.vue';
    import PageHeaderActions from '@/components/page-header/PageHeaderActions.vue';

    const isLoading = ref<boolean>(true);
    const accounts = ref<IAccountListItemResponse[]>([]);

    const getAccountList = () => {
        apiCall<undefined, IAccountListResponse>('account/list', { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    accounts.value = response.data.items;
                }
                // TODO: Handle error in else case
                isLoading.value = false;
            });
    };

    // Init
    onMounted(() => {
        getAccountList();
    });

</script>

<template>
    <div class="account-list">
        <PageHeaderActions>
            <RouterLink :to="routes.account.create" class="btn btn-primary btn-lg w-100">
                <font-awesome-icon icon="fa-solid fa-plus" />
                <span>Add account</span>
            </RouterLink>
        </PageHeaderActions>

        <div v-if="!isLoading" class="card">
            <div class="d-flex flex-column">
                <AccountRow
                    v-for="(account, index) in accounts"
                    :key="index"
                    :account="account"
                    class="in-card"
                />
            </div>
        </div>

        <div v-else>Loading…</div>
    </div>
</template>
