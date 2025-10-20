<script setup lang="ts">

    import './ListView.scss';

    import StyleVariables from '@/assets/variables.module.scss';

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ICategoryTreeListResponse, type ICategoryTreeListItemResponse } from '@/features/categories/models/ICategoryTreeListResponse';
    import CategoryCard from '@/features/categories/components/CategoryCard.vue';

    const categories = ref<ICategoryTreeListItemResponse[]>([]);

    // Init
    onMounted(() => {
        getCategoryTreeList();
    });

    const getCategoryTreeList = async () => {
        apiCall<undefined, ICategoryTreeListResponse>(`category/tree`, { method: 'GET' })
            .then(response => {
                categories.value = response.items;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

</script>

<template>
    <div class="category-list section-container container">
        <div class="category-list-items row">
            <div v-for="category in categories" :key="category.id" class="col-12 col-sm-6 col-md-4 col-lg-3">
                <CategoryCard v-bind="category" :can-add-sub-categories="true" />
            </div>
            <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                <CategoryCard :color-hex="StyleVariables.secondary">
                    <font-awesome-icon icon="fa-solid fa-plus" />
                </CategoryCard>
            </div>
        </div>
    </div>
</template>
