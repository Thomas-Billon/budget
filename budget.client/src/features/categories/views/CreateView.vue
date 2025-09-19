<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import type { ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import { type ITransactionDetailsResponse } from '@/features/transactions/models/ITransactionDetailsResponse';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';

    const router = useRouter();

    const onCreateSuccess = () => {
        router.push({ path: routes.category.list });
    };

    const {
        entity: category,
        createEntity: createCategory,
        createResult: saveAllResult
    } = useCreateEntity<ICategoryRequest, ITransactionDetailsResponse>({ endpoint: 'category', onCreateSuccess });

</script>

<template>
    <CategoryForm :is-new="true"
                  :save-all-result="saveAllResult"
                  v-model="category"
                  @save-all="createCategory" />
</template>
