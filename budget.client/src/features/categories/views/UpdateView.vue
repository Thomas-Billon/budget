<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ICategoryDetailsResponse } from '@/features/categories/models/ICategoryDetailsResponse';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useUpdateEntity from '@/composables/useUpdateEntity';

    const router = useRouter();

    const onGetByIdError = () => {
        // TODO: Add error
    };

    const onFullUpdateSuccess = () => {
        router.push({ path: routes.category.hierarchy });
    };

    const { entity: category, fullUpdate, fullUpdateResult, partialUpdate, partialUpdateResult } = useUpdateEntity<ICategoryRequest, ICategoryDetailsResponse>({ endpoint: 'category', onGetByIdError, onFullUpdateSuccess });

</script>

<template>
    <CategoryForm :is-new="false"
                  :save-all-result="fullUpdateResult"
                  :save-partial-result="partialUpdateResult"
                  v-model="category"
                  @save-all="fullUpdate"
                  @save-partial="partialUpdate" />
</template>
