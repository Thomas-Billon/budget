<script setup lang="ts">

    import { ref, watch } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type IAccountDetailsResponse } from '@/features/accounts/models/IAccountDetailsResponse';
    import { type IAccountRequest, getDefaultAccountRequest } from '@/features/accounts/models/IAccountRequest';
    import AccountForm from '@/features/accounts/components/AccountForm.vue';
    import useGetEntity from '@/composables/useGetEntity';
    import useUpdateEntity from '@/composables/useUpdateEntity';
    import useDeleteEntity from '@/composables/useDeleteEntity';
    import useMountedOrRouteParamUpdate from '@/composables/useMountedOrRouteParamUpdate';
    import { getIdFromRoute } from '@/utils/Route';

    const router = useRouter();

    const isLoading = ref<boolean>(true);
    const account = ref(getDefaultAccountRequest());

    useMountedOrRouteParamUpdate((params) => {
        const id = getIdFromRoute(params?.id);

        if (id) {
            getEntity(id);
        }
    });

    const onGetError = () => {
        // TODO: Add error
    };

    const onFullUpdateSuccess = () => {
        router.push({ path: routes.account.list });
    };

    const onDeleteSuccess = () => {
        router.push({ path: routes.account.list });
    };

    const endpoint = 'account';

    const { entity, getEntity } = useGetEntity<IAccountDetailsResponse>({ endpoint, onGetError });
    const { fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult } = useUpdateEntity<IAccountRequest>({ endpoint, onFullUpdateSuccess });
    const { deleteEntity, deleteResult } = useDeleteEntity({ endpoint, onDeleteSuccess });

    // Convert entity from db to request object
    watch(entity, (result) => {
        if (result) {
            account.value = {
                id: result.id,
                name: result.name,
                bank: result.bank,
                currency: result.currency
            };
            isLoading.value = false;
        }
    });

</script>

<template>
    <AccountForm
        v-model="account"
        :is-new="false"
        :is-loading="isLoading"
        :is-auto-save="false"
        :save-all-result="fullUpdateResult"
        :save-partial-result="partialUpdateResult"
        :delete-result="deleteResult"
        @save-all="fullUpdateEntity"
        @save-partial="partialUpdateEntity"
        @delete="deleteEntity"
    />
</template>
