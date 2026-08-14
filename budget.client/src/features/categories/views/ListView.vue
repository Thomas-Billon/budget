<script setup lang="ts">

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ICategoryListResponse, type ICategoryListItemResponse } from '@/features/categories/models/ICategoryListResponse';
    import { routes } from '@/router';
    import CategoryRow from '@/features/categories/components/CategoryRow.vue';
    import PageHeaderActions from '@/components/page-header/PageHeaderActions.vue';

    const isLoading = ref<boolean>(true);
    const categories = ref<ICategoryListItemResponse[]>([]);

    const getCategoryList = () => {
        apiCall<undefined, ICategoryListResponse>('category/list', { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    categories.value = response.data.items;
                }
                // TODO: Handle error in else case
                isLoading.value = false;
            });
    };

    // Init
    onMounted(() => {
        getCategoryList();
    });

</script>

<template>
    <div class="category-list">
        <PageHeaderActions>
            <RouterLink :to="routes.category.create" class="btn btn-primary btn-lg w-100">
                <font-awesome-icon icon="fa-solid fa-plus" />
                <span>Add category</span>
            </RouterLink>
        </PageHeaderActions>

        <div v-if="!isLoading" class="card">
            <div class="d-flex flex-column">
                <CategoryRow
                    v-for="(category, index) in categories"
                    :key="index"
                    :category="category"
                    class="in-card"
                />
            </div>
        </div>

        <div v-else>Loading…</div>
    </div>
</template>
