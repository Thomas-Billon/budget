<script setup lang="ts">

    import { onMounted, ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import { routes } from '@/router.ts';
    import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
    import { type ICategoryDetailsResponse } from '@/features/categories/models/ICategoryDetailsResponse';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import CategoryForm from '@/features/categories/components/CategoryForm.vue';

    const route = useRoute();
    const router = useRouter();

    const category = ref<Partial<ICategoryDetailsResponse>>({});

    const saveAllResult = ref<ApiCallResult>();
    const savePartialResult = ref<ApiCallResult>();

    // Init
    onMounted(() => {
        const categoryId = parseInt(route.params.id as string);
        getCategoryById(categoryId);
    });

    // On route change
    watch(() => route.params.id, (id) => {
        const categoryId = parseInt(id as string);
        getCategoryById(categoryId);
    });

    const getCategoryById = async (id: number) => {
        apiCall<undefined, ICategoryDetailsResponse>(`category/${id}`, { method: 'GET' })
            .then(response => {
                category.value = response;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

    const updateCategoryAll = async (data: Partial<ICategoryRequest>) => {
        apiCall<Partial<ICategoryRequest>, undefined>(`category/${data.id}`, { method: 'PUT', body: data })
            .then(_ => {
                router.push({ path: routes.category.list });
            })
            .catch(() => {
                saveAllResult.value = {
                    isSuccess: false,
                    timestamp: Date.now()
                };
            });
    };

    const updateCategoryPartial = async (id: number, data: Partial<ICategoryRequest>) => {
        apiCall<Partial<ICategoryRequest>, undefined>(`category/${id}`, { method: 'PATCH', body: data })
            .then(_ => {
                savePartialResult.value = {
                    isSuccess: true,
                    timestamp: Date.now()
                };
            })
            .catch(() => {
                savePartialResult.value = {
                    isSuccess: false,
                    timestamp: Date.now()
                };
            });
    };

</script>

<template>
    <CategoryForm :is-new="false"
                  :save-all-result="saveAllResult"
                  :save-partial-result="savePartialResult"
                  v-model="category"
                  @save-all="updateCategoryAll"
                  @save-partial="updateCategoryPartial" />
</template>
