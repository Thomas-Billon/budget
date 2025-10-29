<script setup lang="ts">

    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ICategoryRequest, defaultCategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import { type ICategoryDetailsResponse } from '@/features/categories/models/ICategoryDetailsResponse';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';

    const router = useRouter();

    const onCreateSuccess = () => {
        router.push({ path: routes.category.hierarchy });
    };

    const { entity: category, createEntity, createResult } = useCreateEntity<ICategoryRequest, ICategoryDetailsResponse>({ endpoint: 'category', defaultEntity: defaultCategoryRequest, onCreateSuccess });

    category.value.parentCategoryId = history.state?.parentCategoryId;

</script>

<template>
    <CategoryForm :is-new="true"
                  :save-all-result="createResult"
                  v-model="category"
                  @save-all="createEntity" />
</template>
