<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ICategoryDetailsResponse } from '@/features/categories/models/ICategoryDetailsResponse';
    import { type ICategoryRequest, defaultCategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useUpdateEntity from '@/composables/useUpdateEntity';
    import useDeleteEntity from '@/composables/useDeleteEntity';

    const router = useRouter();

    const onGetDetailsError = () => {
        // TODO: Add error
    };

    const onFullUpdateSuccess = () => {
        router.push({ path: routes.category.hierarchy });
    };

    const onDeleteSuccess = () => {
        router.push({ path: routes.category.hierarchy });
    };

    const mapResponseToRequest = (response: ICategoryDetailsResponse): ICategoryRequest => {
        return {
            id: response.id,
            name: response.name,
            color: response.color,
            parentCategoryId: response.parentCategoryId
        };
    };

    const { entity: category, fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult } = useUpdateEntity<ICategoryRequest, ICategoryDetailsResponse>({ endpoint: 'category', defaultEntity: defaultCategoryRequest, mapResponseToRequest, onGetDetailsError, onFullUpdateSuccess });
    const { deleteEntity, deleteResult } = useDeleteEntity({ endpoint: 'category', onDeleteSuccess });

</script>

<template>
    <CategoryForm :is-new="false"
                  :save-all-result="fullUpdateResult"
                  :save-partial-result="partialUpdateResult"
                  :delete-result="deleteResult"
                  v-model="category"
                  @save-all="fullUpdateEntity"
                  @save-partial="partialUpdateEntity"
                  @delete="deleteEntity" />
</template>
