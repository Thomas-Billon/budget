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

    const onUpdateSuccess = () => {
        router.push({ path: routes.category.list });
    };

    const { entity: category, updateEntity, updateResult, updateEntityPartial, updatePartialResult, } = useUpdateEntity<ICategoryRequest, ICategoryDetailsResponse>({ endpoint: 'category', onGetByIdError, onUpdateSuccess });

</script>

<template>
    <CategoryForm :is-new="false"
                  :save-all-result="updateResult"
                  :save-partial-result="updatePartialResult"
                  v-model="category"
                  @save-all="updateEntity"
                  @save-partial="updateEntityPartial" />
</template>
