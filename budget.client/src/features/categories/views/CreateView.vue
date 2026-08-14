<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { type ICategoryRequest, getDefaultCategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';
    import useCreateEntity from '@/composables/useCreateEntity';

    const router = useRouter();

    const category = ref(getDefaultCategoryRequest());

    const onCreateSuccess = () => {
        router.push({ path: routes.category.list });
    };

    const { createEntity, createResult } = useCreateEntity<ICategoryRequest>({ endpoint: 'category', onCreateSuccess });

</script>

<template>
    <CategoryForm
        v-model="category"
        :is-new="true"
        :is-loading="false"
        :is-auto-save="false"
        :save-all-result="createResult"
        @save-all="createEntity"
    />
</template>
