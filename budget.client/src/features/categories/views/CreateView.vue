<script setup lang="ts">

    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
    import type { ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';

    const router = useRouter();

    const category = ref<Partial<ICategoryRequest>>({});

    const saveAllResult = ref<ApiCallResult>();

    const createCategory = async (data: Partial<ICategoryRequest>) => {
        apiCall<Partial<ICategoryRequest>, undefined>('category', { method: 'POST', body: data })
            .then(_ => {
                router.push({ path: routes.category.list });
            })
            .catch(() => {
                saveAllResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    };

</script>

<template>
    <CategoryForm :is-new="true"
                        :save-all-result="saveAllResult"
                        v-model="category"
                        @save-all="createCategory" />
</template>
