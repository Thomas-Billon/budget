<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ICategoryRequest, getDefaultCategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';
    import { getIdFromRoute } from '@/utils/Route';
    import useMountedOrRouteParamUpdate from '@/composables/useMountedOrRouteParamUpdate';

    const router = useRouter();

    const category = ref(getDefaultCategoryRequest());

    useMountedOrRouteParamUpdate((params) => {
        if (params?.parentCategoryId) {
            const parentCategoryId = getIdFromRoute(params?.parentCategoryId);

            if (parentCategoryId) {
                category.value.parentCategoryId = parentCategoryId;
            }
        }
    });

    const onCreateSuccess = () => {
        router.push({ path: routes.category.hierarchy });
    };

    const { createEntity, createResult } = useCreateEntity<ICategoryRequest>({ endpoint: 'category', onCreateSuccess });

</script>

<template>
    <CategoryForm :is-new="true"
                  :save-all-result="createResult"
                  v-model="category"
                  @save-all="createEntity" />
</template>
