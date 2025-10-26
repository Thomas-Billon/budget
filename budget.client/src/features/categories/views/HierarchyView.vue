<script setup lang="ts">

    import './HierarchyView.scss';

    import StyleVariables from '@/assets/variables.module.scss';

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ICategoryHierarchyResponse, type ICategoryHierarchyItemResponse } from '@/features/categories/models/ICategoryHierarchyResponse';
    import CategoryCard from '@/features/categories/components/CategoryCard.vue';

    const categories = ref<ICategoryHierarchyItemResponse[]>([]);

    // Init
    onMounted(() => {
        getCategoryHierarchy();
    });

    const getCategoryHierarchy = async () => {
        apiCall<undefined, ICategoryHierarchyResponse>(`category/hierarchy`, { method: 'GET' })
            .then(response => {
                categories.value = response.items;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

</script>

<template>
    <div class="category-hierarchy section-container container">
        <div class="category-hierarchy-items row">
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
