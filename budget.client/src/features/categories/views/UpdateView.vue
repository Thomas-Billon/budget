<script setup lang="ts">

    import { ref, watch } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ICategoryDetailsResponse } from '@/features/categories/models/ICategoryDetailsResponse';
    import { type ICategoryRequest, getDefaultCategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useGetEntity from '@/composables/useGetEntity';
    import useUpdateEntity from '@/composables/useUpdateEntity';
    import useDeleteEntity from '@/composables/useDeleteEntity';
    import useMountedOrRouteParamUpdate from '@/composables/useMountedOrRouteParamUpdate';
    import { getIdFromRoute } from '@/utils/Route';

    const router = useRouter();

    const category = ref(getDefaultCategoryRequest());

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
        router.push({ path: routes.category.hierarchy });
    };

    const onDeleteSuccess = () => {
        router.push({ path: routes.category.hierarchy });
    };

    const endpoint = 'category';

    const { entity, getEntity } = useGetEntity<ICategoryDetailsResponse>({ endpoint, onGetError });
    const { fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult } = useUpdateEntity<ICategoryRequest>({ endpoint, onFullUpdateSuccess });
    const { deleteEntity, deleteResult } = useDeleteEntity({ endpoint, onDeleteSuccess });

    // Convert entity from db to request object
    watch(entity, (result) => {
        if (result) {
            category.value = {
                id: result.id,
                name: result.name,
                color: result.color,
                parentCategoryId: result.parentCategoryId
            };
        }
    });

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
